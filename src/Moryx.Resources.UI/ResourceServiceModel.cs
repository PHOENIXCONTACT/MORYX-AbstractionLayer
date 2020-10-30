// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.Threading.Tasks;
using Moryx.Logging;
using Moryx.Resources.UI.ResourceService;
using Moryx.Tools.Wcf;
using MethodEntry = Moryx.Resources.UI.ResourceService.MethodEntry;
using Entry = Moryx.Resources.UI.ResourceService.Entry;

namespace Moryx.Resources.UI
{
    /// <summary>
    /// Service model implementation for the resource rest service
    /// </summary>
    internal class ResourceServiceModel : WebHttpServiceConnectorBase, IResourceServiceModel
    {
        public override string ServiceName => nameof(IResourceInteraction);

        protected override string ClientVersion => "5.0.0";

        public ResourceTypeModel TypeTree { get; private set; }

        public ResourceServiceModel(IWcfClientFactory clientFactory, IModuleLogger  logger)
            : base(clientFactory, logger.GetChild(nameof(ResourceServiceModel), typeof(ResourceServiceModel)))
        {
        }

        public override async Task ConnectionCallback(ConnectionState connectionState)
        {
            if (connectionState != ConnectionState.Success)
                TypeTree = new ResourceTypeModel();
            else
            {
                await GetTypeTree();
            }
        }

        public async Task<ResourceTypeModel> GetTypeTree()
        {
            TypeTree = await GetAsync<ResourceTypeModel>("types");
            return TypeTree;
        }

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

        public Task<ResourceModel[]> GetResources(ResourceQuery query)
        {
            return PostAsync<ResourceModel[]>("query", query);
        }

        public Task<ResourceModel> CreateResource(string typeName)
        {
            return PostAsync<ResourceModel>($"construct/{typeName}", null);
        }

        public Task<ResourceModel> CreateResource(string typeName, MethodEntry constructor)
        {
            return PostAsync<ResourceModel>($"construct/{typeName}?method={constructor.Name}", constructor.Parameters);
        }

        public Task<ResourceModel> SaveResource(ResourceModel resource)
        {
            return resource.Id == 0
                ? PostAsync<ResourceModel>("resource", resource)
                : PutAsync<ResourceModel>($"resource/{resource.Id}", resource);
        }

        public Task<bool> RemoveResource(long resourceId)
        {
            return DeleteAsync($"resource/{resourceId}");
        }

        public Task<ResourceModel[]> GetDetails(long[] resourceIds)
        {
            return GetAsync<ResourceModel[]>($"batch/{string.Join(",", resourceIds)}");
        }

        public Task<Entry> InvokeMethod(long resourceId, MethodEntry method)
        {
            return PostAsync<Entry>($"resource/{resourceId}/invoke/{method.Name}", method.Parameters);
        }
    }
}