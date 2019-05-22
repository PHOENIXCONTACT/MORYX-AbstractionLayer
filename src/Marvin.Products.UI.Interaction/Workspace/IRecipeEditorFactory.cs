using Marvin.Container;
using Marvin.Products.UI.Recipes;

namespace Marvin.Products.UI.Interaction
{
    [PluginFactory]
    public interface IRecipeEditorFactory
    {
        IRecipeWorkspace CreateRecipeEditor(string title, long[] recipeIds);

        IRecipeWorkspace CreateRecipeEditor(string title, long productId);
    }
}
