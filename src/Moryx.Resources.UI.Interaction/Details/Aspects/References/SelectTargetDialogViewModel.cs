// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using Moryx.ClientFramework.Commands;
using Moryx.ClientFramework.Dialog;
using Moryx.ClientFramework.Tasks;
using Moryx.Resources.UI.Interaction.Properties;
using Moryx.Resources.UI.ResourceService;

namespace Moryx.Resources.UI.Interaction.Aspects
{
    internal class SelectTargetDialogViewModel : DialogScreen
    {
        private readonly IResourceServiceModel _resourceServiceModel;

        public ResourceReferenceViewModel Reference { get; }

        public override string DisplayName => Strings.SelectTargetDialogViewModel_DisplayName;

        public ICommand SelectCmd { get; set; }

        public ICommand CancelCmd { get; set; }

        private TaskNotifier _taskNotifier;

        public TaskNotifier TaskNotifier
        {
            get { return _taskNotifier; }
            private set
            {
                _taskNotifier = value;
                NotifyOfPropertyChange(nameof(TaskNotifier));
            }
        }

        private string _errorMessage;
        private ResourceInfoViewModel _selectedTarget;

        /// <summary>
        /// Displays general error messages if something was wrong
        /// </summary>
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                NotifyOfPropertyChange();
            }
        }

        public SelectTargetDialogViewModel(ResourceReferenceViewModel reference,
            IResourceServiceModel resourceServiceModel)
        {
            _resourceServiceModel = resourceServiceModel;
            Reference = reference;

            SelectCmd = new AsyncCommand(Select, CanSelect, true);
            CancelCmd = new AsyncCommand(Cancel, _ => true, true);
        }

        public ResourceInfoViewModel[] PossibleTargets { get; private set; }

        public ResourceInfoViewModel SelectedTarget
        {
            get { return _selectedTarget; }
            set
            {
                _selectedTarget = value;
                NotifyOfPropertyChange();
            }
        }

        /// <inheritdoc />
        protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            await base.OnInitializeAsync(cancellationToken);

            var loadingTask = Task.Run(async delegate
            {
                try
                {
                    var supportedTypes = Reference.Type.Model.SupportedTypes;
                    var possibleTargetsLoadTask = _resourceServiceModel.GetResources(new ResourceQuery { Types = supportedTypes });

                    TaskNotifier = new TaskNotifier(possibleTargetsLoadTask);

                    var possibleTargets = await possibleTargetsLoadTask;
                    var possibleTargetsVms = possibleTargets.Where(resource => Reference.Targets.All(t => t.Id != resource.Id))
                        .Select(possible => new ResourceInfoViewModel(possible));

                    Execute.OnUIThread(delegate
                    {
                        PossibleTargets = possibleTargetsVms.ToArray();
                        SelectedTarget = PossibleTargets.FirstOrDefault();
                        NotifyOfPropertyChange(nameof(PossibleTargets));
                    });
                }
                catch (Exception e)
                {
                    Execute.OnUIThread(() => ErrorMessage = e.Message);
                }
            }, cancellationToken);

            TaskNotifier = new TaskNotifier(loadingTask);
        }

        private bool CanSelect(object obj) =>
            SelectedTarget != null;

        private Task Select(object obj) =>
            TryCloseAsync(true);

        private Task Cancel(object obj) =>
            TryCloseAsync(false);
    }
}
