using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using C4I;
using Marvin.ClientFramework.Commands;
using Marvin.ClientFramework.Dialog;
using Marvin.Products.UI.Interaction.Properties;

namespace Marvin.Products.UI.Interaction.Aspects
{
    [ProductAspectRegistration(nameof(RecipesAspectViewModel))]
    internal class RecipesAspectViewModel : ProductAspectViewModelBase
    {
        #region Dependency Injection

        public IRecipeDetailsFactory DetailsFactory { get; set; }

        public IProductServiceModel ProductServiceModel { get; set; }

        public IDialogManager DialogManager { get; set; }

        public RecipeConductor RecipeConductor { get; private set; }

        #endregion

        #region Fields and Properties

        private readonly List<WorkplanViewModel> _workplans = new List<WorkplanViewModel>();

        public override string DisplayName => Strings.RecipesAspectViewModel_DisplayName;

        public ICommand AddRecipeCmd { get; }

        public ICommand RemoveRecipeCmd { get; }

        #endregion

        public RecipesAspectViewModel()
        {
            AddRecipeCmd = new AsyncCommand(AddRecipe, CanAddRecipe, true);
            RemoveRecipeCmd = new RelayCommand(RemoveRecipe, CanRemoveRecipe);
        }

        public override async Task Load(ProductViewModel product)
        {
            await base.Load(product);

            try
            {
                var workplans = await ProductServiceModel.GetWorkplans();
                _workplans.AddRange(workplans.Select(wm => new WorkplanViewModel(wm)));

                RecipeConductor = new RecipeConductor(DetailsFactory, _workplans, Product.Recipes);
                await RecipeConductor.Load();
            }
            catch (Exception e)
            {
                DialogManager.ShowMessageBox($"Error while receiving workplans. {e}",
                    "Error while receiving workplans.");
            }
        }

        public override void BeginEdit()
        {
            base.BeginEdit();
            RecipeConductor.BeginEdit();
        }

        public override void EndEdit()
        {
            RecipeConductor.EndEdit();
            base.EndEdit();
        }

        public override void CancelEdit()
        {
            RecipeConductor.CancelEdit();
            base.CancelEdit();
        }

        public override Task Save() =>
            RecipeConductor.Save();

        private bool CanAddRecipe(object obj) =>
            IsEditMode;

        private async Task AddRecipe(object obj)
        {
            var dialog = new AddRecipeDialogViewModel(ProductServiceModel, _workplans);
            await DialogManager.ShowDialogAsync(dialog);
            if (!dialog.Result)
                return;

            Product.Recipes.Add(dialog.RecipePrototype);
            var details = RecipeConductor.Items.Cast<IRecipeDetails>()
                .FirstOrDefault(d => d.EditableObject == dialog.RecipePrototype);

            if (details != null)
                RecipeConductor.ActivateItem(details);
        }

        private bool CanRemoveRecipe(object obj) =>
            IsEditMode && RecipeConductor.ActiveItem != null;

        private void RemoveRecipe(object obj) =>
            Product.Recipes.Remove(((IRecipeDetails) RecipeConductor.ActiveItem).EditableObject);
    }
}