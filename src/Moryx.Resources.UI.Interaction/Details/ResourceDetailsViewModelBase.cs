// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using Moryx.AbstractionLayer.UI;
using Moryx.AbstractionLayer.UI.Aspects;
using Moryx.Logging;
using Moryx.Resources.UI.Interaction.Properties;
using Moryx.Resources.UI.ResourceService;
using Moryx.Tools;

namespace Moryx.Resources.UI.Interaction
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
        public IResourceServiceModel ResourceServiceModel { get; set; }

        /// <summary>
        /// Its a logger ...
        /// </summary>
        public IModuleLogger Logger { get; set; }

        /// <summary>
        /// Factory to create aspects
        /// </summary>
        public IAspectFactory AspectFactory { get; set; }

        /// <summary>
        /// Configuration of the module
        /// </summary>
        public ModuleConfig Config { get; set; }

        #endregion

        #region Fields and Properties

        /// <summary>
        /// ViewModel of the ResourceType
        /// </summary>
        public ResourceTypeViewModel Type { get; private set; }

        /// <summary>
        /// Depth of the resource tree for GetDetails
        /// </summary>
        protected virtual int DetailsDepth => 1;

        /// <summary>
        /// Property defining if aspects should be loaded or not
        /// </summary>
        protected virtual bool AspectUsage => false;

        /// <summary>
        /// View models of aspects from this resource
        /// </summary>
        public AspectConductorViewModel Aspects { get; } = new AspectConductorViewModel(Strings.ResourceDetailsViewModelBase_No_relevant_aspects);

        #endregion

        /// <inheritdoc />
        public void Initialize(string typeName)
        {
            Logger = Logger.GetChild(typeName, GetType());
        }

        /// <inheritdoc />
        protected override void OnActivate()
        {
            base.OnActivate();
            ScreenExtensions.TryActivate(Aspects);

        }

        /// <inheritdoc />
        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            ScreenExtensions.TryDeactivate(Aspects, close);
        }

        /// <inheritdoc />
        public virtual async Task Load(long resourceId)
        {
            var resources = await ResourceServiceModel.GetDetails(resourceId);

            await AssignLoadedResource(resources[0]);
        }

        /// <inheritdoc />
        public virtual Task Load(ResourceModel resource) =>
            AssignLoadedResource(resource);

        /// <summary>
        /// If an resource was loaded (new or existing), the resource
        /// can be assigned to this view model
        /// </summary>
        private async Task AssignLoadedResource(ResourceModel resource)
        {
            IsBusy = true;

            try
            {
                // Load type from type tree
                var typeTreeModel = await ResourceServiceModel.GetTypeTree();
                var flatTypeTree = typeTreeModel.DerivedTypes.Flatten(t => t.DerivedTypes).SingleOrDefault(t => t.Name == resource.Type);
                Type = new ResourceTypeViewModel(flatTypeTree);

                EditableObject = new ResourceViewModel(resource, Type);

                if (AspectUsage)
                {
                    var typedAspects = Config.AspectConfigurations.FirstOrDefault(ac => ac.TypeName == resource.Type);
                    List<AspectConfiguration> aspectConfigurations;
                    if (typedAspects == null || typedAspects.Aspects.Count == 0)
                        aspectConfigurations = Config.DefaultAspects;
                    else
                        aspectConfigurations = typedAspects.Aspects;

                    var aspects = aspectConfigurations.Select(ca => (IResourceAspect)AspectFactory.Create(ca.PluginName))
                        .Where(a => a.IsRelevant(EditableObject)).ToArray();

                    var aspectLoadTask = new List<Task>(aspects.Select(aspect => aspect.Load(EditableObject)));

                    await Task.WhenAll(aspectLoadTask);
                    Aspects.Items.AddRange(aspects);
                }

                Logger.Log(LogLevel.Trace, "Loaded resource with id {0}.", EditableObject.Id);
            }
            catch (Exception e)
            {
                //TODO
                throw;
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <inheritdoc />
        public override void Validate(ICollection<ValidationResult> validationErrors)
        {
            base.Validate(validationErrors);
            Aspects.Items.OfType<IResourceAspect>().ForEach(a => a.Validate(validationErrors));
        }

        ///
        public override async Task Save()
        {
            IsBusy = true;

            try
            {
                foreach (var aspect in Aspects.Items.Cast<IResourceAspect>())
                    await aspect.Save();

                var updated = await ResourceServiceModel.SaveResource(EditableObject.Model);
                EditableObject.UpdateModel(updated);
            }
            catch
            {
                //TODO
                throw;
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <inheritdoc />
        public override void BeginEdit()
        {
            Aspects.BeginEdit();
            base.BeginEdit();
        }

        /// <inheritdoc />
        public override void EndEdit()
        {
            Aspects.EndEdit();
            base.EndEdit();
        }

        /// <inheritdoc />
        public override void CancelEdit()
        {
            Aspects.CancelEdit();
            base.CancelEdit();
        }
    }
}
