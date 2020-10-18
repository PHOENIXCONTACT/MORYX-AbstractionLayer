// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Moryx.Communication;
using Moryx.Configuration;
using Moryx.Resources.UI.ResourceService;
using Moryx.Tools.Wcf;
using Newtonsoft.Json;
using MethodEntry = Moryx.Resources.UI.ResourceService.MethodEntry;
using Entry = Moryx.Resources.UI.ResourceService.Entry;


namespace Moryx.Resources.UI
{
    /// <summary>
    /// REST client with a few workarounds. Include proper Platform support
    /// </summary>
    internal class ResourceServiceModel : IResourceServiceModel
    {
        private readonly string _host;
        private readonly int _port;
        private readonly IProxyConfig _proxyConfig;

        private HttpClient _httpClient;

        private bool _isAvailable;
        public bool IsAvailable
        {
            get => _isAvailable;
            set
            {
                _isAvailable = value;
                AvailabilityChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler AvailabilityChanged;

        public ResourceTypeModel TypeTree { get; private set; }

        public ResourceServiceModel(string host, int port, IProxyConfig proxyConfig)
        {
            _host = host;
            _port = port;
            _proxyConfig = proxyConfig;
        }

        public void Start()
        {
            // Create HttpClient
            if (_proxyConfig?.EnableProxy == true && !_proxyConfig.UseDefaultWebProxy)
            {
                var proxy = new WebProxy
                {
                    Address = new Uri($"http://{_proxyConfig.Address}:{_proxyConfig.Port}"),
                    BypassProxyOnLocal = false,
                    UseDefaultCredentials = true
                };

                _httpClient = new HttpClient(new HttpClientHandler { Proxy = proxy });
            }
            else
            {
                _httpClient = new HttpClient();
            }
            _httpClient.BaseAddress = new Uri($"http://{_host}:{_port}/endpoints/");

            TryFetchEndpoint();
        }

        private void TryFetchEndpoint()
        {
            // Fetch endpoint name
            _httpClient.GetStringAsync($"service/{nameof(IResourceInteraction)}")
                .ContinueWith(EvaluateResponse);
        }

        private void EvaluateResponse(Task<string> resp)
        {
            //Try again or dispose old client
            if (resp.Status != TaskStatus.RanToCompletion || string.IsNullOrEmpty(resp.Result))
            {
                TryFetchEndpoint();
                return;
            }

            // Parse endpoint url
            var endpoints = JsonConvert.DeserializeObject<Endpoint[]>(resp.Result);
            var productEndpoint = endpoints.FirstOrDefault(e => e.Binding == ServiceBindingType.WebHttp)?.Address;
            if (string.IsNullOrEmpty(productEndpoint))
            {
                TryFetchEndpoint();
                return;
            }

            // Create new base address client
            _httpClient.Dispose();
            _httpClient = new HttpClient { BaseAddress = new Uri(productEndpoint) };

            IsAvailable = true;

            Task.Run(async delegate { await GetTypeTree(); });
        }

        public void Stop()
        {
            _httpClient?.Dispose();
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

        private async Task<T> GetAsync<T>(string url)
        {
            var response = await _httpClient.GetStringAsync(url);
            return JsonConvert.DeserializeObject<T>(response);
        }

        private async Task<T> PostAsync<T>(string url, object payload)
        {
            var payloadString = string.Empty;
            if (payload != null)
                payloadString = JsonConvert.SerializeObject(payload);

            var response = await _httpClient.PostAsync(url, new StringContent(payloadString, Encoding.UTF8, "text/json"));
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseContent);
        }

        private async Task<T> PutAsync<T>(string url, object payload)
        {
            var payloadString = string.Empty;
            if (payload != null)
                payloadString = JsonConvert.SerializeObject(payload);

            var response = await _httpClient.PutAsync(url, new StringContent(payloadString, Encoding.UTF8, "text/json"));
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseContent);
        }

        private async Task<bool> DeleteAsync(string url)
        {
            var response = await _httpClient.DeleteAsync(url);
            return response.StatusCode == HttpStatusCode.OK;
        }

        private class Endpoint
        {
            /// <summary>
            /// The binding type of the service.
            /// </summary>
            public ServiceBindingType Binding { get; set; }

            /// <summary>
            /// The URL of the service.
            /// </summary>
            public string Address { get; set; }

            /// <summary>
            /// The service's version
            /// </summary>
            public string Version { get; set; }
        }
    }
}
