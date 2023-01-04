// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System;
using System.Collections.Generic;
using Moryx.AbstractionLayer.Capabilities;
using Moryx.AbstractionLayer.Resources;
using Moryx.Runtime.Modules;

namespace Moryx.Resources.Management
{
    internal class ResourceManagementFacade : IResourceManagement, IFacadeControl
    {
        #region Dependency Injection

        public IResourceManager Manager { get; set; }

        public IResourceGraph ResourceGraph { get; set; }

        public IResourceTypeController TypeController { get; set; }
   
        #endregion

        #region IFacadeControl
        /// <see cref="IFacadeControl.ValidateHealthState"/>
        public Action ValidateHealthState { get; set; }

        /// <seealso cref="IFacadeControl"/>
        public void Activate()
        {
            Manager.ResourceAdded += OnResourceAdded;
            Manager.CapabilitiesChanged += OnCapabilitiesChanged;
            Manager.ResourceRemoved += OnResourceRemoved;
        }

        /// <seealso cref="IFacadeControl"/>
        public void Deactivate()
        {
            Manager.ResourceAdded -= OnResourceAdded;
            Manager.CapabilitiesChanged -= OnCapabilitiesChanged;
            Manager.ResourceRemoved -= OnResourceRemoved;
        }

        private void OnCapabilitiesChanged(object sender, ICapabilities args)
        {
            CapabilitiesChanged?.Invoke(((IPublicResource)sender).Proxify(TypeController), args);
        }

        private void OnResourceAdded(object sender, IPublicResource publicResource)
        {
            ResourceAdded?.Invoke(this, publicResource.Proxify(TypeController));
        }

        private void OnResourceRemoved(object sender, IPublicResource publicResource)
        {
            ResourceRemoved?.Invoke(this, publicResource.Proxify(TypeController));
        }
        #endregion

        #region IResourceManagement
        public TResource GetResource<TResource>() where TResource : class, IPublicResource
        {
            ValidateHealthState();
            return ResourceGraph.GetResource<TResource>().Proxify(TypeController);
        }

        public TResource GetResource<TResource>(long id)
            where TResource : class, IPublicResource
        {
            ValidateHealthState();
            return ResourceGraph.GetResource<TResource>(id).Proxify(TypeController);
        }

        public TResource GetResource<TResource>(string name)
            where TResource : class, IPublicResource
        {
            ValidateHealthState();
            return ResourceGraph.GetResource<TResource>(name).Proxify(TypeController);
        }

        public TResource GetResource<TResource>(ICapabilities requiredCapabilities)
            where TResource : class, IPublicResource
        {
            ValidateHealthState();
            return ResourceGraph.GetResource<TResource>(r => requiredCapabilities.ProvidedBy(r.Capabilities)).Proxify(TypeController);
        }

        public TResource GetResource<TResource>(Func<TResource, bool> predicate)
            where TResource : class, IPublicResource
        {
            ValidateHealthState();
            return ResourceGraph.GetResource(predicate).Proxify(TypeController);
        }

        public IEnumerable<TResource> GetResources<TResource>() where TResource : class, IPublicResource
        {
            ValidateHealthState();
            return ResourceGraph.GetResources<TResource>().Proxify(TypeController);
        }

        public IEnumerable<TResource> GetResources<TResource>(ICapabilities requiredCapabilities)
            where TResource : class, IPublicResource
        {
            ValidateHealthState();
            return ResourceGraph.GetResources<TResource>(r => requiredCapabilities.ProvidedBy(r.Capabilities)).Proxify(TypeController);
        }

        public IEnumerable<TResource> GetResources<TResource>(Func<TResource, bool> predicate)
            where TResource : class, IPublicResource
        {
            ValidateHealthState();
            return ResourceGraph.GetResources(predicate).Proxify(TypeController);
        }


        #endregion

        #region IResourceModification
        public long Create(Type resourceType, Action<Resource> initializer)
        {
            ValidateHealthState();

            var resource = ResourceGraph.Instantiate(resourceType.ResourceType());
            initializer(resource);
            ResourceGraph.Save(resource);
            return resource.Id;
        }

        public TResult Read<TResult>(long id, Func<Resource, TResult> accessor)
        {
            ValidateHealthState();

            var resource = ResourceGraph.Get(id);

            var result = accessor(resource);
            return result;
        }

        public void Modify(long id, Func<Resource, bool> modifier)
        {
            ValidateHealthState();

            var resource = ResourceGraph.Get(id);
            if (resource == null)
                throw new KeyNotFoundException($"No resource with Id {id} found!");

            var result = modifier(resource);
            if (result)
                ResourceGraph.Save(resource);
        }

        public bool Delete(long id)
        {
            ValidateHealthState();

            var resource = ResourceGraph.Get(id);
            if (resource == null)
                return false;

            return ResourceGraph.Destroy(resource);
        }

        public IEnumerable<TResource> GetAllResources<TResource>(Func<TResource, bool> predicate)
             where TResource : class, IResource
        {
            ValidateHealthState();
            return ResourceGraph.GetResources(predicate);
        }
        #endregion


        /// <inheritdoc />
        public event EventHandler<IPublicResource> ResourceAdded;

        /// <inheritdoc />
        public event EventHandler<IPublicResource> ResourceRemoved;

        /// <inheritdoc />
        public event EventHandler<ICapabilities> CapabilitiesChanged;
    }

}
