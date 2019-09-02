using Marvin.Products.UI.ProductService;

namespace Marvin.Products.UI
{
    /// <summary>
    /// Simplified representation of a Product.
    /// This is used for showing basic information about a product.
    /// </summary>
    public class ProductInfoViewModel
    {
        /// <summary>
        /// Underlying model
        /// </summary>
        public ProductModel Model { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductInfoViewModel"/> class.
        /// </summary>
        public ProductInfoViewModel(ProductModel productModel)
        {
            Model = productModel;
        }

        /// <summary>
        /// Id of the Product
        /// </summary>
        public long Id => Model.Id;

        /// <summary>
        /// Identifier of the product
        /// </summary>
        public string Identifier => Model.Identifier;

        /// <summary>
        /// Revision of the product
        /// </summary>
        public short Revision => Model.Revision;

        /// <summary>
        /// Name of the product
        /// </summary>
        public string Name => Model.Name;

        /// <summary>
        /// Combination of identifier and revision
        /// </summary>
        public string FullIdentifier => $"{Model.Identifier}-{Model.Revision:D2}";

        /// <summary>
        /// Combination of FullIdentifier and name
        /// </summary>
        public string DisplayName => $"{Model.Identifier}-{Model.Revision:D2} {Model.Name}";

        /// <summary>
        /// Type of the Product
        /// </summary>
        public string Type => Model.Type;
    }
}