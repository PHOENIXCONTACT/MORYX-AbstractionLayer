// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Moryx.ClientFramework.Commands;
using Moryx.Controls;
using Moryx.Resources.UI.Interaction.Properties;
using Moryx.Serialization;

namespace Moryx.Resources.UI.Interaction.Aspects
{
    [ResourceAspectRegistration(nameof(ResourceMethodsAspectViewModel))]
    internal class ResourceMethodsAspectViewModel : ResourceAspectViewModelBase
    {
        public override string DisplayName => Strings.ResourceMethodsAspectViewModel_DisplayName;

        #region Dependencies

        public IResourceServiceModel ResourceServiceModel { get; set; }

        #endregion

        public ICommand MethodInvokeCmd { get; set; }

        private EntryViewModel _methodInvocationResult;
        private ResourceMethodViewModel _selectedMethod;

        /// <summary>
        /// Selected Method in UI
        /// </summary>
        public ResourceMethodViewModel SelectedMethod
        {
            get { return _selectedMethod; }
            set
            {
                _selectedMethod = value;
                MethodInvocationResult = null;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Result of a method call
        /// </summary>
        public EntryViewModel MethodInvocationResult
        {
            get { return _methodInvocationResult; }
            set
            {
                if (_methodInvocationResult is null || value is null)
                {
                    _methodInvocationResult = value;
                    NotifyOfPropertyChange();
                }
                else
                    _methodInvocationResult.UpdateModel(value.Entry);
            }
        }

        protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            await base.OnInitializeAsync(cancellationToken);

            MethodInvokeCmd = new AsyncCommand(InvokeMethod, CanInvokeMethod, true);
        }

        public override bool IsRelevant(ResourceViewModel resource)
        {
            return resource.Methods.Any();
        }

        private static bool CanInvokeMethod(object parameters)
        {
            return parameters is ResourceMethodViewModel;
        }

        private async Task InvokeMethod(object parameters)
        {
            var vm = (ResourceMethodViewModel) parameters;
            vm.CopyToModel();

            var result = await ResourceServiceModel.InvokeMethod(Resource.Id, ((ResourceMethodViewModel) parameters).Model);
            var resultEntry = result.ToSerializationEntry();

            if (resultEntry == null)
                MethodInvocationResult = new EntryViewModel(new Entry());
            else if (resultEntry.Value.Type >= EntryValueType.Class)
                MethodInvocationResult = new EntryViewModel(resultEntry);
            else
                MethodInvocationResult = new EntryViewModel(new List<Entry> { resultEntry });
        }

        /// <inheritdoc />
        public override void BeginEdit()
        {
            SelectedMethod?.BeginEdit();
            MethodInvocationResult?.BeginEdit();
            base.BeginEdit();
        }

        /// <inheritdoc />
        public override void EndEdit()
        {
            SelectedMethod?.EndEdit();
            MethodInvocationResult?.EndEdit();
            base.EndEdit();
        }

        /// <inheritdoc />
        public override void CancelEdit()
        {
            SelectedMethod?.CancelEdit();
            MethodInvocationResult?.CancelEdit();
            base.CancelEdit();
        }
    }
}
