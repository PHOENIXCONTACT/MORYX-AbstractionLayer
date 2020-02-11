using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Caliburn.Micro;
using Marvin.AbstractionLayer.UI;
using Marvin.Products.UI.ProductService;
using Marvin.Tools;

namespace Marvin.Products.UI
{
    /// <summary>
    /// View model for part connectors
    /// </summary>
    public class PartConnectorViewModel : PropertyChangedBase, IEditableObject, IIdentifiableObject
    {

        private readonly PartLinkMergeStrategy _partLinkMergeStrategy;

        /// <summary>
        /// Underlying DTO
        /// </summary>
        public PartConnector Model { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PartConnectorViewModel"/> class.
        /// </summary>
        public PartConnectorViewModel(PartConnector model)
        {
            Model = model;
            _partLinkMergeStrategy = new PartLinkMergeStrategy();

            CopyFromModel();
        }

        /// <summary>
        /// Identifier of the PartConnector
        /// </summary>
        public long Id => Model.Id;

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
        /// Updates the model
        /// </summary>
        public void UpdateModel(PartConnector model)
        {
            Model = model;
            CopyFromModel();
        }

        /// <summary>
        /// Copies the DTO to the view model
        /// </summary>
        public void CopyFromModel()
        {
            PartLinks.MergeCollection(Model.Parts, _partLinkMergeStrategy);
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

        private class PartLinkMergeStrategy : IMergeStrategy<PartModel, PartLinkViewModel>
        {
            public PartLinkViewModel FromModel(PartModel model) => new PartLinkViewModel(model);

            public void UpdateModel(PartLinkViewModel viewModel, PartModel model) => viewModel.UpdateModel(model);
        }
    }
}