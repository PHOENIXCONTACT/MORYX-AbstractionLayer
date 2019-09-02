using Marvin.AbstractionLayer.UI;
using Marvin.Container;

namespace Marvin.Products.UI.Interaction
{
    /// <summary>
    /// Component selector for resource view models
    /// </summary>
    [Plugin(LifeCycle.Singleton)]
    internal class RecipeDetailsComponentSelector : DetailsComponentSelector<IRecipeDetails>
    {
        public RecipeDetailsComponentSelector(IContainer container) : base(container)
        {
        }
    }
}