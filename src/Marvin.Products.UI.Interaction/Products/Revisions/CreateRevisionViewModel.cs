using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using C4I;
using Caliburn.Micro;
using Marvin.ClientFramework.Commands;
using Marvin.ClientFramework.Dialog;
using Marvin.ClientFramework.Tasks;
using Marvin.Products.UI.ProductService;

namespace Marvin.Products.UI.Interaction
{
    internal class CreateRevisionViewModel : DialogScreen
    {
        private readonly IProductServiceModel _productServiceModel;
        private string _numberErrorMessage;
        private short _newRevision;
        private TaskNotifier _taskNotifier;
        private string _errorMessage;

        /// <summary>
        /// Command to create the new revision
        /// </summary>
        public ICommand CreateCmd { get; }

        /// <summary>
        /// Command for closing the dialog
        /// </summary>
        public ICommand CancelCmd { get; }

        private ProductModel[] _currentRevisions;

        public short NewRevision
        {
            get { return _newRevision; }
            set
            {
                _newRevision = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Task notifier to display a busy indicator
        /// </summary>
        public TaskNotifier TaskNotifier
        {
            get { return _taskNotifier; }
            private set
            {
                _taskNotifier = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Displays error message from revision number validation
        /// </summary>
        public string NumberErrorMessage
        {
            get { return _numberErrorMessage; }
            set
            {
                _numberErrorMessage = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Product to duplicate
        /// </summary>
        public ProductInfoViewModel Product { get; }

        /// <summary>
        /// Displays general error messages if new revision creation is failed
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
        /// Product which was be created after succeed OnCreateClick() call
        /// </summary>
        public ProductInfoViewModel CreatedProduct { get; private set; }

        /// <summary>
        /// Constructor for this view model
        /// </summary>
        public CreateRevisionViewModel(IProductServiceModel productServiceModel, ProductInfoViewModel product)
        {
            _productServiceModel = productServiceModel;
            Product = product;

            CreateCmd = new AsyncCommand(CreateRevision, CanCreateRevision, true);
            CancelCmd = new RelayCommand(Cancel, CanCancel);
        }

        /// <inheritdoc />
        protected override void OnInitialize()
        {
            base.OnInitialize();
            DisplayName = "New Revision";

            var loadingTask = Task.Run(async delegate
            {
                try
                {
                    _currentRevisions = await _productServiceModel.GetProducts(new ProductQuery
                    {
                        Identifier =  Product.Identifier ,
                        RevisionFilter = RevisionFilter.All
                    }).ConfigureAwait(false);

                    await Execute.OnUIThreadAsync(() => NewRevision = (short)(_currentRevisions.Max(pr => pr.Revision) + 1));
                }
                catch (Exception e)
                {
                    await Execute.OnUIThreadAsync(() => ErrorMessage = e.Message);
                }
            });

            TaskNotifier = new TaskNotifier(loadingTask);
        }

        private bool CanCreateRevision(object arg)
        {
            if (TaskNotifier != null && !TaskNotifier.IsCompleted)
                return false;

            NumberErrorMessage = string.Empty;

            var isRevisionNumberFree = _currentRevisions.All(pr => pr.Revision != NewRevision);
            if (!isRevisionNumberFree)
            {
                NumberErrorMessage = "Revision not available";
                return false;
            }

            return true;
        }

        private async Task CreateRevision(object arg)
        {
            ErrorMessage = string.Empty;

            try
            {
                var duplicateTask = _productServiceModel.DuplicateProduct(Product.Id, Product.Identifier, NewRevision);
                TaskNotifier = new TaskNotifier(duplicateTask);

                var response =  await duplicateTask;
                if (response.IdentityConflict || response.InvalidSource)
                {
                    ErrorMessage = "The given identity and revision conflicts with an existing.";
                }
                else
                {
                    CreatedProduct = new ProductInfoViewModel(response.Duplicate);
                    TryClose(true);
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
        }

        private bool CanCancel(object obj) =>
            !((AsyncCommand)CreateCmd).IsExecuting;

        private void Cancel(object parameters) =>
            TryClose(false);
    }
}