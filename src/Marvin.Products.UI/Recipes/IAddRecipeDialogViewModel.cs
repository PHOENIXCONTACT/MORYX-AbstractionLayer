using Caliburn.Micro;

namespace Marvin.Products.UI.Recipes
{
    public interface IAddRecipeDialogViewModel : IScreen
    {
        bool RecipeCreated { get; }
    }
}
