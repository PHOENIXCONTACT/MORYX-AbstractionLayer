using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Marvin.AbstractionLayer.UI;
using Marvin.Products.UI.ProductService;
using Marvin.Products.UI.Recipes;

namespace Marvin.Products.UI.Interaction
{
    /// <summary>
    /// Standard ViewModel for recipes
    /// </summary>
    public abstract class RecipeDetailsViewModelBase : EditModeViewModelBase<RecipeViewModel>, IRecipeDetails
    {
        #region Dependency Injections

        /// <summary>
        /// Service model to load additional information
        /// </summary>
        public IProductServiceModel ProductServiceModel { get; private set; }

        #endregion

        /// <summary>
        /// Available recipe classifications
        /// </summary>
        public IEnumerable<string> AvailableClassifications => Enum.GetNames(typeof(RecipeClassificationModel));

        /// <summary>
        /// All workplans available
        /// </summary>
        public IReadOnlyCollection<WorkplanViewModel> Workplans { get; private set; }

        /// <inheritdoc />
        public void Initialize(IInteractionController controller, string typeName)
        {
            ProductServiceModel = (IProductServiceModel) controller;
        }

        /// <inheritdoc />
        public async Task Load(long recipeId, IReadOnlyCollection<WorkplanViewModel> workplans)
        {
            var recipeModel = await ProductServiceModel.GetRecipe(recipeId);
            var wp = workplans.FirstOrDefault(w => w.Id == recipeModel.WorkplanId);

            Workplans = workplans;
            NotifyOfPropertyChange(nameof(Workplans));

            EditableObject = new RecipeViewModel(recipeModel, wp);
        }

        /// <inheritdoc />
        protected override async Task OnSave(object parameters)
        {
            try
            {
                await ProductServiceModel.SaveProductionRecipe(EditableObject.Model);
            }
            catch (Exception e)
            {
                DialogManager.ShowMessageBox($"Cannot save recipe: {e}", "Error");
            }

            await base.OnSave(parameters);
        }
    }
}