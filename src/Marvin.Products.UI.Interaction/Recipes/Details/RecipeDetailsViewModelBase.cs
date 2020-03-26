// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Marvin.AbstractionLayer.UI;
using Marvin.ClientFramework.Dialog;
using Marvin.Products.UI.Interaction.Properties;
using Marvin.Products.UI.ProductService;

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
        public IProductServiceModel ProductServiceModel { get; set; }

        /// <summary>
        /// Dialog manager do show dialogs and message boxes
        /// </summary>
        public IDialogManager DialogManager { get; set; }

        #endregion

        /// <summary>
        /// Field to define if the editable object (recipe) is loaded and managed by the details or by another party
        /// In this case, the recipe is self managed if it will be used in <see cref="MasterDetailsWorkspace{TDetailsType,TDetailsFactory,TEmptyDetails}"/>
        /// When it will be used within product aspects.
        /// </summary>
        private bool _isSelfManagedRecipe;

        /// <summary>
        /// All workplans available
        /// </summary>
        public IReadOnlyCollection<WorkplanViewModel> Workplans { get; private set; }

        /// <summary>
        /// Type of this recipe
        /// </summary>
        public RecipeTypeViewModel Type { get; private set; }

        /// <inheritdoc />
        public void Initialize(string typeName)
        {
        }

        /// <inheritdoc />
        public async Task Load(long recipeId, IReadOnlyCollection<WorkplanViewModel> workplans)
        {
            _isSelfManagedRecipe = true;

            var recipeModel = await ProductServiceModel.GetRecipe(recipeId);
            await LoadTypeAndWorkplans(recipeModel, workplans);

            WorkplanViewModel wp = null;
            if (recipeModel.WorkplanId != 0)
                wp = workplans.FirstOrDefault(w => w.Id == recipeModel.WorkplanId);

            EditableObject = new RecipeViewModel(recipeModel)
            {
                Workplan = wp
            };
        }

        /// <inheritdoc />
        public override string DisplayName => EditableObject.Name;

        /// <inheritdoc />
        public async Task Load(RecipeViewModel recipeVm, IReadOnlyCollection<WorkplanViewModel> workplans)
        {
            _isSelfManagedRecipe = false;
            await LoadTypeAndWorkplans(recipeVm.Model, workplans);

            WorkplanViewModel wp = null;
            if (recipeVm.Model.WorkplanId != 0)
                wp = workplans.FirstOrDefault(w => w.Id == recipeVm.Model.WorkplanId);
            recipeVm.Workplan = wp;

            EditableObject = recipeVm;
        }

        private async Task LoadTypeAndWorkplans(RecipeModel model, IReadOnlyCollection<WorkplanViewModel> workplans)
        {
            Workplans = workplans;
            NotifyOfPropertyChange(nameof(Workplans));

            var customization = await ProductServiceModel.GetCustomization();
            var typeModel = customization.RecipeTypes.FirstOrDefault(t => t.Name == model.Type);
            if (typeModel == null)
                throw new InvalidOperationException("Recipe type not found");

            Type = new RecipeTypeViewModel(typeModel);
        }

        /// <inheritdoc />
        public override async Task Save()
        {
            try
            {
                if (_isSelfManagedRecipe)
                    await ProductServiceModel.SaveRecipe(EditableObject.Model);
            }
            catch (Exception e)
            {
                DialogManager.ShowMessageBox($"{Strings.RecipeDetailsViewModelBase_ErrorSavingRecipe_Message} {e}",
                    Strings.RecipeDetailsViewModelBase_ErrorSavingRecipe_Title);
            }
        }
    }
}
