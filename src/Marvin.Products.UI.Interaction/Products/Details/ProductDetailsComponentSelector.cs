using Marvin.AbstractionLayer.UI;
using Marvin.Container;

namespace Marvin.Products.UI.Interaction
{
    /// <summary>
    /// Component selector for resource view models
    /// </summary>
    [Plugin(LifeCycle.Singleton)]
    internal class ProductDetailsComponentSelector : DetailsComponentSelector<IProductDetails, IProductServiceModel>
    {
        public ProductDetailsComponentSelector(IContainer container, IProductServiceModel controller) : base(container, controller)
        {
        }
    }
}