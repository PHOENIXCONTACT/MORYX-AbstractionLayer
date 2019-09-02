using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Marvin.ClientFramework.Commands;
using Marvin.Controls;
using Marvin.Serialization;

namespace Marvin.Resources.UI.Interaction.Aspects.Methods
{
    [ResourceAspectRegistration(nameof(ResourceMethodsAspectViewModel))]
    internal class ResourceMethodsAspectViewModel : ResourceAspectViewModelBase
    {
        public override string DisplayName => "Methods";

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
                _methodInvocationResult = value;
                NotifyOfPropertyChange();
            }
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

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
            var resultEntry = await ResourceServiceModel.InvokeMethod(Resource.Id, ((ResourceMethodViewModel) parameters).Model);

            if (resultEntry == null)
                MethodInvocationResult = new EntryViewModel(new Entry());
            else if (resultEntry.Value.Type >= EntryValueType.Class)
                MethodInvocationResult = new EntryViewModel(resultEntry);
            else
                MethodInvocationResult = new EntryViewModel(new List<Entry> { resultEntry });
        }
    }
}
