// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.Threading.Tasks;
using Moryx.Resources.UI.ResourceService;
using Moryx.Serialization;
using Moryx.Tools.Wcf;

namespace Moryx.Resources.UI
{
    internal class ResourceServiceModel : HttpServiceConnectorBase<ResourceInteractionClient, IResourceInteraction>, IResourceServiceModel
    {
        #region Properties

        protected override string MinServerVersion => "2.0.0";

        protected override string ClientVersion => "2.0.0";

        public ResourceTypeModel TypeTree { get; private set; }

        #endregion

        public ResourceServiceModel(IWcfClientFactory clientFactory)
        {
            ClientFactory = clientFactory;
            TypeTree = new ResourceTypeModel();
        }

        protected override void ClientCallback(ConnectionState state, ResourceInteractionClient client)
        {
            base.ClientCallback(state, client);

            if (state != ConnectionState.Success)
                TypeTree = new ResourceTypeModel();

            if (IsAvailable)
            {
                Task.Run(async delegate
                {
                    await GetTypeTree();
                });
            }
        }

        public async Task<ResourceTypeModel> GetTypeTree()
        {
            TypeTree = await WcfClient.GetTypeTreeAsync();
            return TypeTree;
        }

        public Task<ResourceModel[]> GetResourceTree()
        {
            return WcfClient.GetResourcesAsync(new ResourceQuery
            {
                ReferenceCondition = new ReferenceFilter
                {
                    Name = "Parent",
                    ValueConstraint = ReferenceValue.NullOrEmpty
                },
                ReferenceRecursion = true,
                IncludedReferences = new[]
                {
                    new ReferenceFilter{Name = "Children"}
                }
            });
        }

        public Task<ResourceModel[]> GetResources(ResourceQuery query)
        {
            return WcfClient.GetResourcesAsync(query);
        }

        public Task<ResourceModel> CreateResource(string typeName, MethodEntry constructor)
        {
            return WcfClient.CreateAsync(typeName, constructor);
        }

        public Task<ResourceModel> SaveResource(ResourceModel resource)
        {
            return WcfClient.SaveAsync(resource);
        }

        public Task<bool> RemoveResource(long resourceId)
        {
            return WcfClient.RemoveAsync(resourceId);
        }

        public Task<ResourceModel[]> GetDetails(long[] resourceIds)
        {
            return WcfClient.GetDetailsAsync(resourceIds);
        }

        public Task<Entry> InvokeMethod(long resourceId, MethodEntry method)
        {
            return WcfClient.InvokeMethodAsync(resourceId, method);
        }
    }
}
