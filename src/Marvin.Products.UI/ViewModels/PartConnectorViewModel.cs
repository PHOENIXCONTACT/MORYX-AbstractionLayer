using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Caliburn.Micro;
using Marvin.Products.UI.ProductService;
using Marvin.Tools;

namespace Marvin.Products.UI
{
    /// <summary>
    /// View model for part connectors
    /// </summary>
    public class PartConnectorViewModel : PropertyChangedBase, IEditableObject
    {
        /// <summary>
        /// Underlying DTO
        /// </summary>
        public PartConnector Model { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PartConnectorViewModel"/> class.
        /// </summary>
        public PartConnectorViewModel(PartConnector model)
        {
            Model = model;
            CopyFromModel();
        }

        /// <summary>
        /// DisplayName of the connector
        /// </summary>
        public string Name => Model.DisplayName ?? Model.Name;

        /// <summary>
        /// Flag if the connector represents a collection
        /// </summary>
        public bool IsCollection => Model.IsCollection;

        /// <summary>
        /// Underlying type
        /// </summary>
        public string Type => Model.Type;

        /// <summary>
        /// List of part links. Hint: Single entry or null if IsCollection is false
        /// </summary>
        public ObservableCollection<PartLinkViewModel> PartLinks { get; } = new ObservableCollection<PartLinkViewModel>();

        /// <summary>
        /// Copies the DTO to the view model
        /// </summary>
        public void CopyFromModel()
        {
            PartLinks.Clear();
            PartLinks.AddRange(Model.Parts.Select(p => new PartLinkViewModel(p)));
        }

        /// <summary>
        /// Copies the view model to the DTO
        /// </summary>
        public void CopyToModel()
        {
            PartLinks.ForEach(pl => pl.CopyToModel());
            Model.Parts = PartLinks.Select(p => p.Model).ToArray();
        }

        /// <inheritdoc />
        public void BeginEdit()
        {
        }

        /// <inheritdoc />
        public void EndEdit()
        {
            CopyToModel();
        }

        /// <inheritdoc />
        public void CancelEdit()
        {
            CopyFromModel();
        }

        /// <summary>
        /// Create a new reference to the product
        /// </summary>
        public PartLinkViewModel CreatePartLink(ProductInfoViewModel product)
        {
            var productModel = product.Model;
            var partModel = new PartModel
            {
                Product = productModel,
                Properties = Model.PropertyTemplates.Clone(true)
            };

            return new PartLinkViewModel(partModel, product);
        }
    }
}