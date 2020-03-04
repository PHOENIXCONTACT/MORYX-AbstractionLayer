using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using C4I;
using Caliburn.Micro;
using Marvin.AbstractionLayer.UI.Properties;
using Marvin.ClientFramework;
using Marvin.ClientFramework.Commands;
using Marvin.ClientFramework.Dialog;
using Marvin.Logging;
using MessageBoxImage = Marvin.ClientFramework.Dialog.MessageBoxImage;
using MessageBoxOptions = Marvin.ClientFramework.Dialog.MessageBoxOptions;

namespace Marvin.AbstractionLayer.UI
{
    /// <summary>
    /// Base workspace for master details view with edit mode
    /// </summary>
    /// <typeparam name="TDetailsType">The type of the details.</typeparam>
    /// <typeparam name="TDetailsFactory">The type of the details factory.</typeparam>
    /// <typeparam name="TEmptyDetails">The type of the empty details</typeparam>
    public abstract class MasterDetailsWorkspace<TDetailsType, TDetailsFactory, TEmptyDetails> : ModuleWorkspace<IScreen>.OneActive
        where TDetailsType : class, IEditModeViewModel
        where TDetailsFactory : IDetailsFactory<TDetailsType>
        where TEmptyDetails : EmptyDetailsViewModelBase, new()
    {
        #region Dependencies

        /// <summary>
        /// Factory to create or destroy detail view models
        /// </summary>
        public TDetailsFactory DetailsFactory { get; set; }

        /// <summary>
        /// Default dependency to show dialogs and MessageBoxes
        /// </summary>
        public IDialogManager DialogManager { get; set; }

        /// <summary>
        /// Its only a logger ;-)
        /// </summary>
        public IModuleLogger Logger { get; set; }

        #endregion

        #region Fields and Properties

        /// <summary>
        /// Command to enter the edit mode
        /// </summary>
        public ICommand EnterEditCmd { get; }

        /// <summary>
        /// Command to cancel the edit mode
        /// </summary>
        public ICommand CancelEditCmd { get; }

        /// <summary>
        /// Command to save the current details
        /// </summary>
        public IAsyncCommand SaveCmd { get; }

        /// <summary>
        /// Will represent the <see cref="ConductorBaseWithActiveItem{T}.ActiveItem"/> but in the detail type
        /// </summary>
        public TDetailsType CurrentDetails => (TDetailsType)ActiveItem;

        private bool _isEditMode;

        /// <summary>
        /// Edit mode
        /// </summary>
        public bool IsEditMode
        {
            get { return _isEditMode; }
            private set
            {
                _isEditMode = value;
                NotifyOfPropertyChange(nameof(IsEditMode));
            }
        }

        private bool _isBusy;

        /// <summary>
        /// Should be set to true if the master will load some  information
        /// </summary>
        public bool IsBusy
        {
            get { return _isBusy; }
            protected set
            {
                _isBusy = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Empty details
        /// </summary>
        protected TEmptyDetails EmptyDetails;

        #endregion

        /// <summary>
        /// Default constructor to initialize commands and so on
        /// </summary>
        protected MasterDetailsWorkspace()
        {
            EnterEditCmd = new RelayCommand(o => EnterEdit(), o => CanEnterEdit());
            CancelEditCmd = new RelayCommand(o => CancelEdit(), o => CanCancelEdit());
            SaveCmd = new AsyncCommand(Save, CanSave, true);
        }

        ///
        protected override void OnInitialize()
        {
            base.OnInitialize();

            // Show busy indicator on master because we are not sure if the master is currently loaded
            IsBusy = true;

            Logger = Logger.GetChild(typeof(TDetailsType).Name, GetType());

            // Create empty details here
            EmptyDetails = DetailsFactory.Create(DetailsConstants.EmptyType) as TEmptyDetails;
            ShowEmpty();
        }

        /// <summary>
        /// Gets called when the master item has been changed
        /// </summary>
        public virtual Task OnMasterItemChanged(object sender, RoutedPropertyChangedEventArgs<object> args)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// Loads the details view
        /// </summary>
        protected async Task LoadDetails(Func<Task> loaderAction)
        {
            IsBusy = true;

            try
            {
                await loaderAction();
            }
            catch (Exception e)
            {
                var errorMessage = Strings.MasterDetailsWorkspace_Load_error;
                Logger.LogException(LogLevel.Error, e, errorMessage);

                EmptyDetails.Display(MessageSeverity.Error, errorMessage);
                ActivateItem(EmptyDetails);
                throw;
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected bool CanEnterEdit() =>
            !IsEditMode && CurrentDetails.CanBeginEdit();

        protected void EnterEdit()
        {
            IsEditMode = true;
            CurrentDetails.BeginEdit();
        }

        protected bool CanCancelEdit() =>
            IsEditMode && CurrentDetails.CanCancelEdit();

        protected void CancelEdit()
        {
            CurrentDetails.CancelEdit();
            OnCanceled();
            IsEditMode = false;
        }

        protected virtual void OnCanceled()
        {

        }

        private bool CanSave(object parameters) =>
            IsEditMode && CurrentDetails.CanEndEdit();

        private async Task Save(object parameters)
        {
            IsBusy = true;

            try
            {
                var validationErrors = new List<ValidationResult>();
                CurrentDetails.Validate(validationErrors);

                if (validationErrors.Any())
                {
                    var message = string.Join(Environment.NewLine, validationErrors.Select(v => v.ErrorMessage));
                    await DialogManager.ShowMessageBoxAsync(message, "Error");
                    return;
                }

                CurrentDetails.EndEdit();

                await CurrentDetails.Save();
                await OnSaved();

                IsEditMode = false;
            }
            catch (Exception e)
            {
                // TODO: Maybe reset
                OnSaveError(e);
                throw;
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// Will be called if saving raises an exception
        /// </summary>
        protected virtual void OnSaveError(Exception exception)
        {
            DialogManager.ShowMessageBox("Saving failed with the following error: " + exception, "Error",
                MessageBoxOptions.Ok, MessageBoxImage.Error);
        }

        /// <summary>
        /// Will be called if the save procedure with the details was successful
        /// </summary>
        protected virtual Task OnSaved()
        {
            return Task.FromResult(true);
        }

        /// <inheritdoc />
        public override void ActivateItem(IScreen item)
        {
            if (item == ActiveItem)
                return;

            if (ActiveItem != null)
            {
                // TODO: ActiveItem is null after Deactivate
                base.DeactivateItem(ActiveItem, true);
                var detailItem = (TDetailsType)ActiveItem;
                DetailsFactory.Destroy(detailItem);
            }

            base.ActivateItem(item);
            NotifyOfPropertyChange(() => CurrentDetails);
        }

        /// <summary>
        /// Shows the empty view model with the message to select a product from the tree.
        /// </summary>
        protected virtual void ShowEmpty()
        {
            ActivateItem(EmptyDetails);
        }
    }
}