using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using C4I;
using Caliburn.Micro;
using Marvin.ClientFramework.Commands;
using Marvin.ClientFramework.Dialog;
using Marvin.ClientFramework.Tasks;
using Marvin.Logging;
using Marvin.Products.UI.Interaction.Properties;
using Marvin.Products.UI.ProductService;

namespace Marvin.Products.UI.Interaction
{
    internal class ImportViewModel : DialogScreen
    {
        #region Fields and Properties

        private readonly IProductServiceModel _productServiceModel;
        private readonly IModuleLogger _logger;

        public ICommand OkCmd { get; private set; }

        public ICommand CancelCmd { get; private set; }

        #endregion

        private string _errorText;
        /// <summary>
        /// Error text
        /// </summary>
        public string ErrorText
        {
            get { return _errorText; }
            set
            {
                _errorText = value;
                NotifyOfPropertyChange();
            }
        }

        private BindableCollection<ImporterViewModel> _importers;
        public BindableCollection<ImporterViewModel> Importers
        {
            get { return _importers; }
            set
            {
                _importers = value;
                NotifyOfPropertyChange();
            }
        }

        private ImporterViewModel _selectedImporter;
        private TaskNotifier _taskNotifier;

        /// <summary>
        /// Selected importer
        /// </summary>
        public ImporterViewModel SelectedImporter
        {
            get { return _selectedImporter; }
            set
            {
                _selectedImporter = value;
                NotifyOfPropertyChange();
            }
        }

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
        /// Imported product
        /// </summary>
        public ProductModel ImportedProduct { get; private set; }

        /// <summary>
        /// Creates a new instance of the <see cref="ImportViewModel"/>
        /// </summary>
        public ImportViewModel(IModuleLogger logger, IProductServiceModel productServiceModel)
        {
            _logger = logger;
            _productServiceModel = productServiceModel;
        }

        protected override void OnInitialize()
        {
            DisplayName = Strings.ImportViewModel_DisplayName;

            OkCmd = new AsyncCommand(Ok);
            CancelCmd = new RelayCommand(Cancel);


            var loaderTask = Task.Run(async delegate
            {
                var customization = await _productServiceModel.GetCustomization().ConfigureAwait(false);
                await Execute.OnUIThreadAsync(delegate
                {
                    var importers = customization.Importers.Select(i => new ImporterViewModel(i, _productServiceModel));
                    Importers = new BindableCollection<ImporterViewModel>(importers);

                    if (Importers.Count > 0)
                        SelectedImporter = Importers[0];
                });
            });

            TaskNotifier = new TaskNotifier(loaderTask);
        }

        /// <summary>
        /// Imports product
        /// </summary>
        private async Task Ok(object obj)
        {
            if (!SelectedImporter.ValidateInput())
            {
                ErrorText = Strings.ImportViewModel_Fill_required_fields;
                return;
            }
            try
            {
                ImportedProduct = await SelectedImporter.Import();
            }
            catch (Exception ex)
            {
                ErrorText = (string.IsNullOrEmpty(ex.Message) ? Strings.ImportViewModel_Import_error : ex.Message) + "\n" + Strings.ImportViewModel_Import_error_info;
                _logger.LogException(LogLevel.Error, ex, ex.Message);
                return;
            }

            TryClose(true);
        }

        /// <summary>
        ///     Method call on Cancel
        /// </summary>
        private void Cancel(object obj)
        {
            TryClose(false);
        }
    }
}