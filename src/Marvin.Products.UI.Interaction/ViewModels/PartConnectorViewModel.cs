using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Marvin.Products.UI.Interaction.InteractionSvc;

namespace Marvin.Products.UI.Interaction
{
    /// <summary>
    /// 
    /// </summary>
    public class PartConnectorViewModel
    {
        private readonly PartConnector _connector;

        internal PartConnectorViewModel(PartConnector connector)
        {
            _connector = connector;

            Parts = new ObservableCollection<PartViewModel>(_connector.Parts.Select(p => new PartViewModel(p)));
        }

        /// <summary>
        /// Name of the connector
        /// </summary>
        public string Name => _connector.Name;

        /// <summary>
        /// Displayname of the connector
        /// </summary>
        public string DisplayName => _connector.DisplayName;

        /// <summary>
        /// Flag if the connector represents a collection
        /// </summary>
        public bool IsCollection => _connector.IsCollection;

        /// <summary>
        /// Single part reference
        /// </summary>
        public PartViewModel Part
        {
            get { return Parts.FirstOrDefault(); }
            set
            {
                if (Parts.Count >= 1)
                    Parts[0] = value;
                else
                    Parts.Add(value);
            }
        }

        /// <summary>
        /// Converted part models
        /// </summary>
        public ObservableCollection<PartViewModel> Parts { get; }

        /// <summary>
        /// Create a new reference to the product
        /// </summary>
        public PartViewModel CreatePartReference(ProductViewModel productViewModel)
        {
            var product = productViewModel.Model;
            var partModel = new PartModel
            {
                Product = product,
                Properties = _connector.PropertyTemplates.Instantiate()
            };

            return new PartViewModel(partModel, productViewModel);
        }
    }
}