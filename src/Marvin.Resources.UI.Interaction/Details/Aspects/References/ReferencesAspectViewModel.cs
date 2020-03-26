// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using C4I;
using Marvin.ClientFramework.Commands;
using Marvin.ClientFramework.Dialog;
using Marvin.Resources.UI.Interaction.Properties;

namespace Marvin.Resources.UI.Interaction.Aspects
{
    [ResourceAspectRegistration(nameof(ReferencesAspectViewModel))]
    internal class ReferencesAspectViewModel : ResourceAspectViewModelBase
    {
        #region Dependencies

        public IResourceServiceModel ResourceServiceModel { get; set; }

        public IDialogManager DialogManager { get; set; }

        #endregion

        #region Fields and Properties

        public override string DisplayName => Strings.ReferencesAspectViewModel_DisplayName;

        private ResourceReferenceViewModel _selectedReference;
        private IResourceViewModel _selectedTarget;

        public ICommand SetTargetCmd { get; }

        public ICommand AddTargetCmd { get; }

        public ICommand RemoveTargetCmd { get; }

        public ResourceReferenceViewModel SelectedReference
        {
            get { return _selectedReference; }
            set
            {
                _selectedReference = value;
                NotifyOfPropertyChange();

                if (_selectedReference != null && !_selectedReference.IsCollection)
                    SelectedTarget = _selectedReference.Targets.FirstOrDefault();
            }
        }

        public IResourceViewModel SelectedTarget
        {
            get { return _selectedTarget; }
            set
            {
                _selectedTarget = value;
                NotifyOfPropertyChange();
            }
        }

        #endregion

        public ReferencesAspectViewModel()
        {
            SetTargetCmd = new AsyncCommand(SetTarget, CanSetTarget, true);
            AddTargetCmd = new AsyncCommand(AddTarget, CanAddTarget, true);
            RemoveTargetCmd = new RelayCommand(RemoveTarget, CanRemoveTarget);
        }

        public override bool IsRelevant(ResourceViewModel resource)
        {
            return resource.References.Any();
        }

        public override async Task Load(ResourceViewModel resource)
        {
            await base.Load(resource);

            SelectedReference = Resource.References.OrderBy(r => r.DisplayName).FirstOrDefault();
            SelectedTarget = SelectedReference?.Targets.FirstOrDefault();
        }

        public override void Validate(ICollection<ValidationResult> validationErrors)
        {
            base.Validate(validationErrors);

            foreach (var reference in Resource.References)
            {
                var referenceType = reference.Type;
                if (!referenceType.IsRequired)
                    continue;

                if (reference.AnyReference)
                    continue;

                validationErrors.Add(referenceType.IsCollection
                    ? new ValidationResult($"{reference.DisplayName}: {Strings.ReferencesAspectViewModel_Validation_CollectionTargetMustBeSet}")
                    : new ValidationResult($"{reference.DisplayName}: {Strings.ReferencesAspectViewModel_Validation_SingleTargetMustBeSet}"));
            }
        }

        private bool CanSetTarget(object parameters) =>
            SelectedReference != null && !SelectedReference.IsCollection && IsEditMode && ResourceServiceModel.IsAvailable;

        private async Task SetTarget(object parameters)
        {
            var dialog = new SelectTargetDialogViewModel(SelectedReference, ResourceServiceModel);
            await DialogManager.ShowDialogAsync(dialog);
            if (!dialog.Result || dialog.SelectedTarget == null)
                return;

            SelectedReference.Targets.Clear();
            SelectedReference.Targets.Add(dialog.SelectedTarget);
            SelectedTarget = dialog.SelectedTarget;
        }

        private bool CanAddTarget(object arg) =>
            SelectedReference != null && SelectedReference.IsCollection && IsEditMode && ResourceServiceModel.IsAvailable;

        private async Task AddTarget(object arg)
        {
            var dialog = new SelectTargetDialogViewModel(SelectedReference, ResourceServiceModel);
            await DialogManager.ShowDialogAsync(dialog);
            if (!dialog.Result || dialog.SelectedTarget == null)
                return;

            SelectedReference.Targets.Add(dialog.SelectedTarget);
        }

        private bool CanRemoveTarget(object parameters) =>
            SelectedTarget != null && IsEditMode && ResourceServiceModel.IsAvailable;

        private void RemoveTarget(object parameters)
        {
            SelectedReference.Targets.Remove(SelectedTarget);
            if (!SelectedReference.IsCollection)
                SelectedTarget = null;
        }
    }
}
