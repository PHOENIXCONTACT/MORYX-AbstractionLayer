// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Moryx.WpfToolkit;
using Caliburn.Micro;
using Moryx.AbstractionLayer.UI;
using Moryx.AbstractionLayer.UI.Aspects;
using Moryx.ClientFramework;
using Moryx.ClientFramework.Commands;
using Moryx.Container;
using Moryx.Controls;
using Moryx.Products.UI.Interaction.Properties;
using Moryx.Products.UI.ProductService;
using Moryx.Tools;
using MessageBoxImage = Moryx.ClientFramework.Dialog.MessageBoxImage;
using MessageBoxOptions = Moryx.ClientFramework.Dialog.MessageBoxOptions;

namespace Moryx.Products.UI.Interaction
{
    [Plugin(LifeCycle.Singleton, typeof(IModuleWorkspace), Name = WorkspaceName)]
    internal class ProductsWorkspaceViewModel : MasterDetailsWorkspace<IProductDetails, IProductDetailsFactory, EmptyDetailsViewModel>
    {
        internal const string WorkspaceName = nameof(ProductsWorkspaceViewModel);

        #region Dependencies

        public IProductServiceModel ProductServiceModel { get; set; }

        /// <summary>
        /// Module config to read aspect configuration
        /// </summary>
        public ModuleConfig Config { get; set; }

        /// <summary>
        /// Factory for the aspect configuration
        /// </summary>
        public IAspectConfiguratorFactory AspectConfiguratorFactory { get; set; }

        #endregion

        #region Fields and Properties

        public ObservableCollection<TypeItemViewModel> ProductGroups { get; } = new ObservableCollection<TypeItemViewModel>();

        public ProductQueryViewModel Query { get; private set; }

        public ICommand RefreshCmd { get; }

        public ICommand ImportCmd { get; }

        public ICommand DuplicateCmd { get; }

        public ICommand RemoveCmd { get; }

        public ICommand ShowRevisionsCmd { get; }

        public ICommand FilterCmd { get; }

        public ICommand PropertyFilterCmd { get; }

        public ICommand AspectConfiguratorCmd { get; }

        private TreeItemViewModel _selectedItem;
        public TreeItemViewModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                NotifyOfPropertyChange();
            }
        }

        private ProductInfoViewModel _selectedRevision;
        public ProductInfoViewModel SelectedRevision
        {
            get { return _selectedRevision; }
            set
            {
                _selectedRevision = value;
                NotifyOfPropertyChange();
            }
        }

        private bool _isCustomQuery;

        public bool IsCustomQuery
        {
            get { return _isCustomQuery; }
            set
            {
                _isCustomQuery = value;
                NotifyOfPropertyChange();
            }
        }

        private ProductDefinitionViewModel[] _productTypes;
        public ProductDefinitionViewModel[] ProductTypes
        {
            get => _productTypes;
            set
            {
                _productTypes = value;
                NotifyOfPropertyChange();
            }
        }

        #endregion

        public ProductsWorkspaceViewModel()
        {
            RefreshCmd = new AsyncCommand(Refresh, CanRefresh, true);
            ImportCmd = new AsyncCommand(ImportProduct, CanImportProduct, true);
            DuplicateCmd = new AsyncCommand(DuplicateProduct, CanDuplicateProduct, true);
            RemoveCmd = new AsyncCommand(RemoveProduct, CanRemoveProduct, true);
            ShowRevisionsCmd = new AsyncCommand(ShowRevisions, CanShowRevisions, true);
            FilterCmd = new RelayCommand(ShowFilter);
            PropertyFilterCmd = new AsyncCommand(ShowPropertyFilter, _ => true, true);
            AspectConfiguratorCmd = new AsyncCommand(ShowAspectConfigurator, CanShowAspectConfigurator, true);

            // Set initial query
            ResetQuery();
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            ProductServiceModel.AvailabilityChanged += OnAvailabilityChanged;

            // Use initial query if service is available
            if (ProductServiceModel.IsAvailable)
                UpdateTree();
            else
                IsBusy = false;
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);

            if (close)
                ProductServiceModel.AvailabilityChanged -= OnAvailabilityChanged;
        }

        private void OnAvailabilityChanged(object sender, EventArgs e)
        {
            if (ProductServiceModel.IsAvailable)
                UpdateTree();
            else
                Execute.BeginOnUIThread(() => ProductGroups.Clear());
        }

        public override Task OnMasterItemChanged(object sender, RoutedPropertyChangedEventArgs<object> args)
        {
            return SelectTreeItem((TreeItemViewModel) args.NewValue);
        }

        private Task SelectTreeItem(TreeItemViewModel treeItem)
        {
            SelectedItem = treeItem;

            if (treeItem is ProductItemViewModel productItem)
            {
                // Set current revision when selecting a product
                SelectedRevision = productItem.Product;
                return ActivateDetails(productItem.Product.Type, productItem.Product.Id);
            }

            ShowEmpty();
            return Task.FromResult(true);
        }

        private async Task ActivateDetails(string productType, long detailsId)
        {
            //Select view model for the right resource type
            var detailsVm = DetailsFactory.Create(productType);
            await LoadDetails(() => detailsVm.Load(detailsId));

            ActivateItem(detailsVm);

            // Don't know why but validate all commands again
            CommandManager.InvalidateRequerySuggested();
        }

        protected override Task OnSaved()
        {
            return UpdateTreeAsync();
        }

        protected override Task OnSaveError(Exception exception)
        {
            if (exception is TimeoutException)
            {
                return DialogManager.ShowMessageBoxAsync(Strings.ProductsWorkspaceViewModel_SaveTimeOut_Message,
                    Strings.ProductsWorkspaceViewModel_SaveTimeOut_Title, MessageBoxOptions.Ok, MessageBoxImage.Exclamation);
            }

            return base.OnSaveError(exception);
        }

        private void UpdateTree()
        {
            Execute.BeginOnUIThread(async delegate
            {
                IsBusy = true;

                await UpdateTreeAsync();

                IsBusy = false;
            });
        }

        private async Task UpdateTreeAsync()
        {
            try
            {
                var customization = await ProductServiceModel.GetCustomization(true);
                var productTypes = customization.ProductTypes;
                ProductTypes = productTypes.Select(p => new ProductDefinitionViewModel(p)).ToArray();
                // TODO ERROR Clears
                var products = await ProductServiceModel.GetProducts(Query.GetQuery());

                Merge(productTypes, products);
            }
            catch(Exception e)
            {
                ProductGroups.Clear();
                ShowEmpty();
            }
        }

        protected override void ShowEmpty()
        {
            EmptyDetails.Display(MessageSeverity.Info, Strings.ProductsWorkspaceViewModel_SelectProduct);
            base.ShowEmpty();
        }

        private bool CanRefresh(object parameter) =>
            !IsEditMode && ProductServiceModel.IsAvailable;

        private async Task Refresh(object parameter)
        {
            IsBusy = true;

            var reset = parameter is bool && (bool)parameter;
            if (reset)
                ResetQuery();

            await UpdateTreeAsync();

            if (SelectedItem != null)
                await SelectTreeItem(SelectedItem);

            IsBusy = false;
            if (reset)
                IsCustomQuery = false;
        }

        private async Task ShowPropertyFilter(object arg)
        {
            var filterDialog = new PropertyFilterDialogViewModel(Query.Type);
            await DialogManager.ShowDialogAsync(filterDialog);
            if (!filterDialog.Result)
                return;

            var configuredProperties = filterDialog.CurrentFilter?.SubEntries
                                       ?? new ObservableCollection<EntryViewModel>();

            Query.PropertyFilters = configuredProperties.Select(c => new PropertyFilterViewModel(c.Entry)).ToList();
        }

        private void ShowFilter(object obj)
        {
            IsCustomQuery = !IsCustomQuery;
        }

        private bool CanImportProduct(object obj) =>
            !IsEditMode && ProductServiceModel.IsAvailable;

        private async Task ImportProduct(object parameters)
        {
            var dialog = new ImportViewModel(Logger, ProductServiceModel);
            await DialogManager.ShowDialogAsync(dialog);
            if (!dialog.Result)
                return;

            IsBusy = true;

            await UpdateTreeAsync();

            IsBusy = false;
        }

        private bool CanShowAspectConfigurator(object obj) =>
            ProductGroups.Count > 0 && !IsEditMode;

        private async Task ShowAspectConfigurator(object obj)
        {
            var dialog = AspectConfiguratorFactory.Create(Config.AspectConfigurations,
                ProductGroups.Select(p => p.TypeName).ToArray());
            dialog.DisplayName = "Configuration";

            await DialogManager.ShowDialogAsync(dialog);

            AspectConfiguratorFactory.Destroy(dialog);
        }

        private bool CanRemoveProduct(object obj) =>
            SelectedItem is ProductItemViewModel;

        private async Task RemoveProduct(object obj)
        {
            var dialog = new RemoveProductViewModel(ProductServiceModel, SelectedRevision);
            await DialogManager.ShowDialogAsync(dialog);
            if (!dialog.Result)
                return;

            if (IsEditMode)
                CancelEdit();

            IsBusy = true;

            await UpdateTreeAsync();

            IsBusy = false;
        }

        /// <summary>
        /// Checks if the current product is not in the edit mode to ensure
        /// that no data will be lost after selecting a revision
        /// </summary>
        private bool CanShowRevisions(object obj)
        {
            return !IsEditMode && SelectedItem is ProductItemViewModel;
        }

        /// <summary>
        /// Shows the revision dialog
        /// </summary>
        private async Task ShowRevisions(object obj)
        {
            var productTreeItem = (ProductItemViewModel) SelectedItem;

            var dialog = new RevisionsViewModel(ProductServiceModel, productTreeItem.Product);
            await DialogManager.ShowDialogAsync(dialog);
            if (!dialog.Result)
                return;

            if (dialog.NewRevisionRequested)
            {
                await CreateRevision(productTreeItem);
            }
            else if (dialog.SelectedRevision != null && dialog.SelectedRevision.Id != CurrentDetails.EditableObject.Id)
            {
                SelectedRevision = dialog.SelectedRevision;
                await ActivateDetails(SelectedRevision.Type, SelectedRevision.Id);
            }
        }

        /// <summary>
        /// Opens a dialog for creating a new revision of the current product
        /// </summary>
        private async Task CreateRevision(ProductItemViewModel productItem)
        {
            var dialog = new CreateRevisionViewModel(ProductServiceModel, productItem.Product);
            await DialogManager.ShowDialogAsync(dialog);
            if (!dialog.Result || dialog.CreatedProduct == null)
                return;

            var createdProduct = dialog.CreatedProduct;

            IsBusy = true;

            // Get group
            var group = ProductGroups.Single(t => t.TypeName == dialog.CreatedProduct.Type);

            // Create new tree item
            var newItem = new ProductItemViewModel(createdProduct.Model);
            group.Children.Add(newItem);

            // Change selection
            productItem.IsSelected = false;
            newItem.IsSelected = true;

            // Remove old version
            group.Children.Remove(productItem);

            // To be prepared, update complete tree
            await UpdateTreeAsync();

            // Now we are not busy anymore.
            IsBusy = false;
        }

        private bool CanDuplicateProduct(object obj) =>
            SelectedItem is ProductItemViewModel;

        private async Task DuplicateProduct(object obj)
        {
            var productItem = (ProductItemViewModel)SelectedItem;
            var dialog = new DuplicateProductDialogViewModel(ProductServiceModel, productItem.Product);
            await DialogManager.ShowDialogAsync(dialog);
            if (!dialog.Result)
                return;

            if (IsEditMode)
                CancelEdit();

            IsBusy = true;

            // Get group
            var group = ProductGroups.Single(t => t.TypeName == dialog.ClonedProduct.Type);

            // Create new tree item
            var newItem = new ProductItemViewModel(dialog.ClonedProduct);
            group.Children.Add(newItem);

            // Change selection
            productItem.IsSelected = false;
            newItem.IsSelected = true;

            // To be prepared, update complete tree
            await UpdateTreeAsync();

            // Now we are not busy anymore.
            IsBusy = false;
        }

        private void ResetQuery()
        {
            Query = new ProductQueryViewModel {RevisionFilter = RevisionFilter.Latest };
            NotifyOfPropertyChange(nameof(Query));
        }

        private void Merge(IEnumerable<ProductDefinitionModel> productTypes, ProductModel[] products)
        {
            if (!Config.ShowProductTypeTree)
            {
                var productItemMergeStrategy = new ProductItemMergeStrategy();
                var updatedGroups = new List<TypeItemViewModel>();
                foreach (var productType in productTypes)
                {
                    var groupItem = ProductGroups.FirstOrDefault(item => item.TypeName == productType.Name);
                    var productsForGroup = products.Where(p => p.Type == productType.Name);

                    if (groupItem == null)
                    {
                        groupItem = new TypeItemViewModel(productType);
                        groupItem.Children.AddRange(productsForGroup.Select(p => new ProductItemViewModel(p)));
                        ProductGroups.Add(groupItem);
                    }
                    else
                    {
                        groupItem.UpdateModel(productType);
                        groupItem.Children.MergeCollection(productsForGroup.ToArray(), productItemMergeStrategy);
                    }

                    updatedGroups.Add(groupItem);
                }

                var removedGroups = ProductGroups.Where(treeItem => !updatedGroups.Select(g => g.TypeName).Contains(treeItem.TypeName)).ToArray();
                ProductGroups.RemoveRange(removedGroups);
            }
            else
            {
                // Create target tree
                var targetStructure = new List<TypeItemViewModel>();
                var typeItems = productTypes.Select(p => new TypeItemViewModel(p)).ToList();
                foreach (var typeItem in typeItems)
                {
                    var productsForGroup = products.Where(p => p.Type == typeItem.TypeName);
                    typeItem.Children.AddRange(productsForGroup.Select(p => new ProductItemViewModel(p)));
                    var parent = typeItems.FirstOrDefault(i => i.TypeName == typeItem.Model.BaseDefinition);
                    if (parent != null)
                    {
                        parent.Children.Add(typeItem);
                    }
                    else
                    {
                        targetStructure.Add(typeItem);
                    }
                }

                // Update root entries
                var added = targetStructure.Where(t => ProductGroups.All(s => s.TypeName != t.TypeName));
                var removed = ProductGroups.Where(s => targetStructure.All(t => t.TypeName != s.TypeName));
                ProductGroups.RemoveRange(removed);
                ProductGroups.AddRange(added);

                // Update each branch
                foreach (var template in targetStructure)
                {
                    var toUpdate = ProductGroups.First(s => s.TypeName == template.TypeName);
                    toUpdate.UpdateModel(template.Model);
                    UpdateBranch(template, toUpdate);
                }
            }
        }

        private static void UpdateBranch(TypeItemViewModel template, TypeItemViewModel toUpdate)
        {
            var productsForGroup = template.Children.OfType<ProductItemViewModel>().Select(c => c.Product.Model);
            var subGroups = template.Children.OfType<TypeItemViewModel>();

            var removedProducts = toUpdate.Children
                .OfType<ProductItemViewModel>()
                .Where(c => productsForGroup.All(p => p.Id != c.Id));
            var removedSubGroups = toUpdate.Children
                .OfType<TypeItemViewModel>()
                .Where(c => subGroups.All(g => g.TypeName != c.TypeName));

            toUpdate.Children.RemoveRange(removedProducts);
            toUpdate.Children.RemoveRange(removedSubGroups);

            // Update products for parent group
            foreach (var productModel in productsForGroup)
            {
                var productItem = (ProductItemViewModel)toUpdate.Children.FirstOrDefault(c => c.Id == productModel.Id);
                if(productItem == null)
                    toUpdate.Children.Add(new ProductItemViewModel(productModel));
                else
                    productItem.UpdateModel(productModel);
            }
            
            // Update subgroups
            foreach (var group in subGroups)
            {
                var groupItem = toUpdate.Children.OfType<TypeItemViewModel>().FirstOrDefault(c => c.TypeName == group.TypeName);
                if (groupItem == null)
                {
                    groupItem = new TypeItemViewModel(group.Model);
                    toUpdate.Children.Add(groupItem);
                }
                else
                    groupItem.UpdateModel(group.Model);

                UpdateBranch(group, groupItem);
            }
        }

        private class ProductItemMergeStrategy : IMergeStrategy<ProductModel, TreeItemViewModel>
        {
            public TreeItemViewModel FromModel(ProductModel model) => new ProductItemViewModel(model);

            public void UpdateModel(TreeItemViewModel viewModel, ProductModel model) => ((ProductItemViewModel)viewModel).UpdateModel(model);
        }
    }
}
