using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using C4I;
using Caliburn.Micro;
using Marvin.AbstractionLayer.UI;
using Marvin.Container;
using Marvin.Products.UI.ProductService;
using Marvin.Products.UI.Recipes;

namespace Marvin.Products.UI.Interaction
{
    internal enum RecipeEditorMode
    {
        Product,
        Recipe
    }

    [Plugin(LifeCycle.Transient, typeof(IRecipeWorkspace), Name = WorkspaceName)]
    internal class RecipesWorkspaceViewModel : MasterDetailsWorkspace<IRecipeDetails, IRecipeDetailsFactory, EmptyRecipeDetailsViewModel>, IRecipeWorkspace
    {
        internal const string WorkspaceName = nameof(RecipesWorkspaceViewModel);

        #region Dependency Injections

        public IProductServiceModel ProductServiceModel { get; set; }

        public IProductDialogFactory DialogFactory { get; set; }

        #endregion

        #region Fields and Properties

        private readonly ObservableCollection<RecipeViewModel> _recipes = new ObservableCollection<RecipeViewModel>();
        private readonly ObservableCollection<WorkplanViewModel> _workplans = new ObservableCollection<WorkplanViewModel>();
        private readonly long _preSelectedProductId;
        private readonly long[] _preSelectedRecipeIds = new long[0];

        public string Title { get; }

        public RecipeEditorMode EditorMode { get; }

        public ICommand AddRecipeCmd { get; }

        public ReadOnlyObservableCollection<RecipeViewModel> Recipes { get; }

        public ReadOnlyObservableCollection<WorkplanViewModel> Workplans { get; }

        private RecipeViewModel _selectedRecipe;
        public RecipeViewModel SelectedRecipe
        {
            get { return _selectedRecipe; }
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
            AddRecipeCmd = new RelayCommand(OnAddRecipe);
            Recipes = new ReadOnlyObservableCollection<RecipeViewModel>(_recipes);
            Workplans = new ReadOnlyObservableCollection<WorkplanViewModel>(_workplans);
        }

        public RecipesWorkspaceViewModel(string title, long[] recipeIds) : this()
        {
            _preSelectedRecipeIds = recipeIds;
            _preSelectedProductId = -1;
            Title = title;

            EditorMode = RecipeEditorMode.Recipe;
        }

        public RecipesWorkspaceViewModel(string title, long productId) : this()
        {
            _preSelectedRecipeIds = new long[0];
            _preSelectedProductId = productId;
            Title = title;

            EditorMode = RecipeEditorMode.Product;
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            Task.Run(LoadData);
        }

        public override  async Task OnMasterItemChanged(object sender, RoutedPropertyChangedEventArgs<object> args)
        {
            var selectedRecipe = (RecipeViewModel)args.NewValue;
            if (selectedRecipe == null)
            {
                ShowEmpty();
                return;
            }

            var detailsVm = DetailsFactory.Create(selectedRecipe.Type);
            await LoadDetails(() => detailsVm.Load(selectedRecipe.Id, _workplans));

            ActivateItem(detailsVm);
        }

        protected override void OnDetailsSaved(object sender, EventArgs e)
        {
            base.OnDetailsSaved(sender, e);

            var currentDetailsViewModel = ActiveItem as RecipeDetailsViewModelBase;
            if (currentDetailsViewModel == null)
                return;

            Task.Run(LoadRecipes);
        }

        protected override void ShowEmpty()
        {
            EmptyDetails.Display(MessageSeverity.Info, "Please select a recipe from the list");

            base.ShowEmpty();
        }

        private void OnAddRecipe(object obj)
        {
            if (EditorMode == RecipeEditorMode.Recipe || _preSelectedProductId == -1)
                return;

            var dialogViewModel = DialogFactory.CreateAddRecipeDialog(Title, _preSelectedProductId, Workplans);
            DialogManager.ShowDialog(dialogViewModel, model =>
            {
                if (!model.Result)
                    return;

                // Reload data
                Task.Run(LoadData);
            });
        }

        private async Task LoadData()
        {
            IsMasterBusy = true;

            try
            {
                await LoadWorkplans().ConfigureAwait(false);
                await LoadRecipes().ConfigureAwait(false);
            }
            finally
            {
                IsMasterBusy = false;
            }
        }

        private async Task LoadRecipes()
        {
            switch (EditorMode)
            {
                case RecipeEditorMode.Product:
                    await LoadRecipesByProduct();
                    break;
                case RecipeEditorMode.Recipe:
                    await LoadRecipesByIds();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
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
                DialogManager.ShowMessageBox($"Error while receiving workplans. {e}",
                    "Error");
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
                DialogManager.ShowMessageBox($"Error while receiving recipes. {e}",
                    "Error");
                throw;
            }

            await Execute.OnUIThreadAsync(() => AddRecipeToCollection(recipes.ToArray()));
        }

        private async Task LoadRecipesByProduct()
        {
            if (_preSelectedProductId == -1)
                return;

            RecipeModel[] recipes;
            try
            {
                recipes = await ProductServiceModel.GetProductionRecipes(_preSelectedProductId);
            }
            catch (Exception e)
            {
                DialogManager.ShowMessageBox($"Error while receiving recipes. {e}",
                    "Error");
                throw;
            }

            await Execute.OnUIThreadAsync(() => AddRecipeToCollection(recipes));
        }

        private void AddRecipeToCollection(RecipeModel[] recipeModels)
        {
            foreach (var recipeModel in recipeModels)
            {
                var workplan = _workplans.FirstOrDefault(w => w.Id == recipeModel.WorkplanId);
                var existent = _recipes.FirstOrDefault(w => w.Id == recipeModel.Id);
                if (existent != null)
                {
                    existent.UpdateModel(recipeModel, workplan);
                }
                else
                {
                    _recipes.Add(new RecipeViewModel(recipeModel, workplan));
                }
            }

            //TODO: Remove removed recipes
        }

        private void AddWorkplanToCollection(WorkplanModel[] workplanModels)
        {
            foreach (var workplanModel in workplanModels)
            {
                var existent = _workplans.FirstOrDefault(w => w.Id == workplanModel.Id);
                if (existent != null)
                    existent.UpdateModel(workplanModel);
                else
                    _workplans.Add(new WorkplanViewModel(workplanModel));
            }

            //TODO: Remove removed workplans
        }
    }
}