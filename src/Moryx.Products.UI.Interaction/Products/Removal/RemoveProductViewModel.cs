// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Moryx.WpfToolkit;
using Caliburn.Micro;
using Moryx.ClientFramework.Commands;
using Moryx.ClientFramework.Dialog;
using Moryx.ClientFramework.Tasks;
using Moryx.Products.UI.Interaction.Properties;
using Moryx.Products.UI.ProductService;
using Moryx.Tools;

namespace Moryx.Products.UI.Interaction
{
    internal class RemoveProductViewModel : DialogScreen
    {
        private readonly IProductServiceModel _productServiceModel;
        private TaskNotifier _taskNotifier;
        private string _errorMessage;

        /// <summary>
        /// Product which should be removed
        /// </summary>
        public ProductInfoViewModel ProductToRemove { get; }

        /// <summary>
        /// Command to execute the removal
        /// </summary>
        public AsyncCommand RemoveCmd { get; }

        /// <summary>
        /// Command to cancel this dialog
        /// </summary>
        public ICommand CancelCmd { get; }

        /// <summary>
        /// List of affected products
        /// </summary>
        public ObservableCollection<ProductInfoViewModel> AffectedProducts { get; } = new ObservableCollection<ProductInfoViewModel>();

        /// <summary>
        /// Task notifier to display a busy indicator
        /// </summary>
        public TaskNotifier TaskNotifier
        {
            get { return _taskNotifier; }
            set
            {
                _taskNotifier = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Error message while removing the product
        /// </summary>
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Constructor for this view model
        /// </summary>
        public RemoveProductViewModel(IProductServiceModel productServiceModel, ProductInfoViewModel product)
        {
            _productServiceModel = productServiceModel;
            ProductToRemove = product;

            RemoveCmd = new AsyncCommand(Remove, CanRemove, true);
            CancelCmd = new RelayCommand(Cancel, CanCancel);
        }

        /// <inheritdoc />
        protected override void OnInitialize()
        {
            DisplayName = Strings.RemoveProductViewModel_DisplayName;

            var loadingTask = Task.Run(async delegate
            {
                var affectedProducts = await _productServiceModel.GetProducts(new ProductQuery
                {
                    Identifier = ProductToRemove.Identifier,
                    Revision = ProductToRemove.Revision,
                    RevisionFilter = RevisionFilter.Specific,
                    Selector = Selector.Parent
                }).ConfigureAwait(false);

                var vms = affectedProducts.Select(r => new ProductInfoViewModel(r)).ToArray();
                await Execute.OnUIThreadAsync(delegate
                {
                    AffectedProducts.AddRange(vms);
                    ErrorMessage = Strings.RemoveProductViewModel_NonDeletableHint;
                });
            });

            TaskNotifier = new TaskNotifier(loadingTask);
        }

        private bool CanRemove(object parameters) =>
            AffectedProducts.Count == 0 && _productServiceModel.IsAvailable;

        private async Task Remove(object parameter)
        {
            try
            {
                var removalTask = _productServiceModel.DeleteProduct(ProductToRemove.Id);
                TaskNotifier = new TaskNotifier(removalTask);

                var result = await removalTask;
                if (result == false)
                {
                    ErrorMessage = Strings.RemoveProductViewModel_ErrorWhileRemove;
                }
                else
                {
                    TryClose(true);
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
        }

        private bool CanCancel(object obj) =>
            !RemoveCmd.IsExecuting;

        private void Cancel(object obj) =>
            TryClose(false);
    }
}
