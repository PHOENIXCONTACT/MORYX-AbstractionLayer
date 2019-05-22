using Marvin.AbstractionLayer.UI;
using Marvin.Container;

namespace Marvin.Products.UI.Recipes
{
    [PluginFactory(typeof(IRecipeDetailsComponentSelector))]
    public interface IRecipeDetailsFactory : IDetailsFactory<IRecipeDetails>
    {
    }
}