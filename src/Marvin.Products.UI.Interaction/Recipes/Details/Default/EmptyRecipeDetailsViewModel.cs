using System.Collections.Generic;
using System.Threading.Tasks;
using Marvin.AbstractionLayer.UI;
using Marvin.Products.UI.Recipes;

namespace Marvin.Products.UI.Interaction
{
    /// <summary>
    /// ViewModel for non selected recipe details
    /// </summary>
    [RecipeDetailsRegistration(DetailsConstants.EmptyType)]
    public class EmptyRecipeDetailsViewModel : EmptyDetailsViewModelBase, IRecipeDetails
    {
        /// <summary>
        /// Load dummy
        /// </summary>
        public Task Load(long productId, IReadOnlyCollection<WorkplanViewModel> workplans)
        {
            return SuccessTask;
        }
    }
}
