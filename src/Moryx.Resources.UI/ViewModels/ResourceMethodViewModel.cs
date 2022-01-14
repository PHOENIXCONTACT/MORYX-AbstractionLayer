// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Caliburn.Micro;
using Moryx.Controls;
using Moryx.Resources.UI.ResourceService;
using System.ComponentModel;

namespace Moryx.Resources.UI
{
    /// <summary>
    /// View model that represents an editorVisible of a resource
    /// </summary>
    public class ResourceMethodViewModel : PropertyChangedBase, IEditableObject
    {
        private EntryViewModel _parameters;

        /// <summary>
        /// Underlying model
        /// </summary>
        public MethodEntry Model { get; private set; }

        /// <summary>
        /// Creates a ViewModel from the given Model
        /// </summary>
        public ResourceMethodViewModel(MethodEntry model)
        {
            CopyFromModel(model);
        }

        private void CopyFromModel(MethodEntry model)
        {
            Model = model;
            NotifyOfPropertyChange(nameof(DisplayName));
            NotifyOfPropertyChange(nameof(Description));

            Parameters = new EntryViewModel(Model.Parameters.ToSerializationEntry());
            NotifyOfPropertyChange(nameof(Parameters));
        }

        public void CopyToModel()
        {
            Model.Parameters = Parameters.Entry.ToServiceEntry();
        }

        /// <summary>
        /// Name of the method
        /// </summary>
        public string DisplayName => Model.DisplayName;

        /// <summary>
        /// Method description
        /// </summary>
        public string Description => Model.Description;

        /// <summary>
        /// Parameters which are necessary for the method call
        /// </summary>
        public EntryViewModel Parameters
        {
            get { return _parameters; }
            private set
            {
                if (_parameters is null)
                    _parameters = value;
                else
                    _parameters.UpdateModel(value.Entry);
            }
        }

        public void BeginEdit()
        {
            Parameters.BeginEdit();
        }

        public void EndEdit()
        {
            Parameters.EndEdit();
            CopyToModel();
        }

        public void CancelEdit()
        {
            Parameters.CancelEdit();
            CopyFromModel(Model);
        }
    }
}
