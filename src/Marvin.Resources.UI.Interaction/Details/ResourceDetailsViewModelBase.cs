using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using Marvin.AbstractionLayer.UI;
using Marvin.Logging;
using Marvin.Resources.UI.Interaction.ResourceInteraction;
using Marvin.Serialization;
using Marvin.Tools;

namespace Marvin.Resources.UI.Interaction
{
    /// <summary>
    /// Base class for resource details view model
    /// </summary>
    public abstract class ResourceDetailsViewModelBase : EditModeViewModelBase<ResourceViewModel>, IResourceDetails
    {
        #region Dependencies

        /// <summary>
        /// Internal resource controller
        /// </summary>
        internal IResourceController ResourceController { get; private set; }

        /// <summary>
        /// Dialog factory for type selectors and method invocation
        /// </summary>
        public IResourceDialogFactory DialogFactory { get; set; }

        #endregion

        #region Fields and Properties

        /// <inheritdoc />
        public long CurrentResourceId { get; private set; }

        /// <summary>
        /// Current config entries
        /// </summary>
        protected Entry ConfigEntries
        {
            get { return EditableObject.Properties; }
            set { EditableObject.Properties = value; }
        }

        /// <summary>
        /// All methods of this type of resource
        /// </summary>
        public ResourceMethodViewModel[] Methods { get; private set; }

        /// <summary>
        /// Reference collection
        /// </summary>
        public ReferenceCollectionViewModel References { get; private set; }

        /// <summary>
        /// ViewModel of the ResourceType
        /// </summary>
        public ResourceTypeViewModel Type { get; private set; }

        /// <summary>
        /// Depth of the resource tree for GetDetails
        /// </summary>
        protected virtual int DetailsDepth => 1;

        #endregion

        /// <inheritdoc />
        public void Initialize(IInteractionController controller, string typeName)
        {
            base.Initialize();

            ResourceController = (IResourceController)controller;
            Logger = Logger.GetChild(typeName, GetType());

            Methods = new ResourceMethodViewModel[0];
        }

        /// <inheritdoc />
        protected override void OnActivate()
        {
            ScreenExtensions.TryActivate(References);
        }

        /// <inheritdoc />
        protected override void OnDeactivate(bool close)
        {
            ScreenExtensions.TryDeactivate(References, close);
        }

        /// 
        public virtual async Task Load(long resourceId)
        {
            CurrentResourceId = resourceId;

            // Load resource
            var resource = await ResourceController.GetDetails(resourceId, DetailsDepth);

            await AssignLoadedResource(resource);
        }

        ///
        public virtual async Task Create(string resourceTypeName, long parentResourceId, object constructorModel)
        {
            var resource = await ResourceController.CreateResource(resourceTypeName, constructorModel as MethodEntry);
            // Set parent relation
            var parent = resource.References.First(r => r.RelationType == ResourceRelationType.ParentChild && r.Role == ResourceReferenceRole.Source);
            var target = parent.PossibleTargets.FirstOrDefault(r => r.Id == parentResourceId);
            if (target != null)
                parent.Targets.Add(target);
            CurrentResourceId = resource.Id;

            await AssignLoadedResource(resource);
        }

        /// <summary>
        /// If an resource was loaded (new or extisting), the resource 
        /// can be assigned to this view model
        /// </summary>
        private async Task AssignLoadedResource(ResourceModel resource)
        {
            EditableObject = new ResourceViewModel(resource);
            NotifyOfPropertyChange(nameof(EditableObject));

            Methods = resource.Methods.Select(method => new ResourceMethodViewModel(method, this)).ToArray();

            // Filter unset or parent child relationship
            var references = resource.References.OrderBy(r => r.IsCollection)
                .Where(r => r.Targets != null && r.RelationType != ResourceRelationType.ParentChild)
                .Select(ReferenceViewModel.Create).ToArray();

            References = new ReferenceCollectionViewModel(references);
            NotifyOfPropertyChange(nameof(References));

            // Load type from type tree
            var typeModel = ResourceController.TypeTree.Flatten(t => t.DerivedTypes).SingleOrDefault(t => t.Name == EditableObject.Type);
            Type = new ResourceTypeViewModel(typeModel);

            await OnConfigLoaded();

            await OnResourceLoaded();
            Logger.Log(LogLevel.Trace, "Loaded resource with id {0}.", EditableObject.Id);
        }

        /// <summary>
        /// Method will be called if the resource was successfully loaded
        /// </summary>
        protected virtual Task OnResourceLoaded()
        {
            return SuccessTask;
        }

        /// <summary>
        /// Method will be called if the config was loaded successfully
        /// </summary>
        protected virtual Task OnConfigLoaded()
        {
            return SuccessTask;
        }

        /// <summary>
        /// Reload config of the object from the server
        /// </summary>
        /// <returns></returns>
        public async Task UpdateConfig()
        {
            var model = await ResourceController.GetDetails(CurrentResourceId, DetailsDepth);

            ConfigEntries = model.Properties;
        }

        /// <summary>
        /// Open the invocation dialog to execute a resource method
        /// </summary>
        protected internal async Task<Entry> OpenMethodInvocationDialog(ResourceMethodViewModel method)
        {
            var dialog = DialogFactory.CreateMethodInvocation(method);

            await DialogManager.ShowDialogAsync(dialog).ConfigureAwait(false);

            DialogFactory.Destroy(dialog);

            return dialog.ResultEntry;
        }

        /// <summary>
        /// Invoke a resource method directly
        /// </summary>
        protected async Task<Entry> InvokeResourceMethod(ResourceMethodViewModel method)
        {
            return await ResourceController.InvokeMethod(CurrentResourceId, method.Model);
        }

        ///
        protected override bool CanSave(object parameters)
        {
            return base.CanSave(parameters) && EditableObject.IsValid;
        }

        ///
        protected override async Task OnSave(object parameters)
        {
            await ResourceController.SaveResource(EditableObject.Model);
            await base.OnSave(parameters);
        }

        /// <inheritdoc />
        public override void BeginEdit()
        {
            base.BeginEdit();
            References.BeginEdit();
        }

        /// <inheritdoc />
        public override void EndEdit()
        {
            base.EndEdit();
            References.EndEdit();
        }

        /// <inheritdoc />
        public override void CancelEdit()
        {
            base.CancelEdit();
            References.CancelEdit();
        }
    }


    /// <summary>
    /// Typed base class of <see cref="ResourceDetailsViewModelBase"/>
    /// </summary>
    public class ResourceDetailsViewModelBase<T> : ResourceDetailsViewModelBase where T : INotifyPropertyChanged, new()
    {
        /// <summary>
        /// Dedicated converter for this type of details view model
        /// </summary>
        public static readonly EntryToModelConverter ConfigConverter = EntryToModelConverter.Create<T>();

        /// <summary>
        /// Typed view model for the config
        /// </summary>
        public T ConfigViewModel { get; private set; }

        ///
        protected override async Task OnConfigLoaded()
        {
            await base.OnConfigLoaded();

            ConfigViewModel = new T();
            ConfigConverter.FromConfig(ConfigEntries.SubEntries, ConfigViewModel);
        }

        ///
        public override void CancelEdit()
        {
            ConfigConverter.FromConfig(ConfigEntries.SubEntries, ConfigViewModel);

            base.CancelEdit();
        }

        ///
        public override void EndEdit()
        {
            ConfigConverter.ToConfig(ConfigViewModel, ConfigEntries.SubEntries);

            base.EndEdit();
        }
    }
}