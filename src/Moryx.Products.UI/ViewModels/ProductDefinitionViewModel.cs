using System.Collections.Generic;
using Caliburn.Micro;
using Moryx.Controls;
using Moryx.Products.UI.ProductService;
using Entry = Moryx.Serialization.Entry;

namespace Moryx.Products.UI
{
    public class ProductDefinitionViewModel : PropertyChangedBase
    {
        private EntryViewModel _properties;

        /// <summary>
        /// Underlying model
        /// </summary>
        public ProductDefinitionModel Model { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductDefinitionViewModel"/> class.
        /// </summary>
        public ProductDefinitionViewModel(ProductDefinitionModel model)
        {
            Model = model;
            Properties = Model.Properties != null
                ? new EntryViewModel(Model.Properties.ToSerializationEntry())
                : new EntryViewModel(new List<Entry>());
        }

        /// <summary>
        /// Gets the display name of the product definition.
        /// </summary>
        public string DisplayName => Model.DisplayName;

        /// <summary>
        /// Entry view model of the product properties
        /// </summary>
        public EntryViewModel Properties
        {
            get => _properties;
            private set
            {
                _properties = value;
                NotifyOfPropertyChange();
            }
        }
    }
}
