using Caliburn.Micro;
using Marvin.Products.UI.ProductService;

namespace Marvin.Products.UI.Interaction
{
    internal interface IImportViewModel : IScreen
    {
        ProductModel ImportedProduct { get; }
    }
}