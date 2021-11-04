// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using Moryx.AbstractionLayer.UI;
using Moryx.Container;
using Moryx.Products.UI.Interaction.Properties;
using Moryx.Products.UI.ProductService;

namespace Moryx.Products.UI.Interaction
{
    [Plugin(LifeCycle.Transient, typeof(IRecipeWorkspace), Name = WorkspaceName)]
    internal class RecipesWorkspaceViewModel : MasterDetailsWorkspace<IRecipeDetails, IRecipeDetailsFactory, EmptyRecipeDetailsViewModel>, IRecipeWorkspace
    {
        internal const string WorkspaceName = nameof(RecipesWorkspaceViewModel);

        #region Dependency Injections

        public IProductServiceModel ProductServiceModel { get; set; }

        #endregion

        #region Fields and Properties

        private readonly ObservableCollection<RecipeViewModel> _recipes = new ObservableCollection<RecipeViewModel>();
        private readonly ObservableCollection<WorkplanViewModel> _workplans = new ObservableCollection<WorkplanViewModel>();
        private readonly long[] _preSelectedRecipeIds = new long[0];

        public string Title { get; }

        public ReadOnlyObservableCollection<RecipeViewModel> Recipes { get; }

        private RecipeViewModel _selectedRecipe;
        public RecipeViewModel SelectedRecipe
        {
            get => _selectedRecipe;
            set
            {
                if (_selectedRecipe == value)
                    return;

                var oldSelectedRecipe = _selectedRecipe;
                _selectedRecipe = value;
                NotifyOfPropertyChange(nameof(SelectedRecipe));

                Task.Run(() => OnMasterItemChanged(null, new RoutedPropertyChangedEventArgs<object>(oldSelectedRecipe, _selectedRecipe)));
            }
        }

        #endregion

        public RecipesWorkspaceViewModel()
        {
            Recipes = new ReadOnlyObservableCollection<RecipeViewModel>(_recipes);
        }

        public RecipesWorkspaceViewModel(string title, long[] recipeIds) : this()
        {
            _preSelectedRecipeIds = recipeIds;
            Title = title;
        }

        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            await base.OnActivateAsync(cancellationToken);
#pragma warning disable 4014
            Task.Run(LoadData);
#pragma warning restore 4014
        }

        public override async Task OnMasterItemChanged(object sender, RoutedPropertyChangedEventArgs<object> args)
        {
            var selectedRecipe = (RecipeViewModel)args.NewValue;
            if (selectedRecipe == null)
            {
                await ShowEmpty();
                return;
            }

            var detailsVm = DetailsFactory.Create(selectedRecipe.Type);
            await LoadDetails(() => detailsVm.Load(selectedRecipe.Id, _workplans));

            await ActivateItemAsync(detailsVm);
        }

        protected override async Task OnSaved()
        {
            await base.OnSaved();

            var currentDetailsViewModel = ActiveItem as RecipeDetailsViewModelBase;
            if (currentDetailsViewModel == null)
                return;

            await LoadRecipesByIds();
        }

        protected override Task ShowEmpty()
        {
            EmptyDetails.Display(MessageSeverity.Info, Strings.RecipeWorkspaceViewModel_SelectRecipe);

            return base.ShowEmpty();
        }

        private async Task LoadData()
        {
            IsBusy = true;

            try
            {
                await LoadWorkplans().ConfigureAwait(false);
                await LoadRecipesByIds().ConfigureAwait(false);
                await Execute.OnUIThreadAsync(delegate
                {
                    SelectedRecipe = Recipes.FirstOrDefault();
                    return Task.CompletedTask;
                });
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task LoadWorkplans()
        {
            try
            {
                var workplans = await ProductServiceModel.GetWorkplans();
                await Execute.OnUIThreadAsync(() => AddWorkplanToCollection(workplans));
            }
            catch (Exception e)
            {
                DialogManager.ShowMessageBox($"{Strings.RecipeWorkspaceViewModel_ErrorLoadingWorkplans} {e}",
                    Strings.RecipeWorkspaceViewModel_ErrorLoadingWorkplans);
                throw;
            }
        }

        private async Task LoadRecipesByIds()
        {
            var recipes = new List<RecipeModel>();
            try
            {
                foreach (var recipeId in _preSelectedRecipeIds)
                {
                    var recipe = await ProductServiceModel.GetRecipe(recipeId);
                    recipes.Add(recipe);
                }
            }
            catch (Exception e)
            {
                DialogManager.ShowMessageBox($"{Strings.RecipeWorkspaceViewModel_ErrorLoadingRecipes_Message} {e}",
                    Strings.RecipeWorkspaceViewModel_ErrorLoadingRecipes_Title);
                throw;
            }

            await Execute.OnUIThreadAsync(() => AddRecipeToCollection(recipes.ToArray()));
        }

        private Task AddRecipeToCollection(RecipeModel[] recipeModels)
        {
            foreach (var recipeModel in recipeModels)
            {
                var workplan = _workplans.FirstOrDefault(w => w.Id == recipeModel.WorkplanId);
                var existent = _recipes.FirstOrDefault(w => w.Id == recipeModel.Id);
                if (existent != null)
                {
                    existent.UpdateModel(recipeModel);
                    existent.Workplan = workplan;
                }
                else
                {
                    _recipes.Add(new RecipeViewModel(recipeModel)
                    {
                        Workplan = workplan
                    });
                }
            }

            return Task.CompletedTask;
        }

        private Task AddWorkplanToCollection(WorkplanModel[] workplanModels)
        {
            foreach (var workplanModel in workplanModels)
            {
                var existent = _workplans.FirstOrDefault(w => w.Id == workplanModel.Id);
                if (existent != null)
                    existent.UpdateModel(workplanModel);
                else
                    _workplans.Add(new WorkplanViewModel(workplanModel));
            }

            return Task.CompletedTask;
        }
    }
}
