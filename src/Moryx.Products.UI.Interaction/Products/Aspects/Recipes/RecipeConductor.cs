// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace Moryx.Products.UI.Interaction.Aspects
{
    internal class RecipeConductor : Conductor<IScreen>.Collection.OneActive, IEditableObject
    {
        private readonly IRecipeDetailsFactory _detailsFactory;
        private readonly IReadOnlyCollection<WorkplanViewModel> _workplans;
        private readonly ObservableCollection<RecipeViewModel> _recipes;

        private bool _isEditMode;
        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                _isEditMode = value;
                NotifyOfPropertyChange();
            }
        }

        public RecipeConductor(IRecipeDetailsFactory detailsFactory, IReadOnlyCollection<WorkplanViewModel> workplans, ObservableCollection<RecipeViewModel> recipes)
        {
            _detailsFactory = detailsFactory;
            _workplans = workplans;
            _recipes = recipes;

            recipes.CollectionChanged += OnRecipeCollectionChanged;
        }

        public Task Load() =>
            AddRecipes(_recipes);

        private async void OnRecipeCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    await AddRecipes(e.NewItems.Cast<RecipeViewModel>().ToArray());
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveRecipes(e.OldItems.Cast<RecipeViewModel>().ToArray());
                    break;
                case NotifyCollectionChangedAction.Replace:
                    await AddRecipes(e.NewItems.Cast<RecipeViewModel>().ToArray());
                    RemoveRecipes(e.OldItems.Cast<RecipeViewModel>().ToArray());
                    break;
                case NotifyCollectionChangedAction.Reset:
                    RemoveRecipes(Items.Cast<IRecipeDetails>().Select(i => i.EditableObject).ToArray());
                    break;
            }
        }

        private void RemoveRecipes(ICollection<RecipeViewModel> recipes)
        {
            foreach (RecipeViewModel oldRecipe in recipes)
            {
                var details = Items.Cast<IRecipeDetails>().First(d => d.EditableObject == oldRecipe);
                Items.Remove(details);
                _detailsFactory.Destroy(details);
            }
        }

        private async Task AddRecipes(ICollection<RecipeViewModel> recipes)
        {
            var loaderTasks = new List<Task>(recipes.Count);
            loaderTasks.AddRange(recipes.Select(AddRecipe));

            await Task.WhenAll(loaderTasks);
        }

        private async Task AddRecipe(RecipeViewModel recipeViewModel)
        {
            // Load data
            var detailsVm = _detailsFactory.Create(recipeViewModel.Type);
            await detailsVm.Load(recipeViewModel, _workplans);

            // If we are currently in edit mode, we enter it
            if (IsEditMode)
                detailsVm.BeginEdit();

            Items.Add(detailsVm);
        }

        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            await base.OnActivateAsync(cancellationToken);
            await ActivateItemAsync(Items.FirstOrDefault(), cancellationToken);
        }

        public void BeginEdit()
        {
            IsEditMode = true;
            foreach (var details in Items.OfType<IRecipeDetails>())
                details.BeginEdit();
        }

        public void EndEdit()
        {
            foreach (var details in Items.OfType<IRecipeDetails>())
                details.EndEdit();

            IsEditMode = false;
        }

        public void CancelEdit()
        {
            foreach (var details in Items.OfType<IRecipeDetails>())
                details.CancelEdit();

            IsEditMode = false;
        }

        public async Task Save()
        {
            foreach (var details in Items.OfType<IRecipeDetails>())
                await details.Save();
        }
    }
}
