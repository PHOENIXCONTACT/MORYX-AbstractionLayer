// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Moryx.Communication;
using Moryx.Products.UI.ProductService;
using Moryx.Tools.Wcf;
using Newtonsoft.Json;
using Entry = Moryx.Products.UI.ProductService.Entry;

namespace Moryx.Products.UI
{
    /// <summary>
    /// Similar to ResourceServiceModel, replace with proper Platform support
    /// </summary>
    internal class ProductServiceModel : IProductServiceModel
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
        
        public ProductServiceModel(string host, int port, IProxyConfig proxyConfig)
        {
            _host = host;
            _port = port;
            _proxyConfig = proxyConfig;
        }

        private ProductCustomization _customization;

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
            _httpClient.GetStringAsync($"service/{nameof(IProductInteraction)}")
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

            Task.Run(async delegate
            {
                await GetCustomization();
            });
        }

        public void Stop()
        {
            _httpClient?.Dispose();
        }
        
        public async Task<ProductCustomization> GetCustomization()
        {
            if (_customization != null)
                return _customization;

            _customization = await GetAsync<ProductCustomization>("customization");
            return _customization;
        }

        public Task<ProductCustomization> GetCustomization(bool force)
        {
            if (force)
                _customization = null;

            return GetCustomization();
        }

        public Task<ProductModel[]> GetProducts(ProductQuery query)
        {
            return PostAsync<ProductModel[]>($"query", query);
        }

        public Task<ProductModel> CreateProduct(string type)
        {
            return GetAsync<ProductModel>($"construct/{type}");
        }

        public Task<ProductModel> GetProductDetails(long id)
        {
            return GetAsync<ProductModel>($"product/{id}");
        }

        public Task<ProductModel> SaveProduct(ProductModel product)
        {
            return PutAsync<ProductModel>($"product/{product.Id}", product);
        }

        public Task<DuplicateProductResponse> DuplicateProduct(long sourceId, string identifier, short revisionNo)
        {
            var model = new ProductModel {Identifier = identifier, Revision = revisionNo};
            return PostAsync<DuplicateProductResponse>($"product/{sourceId}/duplicate", model);
        }

        public Task<Entry> UpdateImportParameters(string importer, Entry currentParameters)
        {
            return PutAsync<Entry>($"import/{importer}/parameters", currentParameters);
        }

        public Task<ProductModel> ImportProduct(string importerName, Entry parameters)
        {
            return PostAsync<ProductModel>($"import/{importerName}", parameters);
        }

        public Task<bool> DeleteProduct(long productId)
        {
            return DeleteAsync($"product/{productId}");
        }

        public Task<RecipeModel> GetRecipe(long recipeId)
        {
            return GetAsync<RecipeModel>($"recipe/{recipeId}");
        }

        public Task<RecipeModel[]> GetRecipes(long productId)
        {
            return GetAsync<RecipeModel[]>($"recipes?product={productId}");
        }

        public Task<RecipeModel> CreateRecipe(string recipeType)
        {
            return PostAsync<RecipeModel>($"recipe/construct/{recipeType}", null);
        }

        public Task<RecipeModel> SaveRecipe(RecipeModel recipe)
        {
            return recipe.Id == 0
                ? PostAsync<RecipeModel>("recipe", recipe)
                : PutAsync<RecipeModel>($"recipe/{recipe.Id}", recipe);
        }

        public Task<WorkplanModel[]> GetWorkplans()
        {
            return GetAsync<WorkplanModel[]>("workplans");
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
