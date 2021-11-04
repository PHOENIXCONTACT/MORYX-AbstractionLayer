// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using Moryx.ClientFramework.Commands;
using Moryx.ClientFramework.Dialog;
using Moryx.ClientFramework.Tasks;
using Moryx.Container;
using Moryx.Products.UI.Interaction.Properties;
using Moryx.Products.UI.ProductService;

namespace Moryx.Products.UI.Interaction.Aspects
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
            get => _selectedProduct;
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
            get => _partLink;
            set
            {
                _partLink = value;
                NotifyOfPropertyChange();
            }
        }

        private TaskNotifier _taskNotifier;

        public TaskNotifier TaskNotifier
        {
            get => _taskNotifier;
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
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                NotifyOfPropertyChange();
            }
        }

        public SelectPartLinkDialogViewModel(PartConnectorViewModel partConnector)
        {
            _partConnector = partConnector;

            SelectCmd = new AsyncCommand(Select, CanSelect, true);
            CancelCmd = new AsyncCommand(Cancel, _ => true, true);
        }

        protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            await base.OnInitializeAsync(cancellationToken);

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

                    Execute.OnUIThread(delegate
                    {
                        AvailableProducts = productVms.ToArray();
                        NotifyOfPropertyChange(nameof(AvailableProducts));
                    });
                }
                catch (Exception e)
                {
                    Execute.OnUIThread(() => ErrorMessage = e.Message);
                }
            }, cancellationToken);

            TaskNotifier = new TaskNotifier(loadingTask);
        }

        private bool CanSelect(object obj) =>
            SelectedProduct != null && PartLink != null;

        private Task Select(object obj) =>
            TryCloseAsync(true);

        private Task Cancel(object obj) =>
            TryCloseAsync(false);
    }
}
