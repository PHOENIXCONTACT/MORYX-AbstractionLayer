using Marvin.AbstractionLayer.UI;
using Marvin.Container;
using Marvin.Products.UI.Recipes;

namespace Marvin.Products.UI.Interaction
{
    [PluginFactory(typeof(RecipeDetailsComponentSelector))]
    internal interface IRecipeDetailsFactory : IDetailsFactory<IRecipeDetails>
    {
    }
}