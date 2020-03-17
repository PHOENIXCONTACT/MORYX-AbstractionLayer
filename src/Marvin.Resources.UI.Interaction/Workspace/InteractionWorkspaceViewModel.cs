using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using Marvin.AbstractionLayer.UI;
using Marvin.AbstractionLayer.UI.Aspects;
using Marvin.ClientFramework;
using Marvin.ClientFramework.Commands;
using Marvin.Container;
using Marvin.Resources.UI.Interaction.Properties;
using Marvin.Resources.UI.ResourceService;
using Marvin.Tools;
using MessageBoxImage = Marvin.ClientFramework.Dialog.MessageBoxImage;
using MessageBoxOptions = Marvin.ClientFramework.Dialog.MessageBoxOptions;

namespace Marvin.Resources.UI.Interaction
{
    [Plugin(LifeCycle.Singleton, typeof(IModuleWorkspace), Name = WorkspaceName)]
    internal class InteractionWorkspaceViewModel : MasterDetailsWorkspace<IResourceDetails, IResourceDetailsFactory, EmptyDetailsViewModel>
    {
        internal const string WorkspaceName = "InteractionWorkspaceViewModel";

        #region Dependencies

        /// <summary>
        /// Service model to interact with server
        /// </summary>
        public IResourceServiceModel ResourceServiceModel { get; set; }

        /// <summary>
        /// Factory for the aspect configuration
        /// </summary>
        public IAspectConfiguratorFactory AspectConfiguratorFactory { get; set; }

        /// <summary>
        /// Configuration for the aspects
        /// </summary>
        public ModuleConfig Config { get; set; }

        #endregion

        #region Fields and Properties

        public ICommand AddResourceCmd { get; }

        public ICommand RemoveResourceCmd { get; }

        public ICommand RefreshCmd { get; }

        public ICommand AspectConfiguratorCmd { get; }

        public ObservableCollection<TreeItemViewModel> Tree { get; } = new ObservableCollection<TreeItemViewModel>();

        private ResourceTreeItemViewModel _selectedResource;
        public ResourceTreeItemViewModel SelectedResource
        {
            get { return _selectedResource; }
            set
            {
                _selectedResource = value;
                NotifyOfPropertyChange();
            }
        }

        #endregion

        public InteractionWorkspaceViewModel()
        {
            AddResourceCmd = new AsyncCommand(AddResource, CanAddResource, true);
            RemoveResourceCmd = new AsyncCommand(RemoveResource, CanRemoveResource, true);
            RefreshCmd = new AsyncCommand(Refresh, CanRefresh, true);
            AspectConfiguratorCmd = new AsyncCommand(ShowAspectConfigurator, CanShowAspectConfigurator, true);
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            ResourceServiceModel.AvailabilityChanged += OnAvailabilityChanged;

            if (ResourceServiceModel.IsAvailable)
                UpdateTree();
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);

            if (close)
                ResourceServiceModel.AvailabilityChanged -= OnAvailabilityChanged;
        }

        private void OnAvailabilityChanged(object sender, EventArgs e)
        {
            if (ResourceServiceModel.IsAvailable)
                UpdateTree();
            else
                Execute.BeginOnUIThread(() => Tree.Clear());
        }

        /// <summary>
        /// Called from the resource tree if an selected item changed
        /// </summary>
        public override Task OnMasterItemChanged(object sender, RoutedPropertyChangedEventArgs<object> args)
        {
            return SelectResource((ResourceTreeItemViewModel)args.NewValue);
        }

        /// <summary>
        /// Select <see cref="ResourceTreeItemViewModel"/> and load the details
        /// </summary>
        private async Task SelectResource(ResourceTreeItemViewModel treeItem)
        {
            SelectedResource = treeItem;

            if (treeItem == null)
            {
                ShowEmpty();
                return;
            }

            //Select view model for the right resource type
            var detailsVm = DetailsFactory.Create(treeItem.Resource.TypeName);
            await LoadDetails(() => detailsVm.Load(treeItem.Resource.Id));

            ActivateItem(detailsVm);
        }

        /// <summary>
        /// Determines whether a new resource can be created as child of the selected resource
        /// </summary>
        private bool CanAddResource(object parameters) =>
            IsEditMode == false && ResourceServiceModel.IsAvailable;

        /// <summary>
        /// Will add a resource by type which will be selected via a dialog
        /// </summary>
        private async Task AddResource(object parameters)
        {
            var typeDialog = new TypeSelectorViewModel(ResourceServiceModel, SelectedResource);
            await DialogManager.ShowDialogAsync(typeDialog);
            if (!typeDialog.Result)
                return;

            IsBusy = true;

            var detailsVm = DetailsFactory.Create(typeDialog.SelectedType.Name);
            await LoadDetails(async delegate
            {
                await detailsVm.Load(typeDialog.ResourcePrototype);
            });

            // Add selected resource as parent reference
            if (SelectedResource != null)
            {
                var parentReference = detailsVm.EditableObject.References.First(r => r.Model.Name == "Parent");
                parentReference.Targets.Add(SelectedResource.Resource);
            }

            ActivateItem(detailsVm);
            EnterEdit();

            IsBusy = false;
        }

        /// <summary>
        /// Determines weather new resource can be deleted or not
        /// </summary>
        private bool CanRemoveResource(object parameters)
        {
            return IsEditMode == false && SelectedResource != null;
        }

        /// <summary>
        /// Removes the given resource.
        /// </summary>
        private async Task RemoveResource(object parameters)
        {
            var dialog = new RemoveResourceViewModel(ResourceServiceModel, CurrentDetails.EditableObject);
            await DialogManager.ShowDialogAsync(dialog);
            if (!dialog.Result)
                return;

            IsBusy = true;

            await UpdateTreeAsync();

            IsBusy = false;
        }

        private bool CanShowAspectConfigurator(object obj) =>
            Tree.Count > 0 && !IsEditMode;

        private async Task ShowAspectConfigurator(object obj)
        {
            var typeTree = await ResourceServiceModel.GetTypeTree();
            var allTypes = typeTree.DerivedTypes.Flatten(t => t.DerivedTypes);
            var dialog = AspectConfiguratorFactory.Create(Config.AspectConfigurations,
                allTypes.Select(p => p.Name).ToArray());
            dialog.DisplayName = "Configuration";

            await DialogManager.ShowDialogAsync(dialog);

            AspectConfiguratorFactory.Destroy(dialog);
        }

        private bool CanRefresh(object parameters) =>
            !IsEditMode && ResourceServiceModel.IsAvailable;

        private async Task Refresh(object parameters)
        {
            IsBusy = true;

            await UpdateTreeAsync();

            IsBusy = false;
        }

        protected override void OnCanceled()
        {
            base.OnCanceled();

            if (CurrentDetails.EditableObject.Id == 0)
                Task.Run(() => SelectResource(_selectedResource));
        }

        protected override Task OnSaved()
        {
            return UpdateTreeAsync();
        }

        protected override Task OnSaveError(Exception exception)
        {
            if (exception is TimeoutException)
            {
                return DialogManager.ShowMessageBoxAsync(Strings.InteractionWorkspaceViewModel_SaveTimeOut_Message,
                    Strings.InteractionWorkspaceViewModel_SaveTimeOut_Title, MessageBoxOptions.Ok, MessageBoxImage.Exclamation);
            }

            return base.OnSaveError(exception);
        }

        protected override void ShowEmpty()
        {
            EmptyDetails.Display(MessageSeverity.Info, Strings.InteractionWorkspaceViewModel_SelectResourceFromTree);
            base.ShowEmpty();
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

        /// <summary>
        /// Refreshes the resource tree and loads all data from the controller.
        /// </summary>
        private async Task UpdateTreeAsync()
        {
            try
            {
                IsBusy = true;

                var treeTask = ResourceServiceModel.GetResourceTree();
                var typeTask = ResourceServiceModel.GetTypeTree();
                var refreshedTree = await treeTask;
                await typeTask;
                Tree.MergeTree(refreshedTree, new ResourceTreeMergeStrategy());
            }
            catch (Exception e)
            {
                Tree?.Clear();
                ShowEmpty();
            }
            finally
            {
                IsBusy = false;
            }
        }

        public class ResourceTreeMergeStrategy : IMergeStrategy<ResourceModel, TreeItemViewModel>
        {
            public TreeItemViewModel FromModel(ResourceModel model)
            {
                return new ResourceTreeItemViewModel(model);
            }

            public void UpdateModel(TreeItemViewModel viewModel, ResourceModel model)
            {
                var resourceItem = (ResourceTreeItemViewModel)viewModel;
                resourceItem.UpdateModel(model);
            }
        }
    }
}