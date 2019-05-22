using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Marvin.AbstractionLayer.UI;
using Marvin.Products.UI.ProductService;
using Marvin.Serialization;

namespace Marvin.Products.UI.Interaction
{
    /// <summary>
    /// Base class for the product details view model
    /// </summary>
    public abstract class ProductDetailsViewModelBase : EditModeViewModelBase<ProductViewModel>, IProductDetails
    {
        /// <summary>
        /// Service model to load additional information from the server
        /// </summary>
        public IProductServiceModel ProductServiceModel { get; private set; }

        /// <summary>
        /// Model visualized by this view model
        /// </summary>
        private ProductModel _model;

        #region Properties

        /// <summary>
        /// Represents the product properties
        /// </summary>
        public Entry ProductProperties
        {
            get { return _model.Properties; }
            protected set { _model.Properties = value ; }
        }

        /// <summary>
        /// All parts of this product
        /// </summary>
        public List<PartConnectorViewModel> Parts { get; set; }

        ///
        public long ProductId => EditableObject?.Id ?? 0;

        #endregion

        /// <inheritdoc />
        public void Initialize(IInteractionController controller, string typeName)
        {
            base.Initialize();

            ProductServiceModel = (IProductServiceModel)controller;
        }

        ///
        public async Task Load(long productId)
        {
            _model = await ProductServiceModel.GetDetails(productId);

            EditableObject = new ProductViewModel(_model);
            Parts = _model.Parts.Select(p => new PartConnectorViewModel(p)).ToList();
        }

        ///
        protected override async Task OnSave(object parameters)
        {
            var product = await ProductServiceModel.Save(_model);
            EditableObject = new ProductViewModel(product);
            NotifyOfPropertyChange(() => EditableObject);

            await base.OnSave(parameters);
        }
    }
}
