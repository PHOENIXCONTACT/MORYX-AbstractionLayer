// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using C4I;
using Caliburn.Micro;
using Marvin.ClientFramework.Dialog;
using Marvin.ClientFramework.Tasks;
using Marvin.Container;
using Marvin.Products.UI.Interaction.Properties;
using Marvin.Products.UI.ProductService;

namespace Marvin.Products.UI.Interaction.Aspects
{
    [Plugin(LifeCycle.Transient, typeof(SelectPartLinkDialogViewModel))]
    internal class SelectPartLinkDialogViewModel : DialogScreen
    {
        #region Dependencies

        public IProductServiceModel ProductServiceModel { get; set; }

        #endregion

        public override string DisplayName => Strings.SelectPartLinkDialogViewModel_DisplayName;

        private readonly PartConnectorViewModel _partConnector;

        public ICommand SelectCmd { get; }

        public ICommand CancelCmd { get; }

        public ProductInfoViewModel[] AvailableProducts { get; private set; } = new ProductInfoViewModel[0];

        private ProductInfoViewModel _selectedProduct;
        public ProductInfoViewModel SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
                NotifyOfPropertyChange();

                PartLink = _selectedProduct != null
                    ? _partConnector.CreatePartLink(_selectedProduct)
                    : null;
            }
        }

        private PartLinkViewModel _partLink;
        public PartLinkViewModel PartLink
        {
            get { return _partLink; }
            set
            {
                _partLink = value;
                NotifyOfPropertyChange();
            }
        }

        private TaskNotifier _taskNotifier;

        public TaskNotifier TaskNotifier
        {
            get { return _taskNotifier; }
            private set
            {
                _taskNotifier = value;
                NotifyOfPropertyChange(nameof(TaskNotifier));
            }
        }

        private string _errorMessage;

        /// <summary>
        /// Displays general error messages if something was wrong
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

        public SelectPartLinkDialogViewModel(PartConnectorViewModel partConnector)
        {
            _partConnector = partConnector;

            SelectCmd = new RelayCommand(Select, CanSelect);
            CancelCmd = new RelayCommand(Cancel);
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            var loadingTask = Task.Run(async delegate
            {
                try
                {
                    var productsLoadTask = ProductServiceModel.GetProducts(new ProductQuery
                    {
                        Type = _partConnector.Type,
                        RevisionFilter = RevisionFilter.All
                    });

                    TaskNotifier = new TaskNotifier(productsLoadTask);

                    var products = await productsLoadTask;
                    var productVms = products.Select(pm => new ProductInfoViewModel(pm)).OrderBy(p => p.FullIdentifier);

                    await Execute.OnUIThreadAsync(delegate
                    {
                        AvailableProducts = productVms.ToArray();
                        NotifyOfPropertyChange(nameof(AvailableProducts));
                    });
                }
                catch (Exception e)
                {
                    await Execute.OnUIThreadAsync(() => ErrorMessage = e.Message);
                }
            });

            TaskNotifier = new TaskNotifier(loadingTask);
        }

        private bool CanSelect(object obj) =>
            SelectedProduct != null && PartLink != null;

        private void Select(object obj) =>
            TryClose(true);

        private void Cancel(object obj) =>
            TryClose(false);
    }
}
