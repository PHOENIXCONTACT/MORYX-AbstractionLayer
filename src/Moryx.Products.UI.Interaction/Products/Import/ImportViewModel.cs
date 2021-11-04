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
using Moryx.Logging;
using Moryx.Products.UI.Interaction.Properties;
using Moryx.Products.UI.ProductService;

namespace Moryx.Products.UI.Interaction
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
            get => _errorText;
            set
            {
                _errorText = value;
                NotifyOfPropertyChange();
            }
        }

        private BindableCollection<ImporterViewModel> _importers;
        public BindableCollection<ImporterViewModel> Importers
        {
            get => _importers;
            set
            {
                _importers = value;
                NotifyOfPropertyChange();
            }
        }

        private ImporterViewModel _selectedImporter;
        private TaskNotifier _taskNotifier;
        private CancellationTokenSource _currentImportCts;

        /// <summary>
        /// Selected importer
        /// </summary>
        public ImporterViewModel SelectedImporter
        {
            get => _selectedImporter;
            set
            {
                _selectedImporter = value;
                NotifyOfPropertyChange();
            }
        }

        public TaskNotifier TaskNotifier
        {
            get => _taskNotifier;
            set
            {
                _taskNotifier = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ImportViewModel"/>
        /// </summary>
        public ImportViewModel(IModuleLogger logger, IProductServiceModel productServiceModel)
        {
            _logger = logger;
            _productServiceModel = productServiceModel;
        }

        protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            await base.OnInitializeAsync(cancellationToken);

            DisplayName = Strings.ImportViewModel_DisplayName;

            OkCmd = new AsyncCommand(Ok);
            CancelCmd = new AsyncCommand(Cancel, o => true, true);

            var loaderTask = Task.Run(async delegate
            {
                var customization = await _productServiceModel.GetCustomization().ConfigureAwait(false);
                Execute.OnUIThread(delegate
                {
                    var importers = customization.Importers.Select(i => new ImporterViewModel(i, _productServiceModel));
                    Importers = new BindableCollection<ImporterViewModel>(importers);

                    if (Importers.Count > 0)
                        SelectedImporter = Importers[0];
                });
            }, cancellationToken);

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
                var importState = await SelectedImporter.Import();
                if (importState.Completed && string.IsNullOrEmpty(importState.ErrorMessage))
                {
                    await TryCloseAsync(true);
                }
                else if (importState.Completed && !string.IsNullOrEmpty(importState.ErrorMessage))
                {
                    ErrorText = importState.ErrorMessage;
                }
                else
                {
                    _currentImportCts = new CancellationTokenSource();
                    await RunImportPolling(importState, _currentImportCts.Token);
                }
            }
            catch (Exception ex)
            {
                ErrorText = (string.IsNullOrEmpty(ex.Message) ? Strings.ImportViewModel_Import_error : ex.Message) + "\n" + Strings.ImportViewModel_Import_error_info;
                _logger.LogException(LogLevel.Error, ex, ex.Message);
            }
        }

        private async Task RunImportPolling(ImportStateModel initialState, CancellationToken cancellationToken)
        {
            var currentImportState = initialState;

            while (!cancellationToken.IsCancellationRequested)
            {
                if (!currentImportState.Completed)
                {
                    await Task.Delay(1000, cancellationToken);
                    currentImportState = await _productServiceModel.FetchImportProgress(currentImportState.Session);
                    continue;
                }

                if (string.IsNullOrEmpty(currentImportState.ErrorMessage))
                {
                    await TryCloseAsync(true);
                }
                else
                {
                    Execute.OnUIThread(() =>ErrorText = currentImportState.ErrorMessage);
                }

                break;
            }
        }

        /// <summary>
        /// Method call on Cancel
        /// </summary>
        private Task Cancel(object obj)
        {
            if (_currentImportCts != null)
            {
                _currentImportCts.Cancel();
                _currentImportCts.Dispose();
                _currentImportCts = null;
            }

            return TryCloseAsync(false);
        }
    }
}
