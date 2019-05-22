using System.Collections.Generic;
using System.Threading.Tasks;
using Marvin.AbstractionLayer.UI;

namespace Marvin.Products.UI.Recipes
{
    /// <summary>
    /// Recipe details interface
    /// </summary>
    public interface IRecipeDetails : IEditModeViewModel, IDetailsViewModel
    {
        /// <summary>
        /// Method to load the recipe details
        /// </summary>
        Task Load(long recipeId, IReadOnlyCollection<WorkplanViewModel> workplans);
    }
}
