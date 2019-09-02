using Marvin.AbstractionLayer.UI;
using Marvin.Container;

namespace Marvin.Products.UI.Interaction
{
    [PluginFactory(typeof(RecipeDetailsComponentSelector))]
    internal interface IRecipeDetailsFactory : IDetailsFactory<IRecipeDetails>
    {
    }
}