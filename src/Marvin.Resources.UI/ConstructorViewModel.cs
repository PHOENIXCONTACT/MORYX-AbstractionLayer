using Caliburn.Micro;
using Marvin.Controls;
using Marvin.Serialization;

namespace Marvin.Resources.UI
{
    /// <summary>
    /// View model that represents a constructor
    /// </summary>
    public class ConstructorViewModel : PropertyChangedBase
    {
        private bool _isSelected;

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
        public EntryViewModel Parameters { get; private set; }

        /// <summary>
        /// Flag if this constructor was selected
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
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
            UpdateModel(model);
        }

        /// <summary>
        /// Updates the internal model
        /// </summary>
        /// <param name="model"></param>
        public void UpdateModel(MethodEntry model)
        {
            Model = model;
            NotifyOfPropertyChange(nameof(DisplayName));

            Parameters = new EntryViewModel(model.Parameters);
            NotifyOfPropertyChange(nameof(Parameters));
        }
    }
}