using Marvin.AbstractionLayer.UI;
using Marvin.Container;
using Marvin.Products.UI.Recipes;

namespace Marvin.Products.UI.Interaction
{
    /// <summary>
    /// Component selector for resource view models
    /// </summary>
    [Plugin(LifeCycle.Singleton)]
    internal class RecipeDetailsComponentSelector : DetailsComponentSelector<IRecipeDetails, IProductServiceModel>
    {
        public RecipeDetailsComponentSelector(IContainer container, IProductServiceModel controller) : base(container, controller)
        {
        }
    }
}