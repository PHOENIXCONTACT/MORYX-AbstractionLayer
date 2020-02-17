using Marvin.AbstractionLayer.UI;

// ReSharper disable once CheckNamespace
namespace Marvin.Products.UI.ProductService
{
    public partial class PartConnector : IIdentifiableObject
    {
        public long Id => Name.GetHashCode();
    }
}