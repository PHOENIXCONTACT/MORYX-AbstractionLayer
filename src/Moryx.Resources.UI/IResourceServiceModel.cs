// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System;
using System.Threading.Tasks;
using Moryx.Modules;
using Moryx.Resources.UI.ResourceService;
using Moryx.Serialization;
using Moryx.Tools.Wcf;
using Entry = Moryx.Resources.UI.ResourceService.Entry;
using MethodEntry = Moryx.Resources.UI.ResourceService.MethodEntry;

namespace Moryx.Resources.UI
{
    /// <summary>
    /// Model interface for resource interaction
    /// </summary>
    public interface IResourceServiceModel : IHttpServiceConnector
    {
        /// <summary>
        /// Full type tree of the currently installed resources
        /// </summary>
        ResourceTypeModel TypeTree { get; }

        /// <summary>
        /// Returns the type tree of all resources
        /// </summary>
        Task<ResourceTypeModel> GetTypeTree();

        /// <summary>
        /// Returns the resource tree
        /// </summary>
        Task<ResourceModel[]> GetResourceTree();

        /// <summary>
        /// Returns the resource tree
        /// </summary>
        Task<ResourceModel[]> GetResources(ResourceQuery query);

        /// <summary>
        /// Creates a new resource with the given plugin name
        /// </summary>
        Task<ResourceModel> CreateResource(string typeName);

        /// <summary>
        /// Creates a new resource with the given plugin name
        /// </summary>
        Task<ResourceModel> CreateResource(string typeName, MethodEntry arguments);

        /// <summary>
        /// Saves the given resource with all changes
        /// </summary>
        Task<ResourceModel> SaveResource(ResourceModel resource);

        /// <summary>
        /// Removed the given resource
        /// </summary>
        Task<bool> RemoveResource(long resourceId);

        /// <summary>
        /// Gets the details of a resource with the resource id
        /// </summary>
        Task<ResourceModel[]> GetDetails(params long[] resourceIds);

        /// <summary>
        /// Invoke method on a resource object
        /// </summary>
        Task<Entry> InvokeMethod(long resourceId, MethodEntry method);
    }
}
