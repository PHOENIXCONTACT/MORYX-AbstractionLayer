using Marvin.Container;

namespace Marvin.Products.UI.Recipes
{
    [PluginFactory]
    public interface IRecipeDialogFactory
    {
        IAddRecipeDialogViewModel CreateAddRecipeDialogViewModel(NonDetailedProductModel productModel);
    }
}
