// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.Threading.Tasks;
using Moryx.Communication;
using Moryx.Communication.Endpoints;
using Moryx.Logging;
using Moryx.Resources.UI.ResourceService;
using MethodEntry = Moryx.Resources.UI.ResourceService.MethodEntry;
using Entry = Moryx.Resources.UI.ResourceService.Entry;

namespace Moryx.Resources.UI
{
    /// <summary>
    /// Service model implementation for the resource rest service
    /// </summary>
    public class ResourceServiceModel : WebServiceConnectorBase, IResourceServiceModel
    {
        /// <inheritdoc />
        public override string ServiceName => "IResourceInteraction";

        /// <inheritdoc />
        protected override string ClientVersion => "6.0.0";

        /// <inheritdoc />
        public ResourceTypeModel TypeTree { get; private set; }

        /// <inheritdoc />
        public ResourceServiceModel(string host, int port, IProxyConfig proxyConfig, IModuleLogger logger)
            : base(host, port, proxyConfig, logger.GetChild(nameof(ResourceServiceModel), typeof(ResourceServiceModel)))
        {
        }

        /// <inheritdoc />
        public override async Task ConnectionCallback(ConnectionState connectionState)
        {
            if (connectionState != ConnectionState.Success)
                TypeTree = new ResourceTypeModel();
            else
            {
                await GetTypeTree();
            }
        }

        /// <inheritdoc />
        public async Task<ResourceTypeModel> GetTypeTree()
        {
            TypeTree = await GetAsync<ResourceTypeModel>("types");
            return TypeTree;
        }

        /// <inheritdoc />
        public Task<ResourceModel[]> GetResourceTree()
        {
            var query = new ResourceQuery
            {
                ReferenceCondition = new ReferenceFilter
                {
                    Name = "Parent",
                    ValueConstraint = ReferenceValue.NullOrEmpty
                },
                ReferenceRecursion = true,
                IncludedReferences = new[]
                {
                    new ReferenceFilter {Name = "Children"}
                }
            };

            return GetResources(query);
        }

        /// <inheritdoc />
        public Task<ResourceModel[]> GetResources(ResourceQuery query)
        {
            return PostAsync<ResourceModel[]>("query", query);
        }

        /// <inheritdoc />
        public Task<ResourceModel> CreateResource(string typeName)
        {
            return PostAsync<ResourceModel>($"construct/{typeName}", null);
        }

        /// <inheritdoc />
        public Task<ResourceModel> CreateResource(string typeName, MethodEntry constructor)
        {
            return PostAsync<ResourceModel>($"construct/{typeName}?method={constructor.Name}", constructor.Parameters);
        }

        /// <inheritdoc />
        public Task<ResourceModel> SaveResource(ResourceModel resource)
        {
            return resource.Id == 0
                ? PostAsync<ResourceModel>("resource", resource)
                : PutAsync<ResourceModel>($"resource/{resource.Id}", resource);
        }

        /// <inheritdoc />
        public Task<bool> RemoveResource(long resourceId)
        {
            return DeleteAsync($"resource/{resourceId}");
        }

        /// <inheritdoc />
        public Task<ResourceModel[]> GetDetails(long[] resourceIds)
        {
            return GetAsync<ResourceModel[]>($"batch/{string.Join(",", resourceIds)}");
        }

        /// <inheritdoc />
        public Task<Entry> InvokeMethod(long resourceId, MethodEntry method)
        {
            return PostAsync<Entry>($"resource/{resourceId}/invoke/{method.Name}", method.Parameters);
        }
    }
}