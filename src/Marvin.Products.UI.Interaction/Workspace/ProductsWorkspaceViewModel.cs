using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using C4I;
using Marvin.AbstractionLayer.UI;
using Marvin.ClientFramework;
using Marvin.ClientFramework.Commands;
using Marvin.ClientFramework.History;
using Marvin.Container;

namespace Marvin.Products.UI.Interaction
{
    [Plugin(LifeCycle.Singleton, typeof(IModuleWorkspace), Name = WorkspaceName)]
    internal class ProductsWorkspaceViewModel : MasterDetailsWorkspace<IProductDetails, IProductDetailsFactory, EmptyDetailsViewModel>
    {
        internal const string WorkspaceName = nameof(ProductsWorkspaceViewModel);

        #region Dependencies

        public IProductServiceModel Controller { get; set; }

        public IProductDialogFactory ProductDialogFactory { get; set; }

        public IRecipeEditorFactory RecipeEditorFactory { get; set; }

        public IHistoryWriter History { get; set; }

        #endregion

        #region Fields and Properties

        private FilterStatus _filterStatus;
        private ObservableCollection<StructureEntryViewModel> _fullTree;
        private ObservableCollection<StructureEntryViewModel> _displayStructure;
        private long _selectedProductId;

        public RelayCommand ImportDialogCmd { get; private set; }

        public DelegateCommand RemoveProductCmd { get; private set; }

        public RelayCommand ShowRevisionsCmd { get; private set; }

        public RelayCommand CreateRevisionCmd { get; private set; }

        public RelayCommand ShowRecipeEditorCmd { get; private set; }

        public ObservableCollection<StructureEntryViewModel> DisplayStructure
        {
            get { return _displayStructure; }
            private set
            {
                if (Equals(value, _displayStructure))
                    return;

                _displayStructure = value;
                NotifyOfPropertyChange();
            }
        }

        private StructureEntryViewModel _selectedProduct;
        public StructureEntryViewModel SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                if (Equals(value, _selectedProduct)) return;
                _selectedProduct = value;
                NotifyOfPropertyChange();
                RemoveProductCmd.RaiseCanExecuteChanged();
            }
        }

        public string SearchText
        {
            get { return _filterStatus.SearchText; }
            set
            {
                _filterStatus = _filterStatus.Search(value);

                // Update products if it changed
                if (_filterStatus.FilterChanged)
                {
                    FilterProducts();
                }
            }
        }

        #endregion

        protected override void OnInitialize()
        {
            base.OnInitialize();
            ImportDialogCmd = new RelayCommand(ImportProductDialog);
            RemoveProductCmd = new DelegateCommand(RemoveProduct, o => SelectedProduct != null);
            ShowRevisionsCmd = new RelayCommand(ShowRevisionsDialog, CanShowRevisions);
            CreateRevisionCmd = new RelayCommand(CreateRevision, CanCreateRevision);
            ShowRecipeEditorCmd = new RelayCommand(ShowRecipeEditor, CanShowRecipeEditor);

            Controller.StructureUpdated += OnStructureUpdated;

            if (Controller.Structure == null)
                return;

            LoadStructure();
        }

        private void OnStructureUpdated(object sender, EventArgs eventArgs)
        {
            LoadStructure();
        }

        private void LoadStructure()
        {
            //TODO: merge tree expansion
            IsMasterBusy = false;
            _fullTree = new ObservableCollection<StructureEntryViewModel>(Controller.Structure.Select(m => new StructureEntryViewModel(m)));

            if (_selectedProductId != 0)
            {
                SelectProductById();
            }
            else if (CurrentDetails.ProductId != 0)
            {
                _selectedProductId = CurrentDetails.ProductId;
                SelectProductById();
            }
            FilterProducts();
        }

        /// <summary>
        /// Filter the tree according to the search text.
        /// If the search text is empty or smaller than for the last search retrieve a new list.
        /// </summary>
        private void FilterProducts()
        {
            // Apply filter if necessary, otherwise use the full tree
            if (_filterStatus.FilterRequired)
            {
                // Search current list if filter was only extended
                var searchSpace = _filterStatus.FilterExtended ? DisplayStructure : _fullTree;
                DisplayStructure = FilterRecursive(searchSpace, _filterStatus);
            }
            else
            {
                DisplayStructure = _fullTree;
            }
        }

        /// <summary>
        /// Filter the given array recursively and expand all elements which contain
        /// elements that fit the search criteria
        /// </summary>
        private static ObservableCollection<StructureEntryViewModel> FilterRecursive(IEnumerable<StructureEntryViewModel> searchSpace, FilterStatus filter)
        {
            var results = new ObservableCollection<StructureEntryViewModel>();
            foreach (var entry in searchSpace)
            {
                // Check the entry itself
                if (filter.IsMatch(entry.Name))
                {
                    results.Add(entry);
                }
                // Check its branches
                else if (entry.Branches.Count > 0)
                {
                    var matchingBranches = FilterRecursive(entry.Branches, filter);
                    if (matchingBranches.Count == 0)
                        continue;

                    var clone = entry.Clone();
                    clone.IsExpanded = true;
                    clone.Branches = matchingBranches;
                    results.Add(clone);
                }
            }

            return results;
        }

        public override Task OnMasterItemChanged(object sender, RoutedPropertyChangedEventArgs<object> args)
        {
            return SelectProduct((StructureEntryViewModel)args.NewValue);
        }

        private async Task SelectProduct(StructureEntryViewModel entry)
        {
            if (entry?.MaterialNumber == null)
            {
                SelectedProduct = null;
                ShowEmpty();
                return;
            }

            if (!entry.IsProduct)
            {
                ShowEmpty();
                return;
            }

            SelectedProduct = entry;

            //Select view model for the right resource type
            var detailsVm = DetailsFactory.Create(SelectedProduct.Type);
            await LoadDetails(() => detailsVm.Load(SelectedProduct.Id));

            ActivateItem(detailsVm);
        }

        protected override void OnDetailsSaved(object sender, EventArgs e)
        {
            base.OnDetailsSaved(sender, e);

            Controller.UpdateStructure();
        }

        protected override void ShowEmpty()
        {
            EmptyDetails.Display(MessageSeverity.Info, "Please select a product from the tree on the left side.");
            base.ShowEmpty();
        }

        /// <summary>
        /// Show import product dialog
        /// </summary>
        private void ImportProductDialog(object parameters)
        {
            var dialog = ProductDialogFactory.CreateImportDialog();
            DialogManager.ShowDialog(dialog, delegate
            {
                if (dialog.ImportedProduct != null)
                {
                    _selectedProductId = dialog.ImportedProduct.Id;
                    Controller.UpdateStructure();
                }

                ProductDialogFactory.Destroy(dialog);
            });
        }


        private void RemoveProduct(object obj)
        {
            var dialog = ProductDialogFactory.CreateRemoveProductViewModel(SelectedProduct);
            DialogManager.ShowDialog(dialog, dialogResult =>
            {
                if (!dialog.Result)
                    return;

                // Remove product from the tree
                var productId = SelectedProduct.Id;
                RemoveFromCollection(Controller.Structure, e => e.Id == productId, e => e.Branches);
                RemoveFromCollection(_fullTree, e => e.Id == productId, e => e.Branches);
                if (_filterStatus.FilterRequired)
                    RemoveFromCollection(DisplayStructure, e => e.Id == productId, e => e.Branches);
                SelectedProduct = null;
            });
        }

        private void RemoveFromCollection<TEntry>(ICollection<TEntry> collection, Func<TEntry, bool> predicate, Func<TEntry, ICollection<TEntry>> recursionSelector)
        {
            foreach (var entry in collection.ToArray())
            {
                RemoveFromCollection(recursionSelector(entry), predicate, recursionSelector);

                if (predicate(entry))
                    collection.Remove(entry);
            }
        }

        /// <summary>
        /// Checks if the current product is not in the edit mode to ensure
        /// that no data will be lost after selecting a revision
        /// </summary>
        private bool CanShowRevisions(object obj)
        {
            return CurrentDetails.IsEditMode == false && SelectedProduct != null;
        }

        /// <summary>
        /// Shows the revision dialog
        /// </summary>
        private void ShowRevisionsDialog(object obj)
        {
            var dialog = ProductDialogFactory.CreateShowRevisionsDialog(SelectedProduct.MaterialNumber);
            DialogManager.ShowDialog(dialog, delegate (IRevisionsViewModel model)
            {
                var result = model.Result;

                if (result && model.SelectedRevision.HasValue)
                {
                    var selectedRevision = model.SelectedRevision.Value;

                    Task.Run(async delegate
                    {
                        var detailsVm = DetailsFactory.Create(SelectedProduct.Type);
                        await LoadDetails(() => detailsVm.Load(selectedRevision));

                        ActivateItem(detailsVm);
                    });
                }

                ProductDialogFactory.Destroy(dialog);
            });
        }

        /// <summary>
        /// Checks if the current product is not in the edit mode to ensure
        /// that no data will be lost after creation of a new revision
        /// </summary>
        protected virtual bool CanCreateRevision(object parameters)
        {
            return CurrentDetails.IsEditMode == false && SelectedProduct != null;
        }

        /// <summary>
        /// Opens a dialog for creating a new revision of the current product
        /// </summary>
        protected void CreateRevision(object parameters)
        {
            var dialogModel = ProductDialogFactory.CreateCreateRevisionDialog(SelectedProduct);
            DialogManager.ShowDialog(dialogModel, delegate
            {
                if (dialogModel.CreatedProductRevision != 0)
                {
                    Task.Run(async delegate
                    {
                        var detailsVm = DetailsFactory.Create(SelectedProduct.Type);
                        await LoadDetails(() => detailsVm.Load(dialogModel.CreatedProductRevision));

                        ActivateItem(detailsVm);
                        Controller.UpdateStructure();
                    });
                }
                ProductDialogFactory.Destroy(dialogModel);
            });
        }

        private bool CanShowRecipeEditor(object parameters)
        {
            return CurrentDetails.IsEditMode == false && SelectedProduct != null && SelectedProduct.IsProduct;
        }

        private void ShowRecipeEditor(object parameters)
        {
            var recipeWorkspace = RecipeEditorFactory.CreateRecipeEditor(SelectedProduct.Name, SelectedProduct.Id);
            History.Push(recipeWorkspace);
        }

        //TODO: Find a better way to reselect the old product.
        //TODO: Have a look on resource tree merge. It is already done there
        private void SelectProductById()
        {
            bool found = false;
            //search root product groups
            foreach (var root in _fullTree)
            {
                //search products in branch
                foreach (var branch in root.Branches)
                {
                    if (!branch.IsProduct || branch.Id != _selectedProductId)
                        continue;

                    //if product is found select it and expand group
                    root.IsExpanded = true;
                    branch.IsSelected = true;
                    found = true;
                    Task.Run(() => OnMasterItemChanged(null, new RoutedPropertyChangedEventArgs<object>(null, branch)));
                    break;
                }
                //already found so exit
                if (found)
                    break;
            }
            _selectedProductId = 0;
        }
    }
}