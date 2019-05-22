using Marvin.Container;

namespace Marvin.Products.UI.Recipes
{
    /// <summary>
    /// Factory that creates an instance of <see cref="IRecipeWorkspace"/> instance
    /// </summary>
    [PluginFactory]
    public interface IRecipeEditorFactory
    {
        /// <summary>
        /// Returns an instance of <see cref="IRecipeWorkspace"/>
        /// </summary>
        /// <returns></returns>
        IRecipeWorkspace CreateRecipeEditor();
    }
}
