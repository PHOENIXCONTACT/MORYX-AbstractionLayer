// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System;
using System.Threading.Tasks;
using System.Windows.Input;
using C4I;
using Moryx.ClientFramework.Commands;
using Moryx.ClientFramework.Dialog;
using Moryx.ClientFramework.Tasks;
using Moryx.Resources.UI.Interaction.Properties;

namespace Moryx.Resources.UI.Interaction
{
    internal class RemoveResourceViewModel : DialogScreen
    {
        private readonly IResourceServiceModel _resourceServiceModel;
        private TaskNotifier _taskNotifier;
        private string _errorMessage;

        public ResourceViewModel ResourceToRemove { get; set; }

        public AsyncCommand RemoveCmd { get; set; }

        public ICommand CancelCmd { get; set; }

        public TaskNotifier TaskNotifier
        {
            get { return _taskNotifier; }
            set
            {
                _taskNotifier = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Error message while removing the resource
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

        public RemoveResourceViewModel(IResourceServiceModel resourceServiceModel, ResourceViewModel resource)
        {
            _resourceServiceModel = resourceServiceModel;
            ResourceToRemove = resource;

            RemoveCmd = new AsyncCommand(Remove, CanRemove, true);
            CancelCmd = new RelayCommand(Cancel, CanCancel);
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            DisplayName = Strings.RemoveResourceViewModel_DisplayName;
        }

        private bool CanRemove(object arg) =>
            _resourceServiceModel.IsAvailable;

        private async Task Remove(object arg)
        {
            try
            {
                var removeTask = _resourceServiceModel.RemoveResource(ResourceToRemove.Id);
                TaskNotifier = new TaskNotifier(removeTask);

                var result = await removeTask;
                if (result == false)
                {
                    ErrorMessage = Strings.RemoveResourceViewModel_ErrorWhileRemove;
                }
                else
                {
                    TryClose(true);
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
        }

        private bool CanCancel(object obj) =>
            !RemoveCmd.IsExecuting;

        private void Cancel(object obj) =>
            TryClose(false);
    }
}
