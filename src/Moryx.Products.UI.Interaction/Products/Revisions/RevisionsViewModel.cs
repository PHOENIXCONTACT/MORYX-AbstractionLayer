// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using Moryx.ClientFramework.Commands;
using Moryx.ClientFramework.Dialog;
using Moryx.ClientFramework.Tasks;
using Moryx.Products.UI.Interaction.Properties;
using Moryx.Products.UI.ProductService;
using Moryx.Tools;

namespace Moryx.Products.UI.Interaction
{
    internal class RevisionsViewModel : DialogScreen
    {
        #region Fields and Properties

        private readonly IProductServiceModel _productServiceModel;
        private TaskNotifier _taskNotifier;
        private ProductInfoViewModel _selectedRevision;
        private string _errorMessage;

        /// <summary>
        /// Base product for displaying revision
        /// </summary>
        public ProductInfoViewModel Product { get; }

        /// <summary>
        /// Current revisions of this product
        /// </summary>
        public ObservableCollection<ProductInfoViewModel> Revisions { get; } = new ObservableCollection<ProductInfoViewModel>();

        /// <summary>
        /// Command to open the selected revision
        /// </summary>
        public ICommand OpenCmd { get; }

        /// <summary>
        /// Create command to open the creation dialog
        /// </summary>
        public ICommand CreateCmd { get; }

        /// <summary>
        /// Close command to close the dialog
        /// </summary>
        public ICommand CloseCmd { get; }

        /// <summary>
        /// Task notifier to display a busy indicator
        /// </summary>
        public TaskNotifier TaskNotifier
        {
            get => _taskNotifier;
            private set
            {
                _taskNotifier = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Error message while display revisions
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

        /// <summary>
        /// Currently selected revision
        /// </summary>
        public ProductInfoViewModel SelectedRevision
        {
            get => _selectedRevision;
            set
            {
                _selectedRevision = value;
                NotifyOfPropertyChange();
            }
        }

        public bool NewRevisionRequested { get; private set; }

        #endregion

        public RevisionsViewModel(IProductServiceModel productServiceModel, ProductInfoViewModel product)
        {
            _productServiceModel = productServiceModel;
            Product = product;

            OpenCmd = new AsyncCommand(Open, CanOpen, true);
            CreateCmd = new AsyncCommand(Create, CanCreate, true);
            CloseCmd = new AsyncCommand(Close, CanClose, true);
        }

        protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            await base.OnInitializeAsync(cancellationToken);

            DisplayName = Strings.RevisionsViewModel_DisplayName;

            var loadingTask = Task.Run(async delegate
            {
                try
                {
                    var revisions = await _productServiceModel.GetProducts(new ProductQuery
                    {
                        Identifier = Product.Identifier,
                        RevisionFilter = RevisionFilter.All
                    }).ConfigureAwait(false);

                    var vms = revisions.Select(r => new ProductInfoViewModel(r)).ToArray();
                    await Execute.OnUIThreadAsync(() =>
                    {
                        Revisions.AddRange(vms);
                        return Task.CompletedTask;
                    });
                }
                catch (Exception e)
                {
                    await Execute.OnUIThreadAsync(() =>
                    {
                        ErrorMessage = e.Message;
                        return Task.CompletedTask;
                    });
                }

                finally
                {
                    await Execute.OnUIThreadAsync(() =>
                    {
                        CommandManager.InvalidateRequerySuggested();
                        return Task.CompletedTask;
                    });
                }
            }, cancellationToken);

            TaskNotifier = new TaskNotifier(loadingTask);
        }

        private bool IsNotBusy() =>
            TaskNotifier == null || TaskNotifier.IsCompleted;

        private bool CanOpen(object parameters) =>
            IsNotBusy() && SelectedRevision != null;

        private Task Open(object parameters) =>
            TryCloseAsync(true);

        private bool CanClose(object parameters) =>
            IsNotBusy();

        private Task Close(object parameters) =>
            TryCloseAsync(false);

        private bool CanCreate(object parameters) =>
            IsNotBusy();

        private Task Create(object parameters)
        {
            NewRevisionRequested = true;
            return TryCloseAsync(true);
        }
    }
}
