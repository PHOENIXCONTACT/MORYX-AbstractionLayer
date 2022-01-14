// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Caliburn.Micro;
using Moryx.Controls;
using Moryx.Resources.UI.ResourceService;

namespace Moryx.Resources.UI
{
    /// <summary>
    /// View model that represents a constructor
    /// </summary>
    public class ConstructorViewModel : PropertyChangedBase
    {
        private bool _isSelected;
        private EntryViewModel _parameters;

        /// <summary>
        /// Underlying model for the constructor
        /// </summary>
        public MethodEntry Model { get; private set; }

        /// <summary>
        /// Display name of this constructor
        /// </summary>
        public string DisplayName => Model.DisplayName;

        /// <summary>
        /// View model of the parameters for the config editor
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

        /// <summary>
        /// Flag if this constructor was selected
        /// </summary>
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Create new constructor view model from constructor method view model
        /// </summary>
        public ConstructorViewModel(MethodEntry model)
        {
            CopyFromModel(model);
        }

        /// <summary>
        /// Updates the internal model
        /// </summary>
        /// <param name="model"></param>
        public void CopyFromModel(MethodEntry model)
        {
            Model = model;
            NotifyOfPropertyChange(nameof(DisplayName));

            Parameters = new EntryViewModel(Model.Parameters.ToSerializationEntry());
            NotifyOfPropertyChange(nameof(Parameters));
        }
    }
}
