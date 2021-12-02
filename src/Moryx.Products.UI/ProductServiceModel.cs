// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System;
using System.Threading.Tasks;
using Moryx.Communication;
using Moryx.Logging;
using Moryx.Products.UI.ProductService;
using Moryx.Tools.Wcf;
using Entry = Moryx.Products.UI.ProductService.Entry;

namespace Moryx.Products.UI
{
    /// <summary>
    /// Service model implementation for the products rest service
    /// </summary>
    public class ProductServiceModel : WebHttpServiceConnectorBase, IProductServiceModel
    {
        private ProductCustomization _customization;

        /// <inheritdoc />
        public override string ServiceName => nameof(IProductInteraction);

        /// <inheritdoc />
        protected override string ClientVersion => "5.1.0";

        /// <inheritdoc />
        public ProductServiceModel(IWcfClientFactory clientFactory, IModuleLogger logger)
            : base(clientFactory, logger.GetChild(nameof(ProductServiceModel), typeof(ProductServiceModel)))
        {
        }

        /// <inheritdoc />
        public ProductServiceModel(string host, int port, IProxyConfig proxyConfig, IModuleLogger logger)
            : base(host, port, proxyConfig, logger.GetChild(nameof(ProductServiceModel), typeof(ProductServiceModel)))
        {
        }

        /// <inheritdoc />
        public override async Task ConnectionCallback(ConnectionState connectionState)
        {
            if (connectionState != ConnectionState.Success)
                _customization = null;
            else
            {
                await GetCustomization();
            }
        }

        /// <inheritdoc />
        public async Task<ProductCustomization> GetCustomization()
        {
            if (_customization != null)
                return _customization;

            _customization = await GetAsync<ProductCustomization>("customization");
            return _customization;
        }

        /// <inheritdoc />
        public Task<ProductCustomization> GetCustomization(bool force)
        {
            if (force)
                _customization = null;

            return GetCustomization();
        }

        /// <inheritdoc />
        public Task<ProductModel[]> GetProducts(ProductQuery query)
        {
            return PostAsync<ProductModel[]>($"query", query);
        }

        /// <inheritdoc />
        public Task<ProductModel> CreateProduct(string type)
        {
            return GetAsync<ProductModel>($"construct/{type}");
        }

        /// <inheritdoc />
        public Task<ProductModel> GetProductDetails(long id)
        {
            return GetAsync<ProductModel>($"product/{id}");
        }

        /// <inheritdoc />
        public Task<ProductModel> SaveProduct(ProductModel product)
        {
            return PutAsync<ProductModel>($"product/{product.Id}", product);
        }

        /// <inheritdoc />
        public Task<DuplicateProductResponse> DuplicateProduct(long sourceId, string identifier, short revisionNo)
        {
            var model = new ProductModel {Identifier = identifier, Revision = revisionNo};
            return PostAsync<DuplicateProductResponse>($"product/{sourceId}/duplicate", model);
        }

        /// <inheritdoc />
        public Task<Entry> UpdateImportParameters(string importer, Entry currentParameters)
        {
            return PutAsync<Entry>($"import/{importer}/parameters", currentParameters);
        }

        /// <inheritdoc />
        public Task<ImportStateModel> Import(string importerName, Entry parameters)
        {
            return PostAsync<ImportStateModel>($"import/{importerName}", parameters);
        }

        /// <inheritdoc />
        public Task<ImportStateModel> FetchImportProgress(Guid session)
        {
            return GetAsync<ImportStateModel>($"import/session/{session}");
        }

        /// <inheritdoc />
        public Task<bool> DeleteProduct(long productId)
        {
            return DeleteAsync($"product/{productId}");
        }

        /// <inheritdoc />
        public Task<RecipeModel> GetRecipe(long recipeId)
        {
            return GetAsync<RecipeModel>($"recipe/{recipeId}");
        }

        /// <inheritdoc />
        public Task<RecipeModel[]> GetRecipes(long productId)
        {
            return GetAsync<RecipeModel[]>($"recipes?product={productId}");
        }

        /// <inheritdoc />
        public Task<RecipeModel> CreateRecipe(string recipeType)
        {
            return PostAsync<RecipeModel>($"recipe/construct/{recipeType}", null);
        }

        /// <inheritdoc />
        public Task<RecipeModel> SaveRecipe(RecipeModel recipe)
        {
            return recipe.Id == 0
                ? PostAsync<RecipeModel>("recipe", recipe)
                : PutAsync<RecipeModel>($"recipe/{recipe.Id}", recipe);
        }

        /// <inheritdoc />
        public Task<WorkplanModel[]> GetWorkplans()
        {
            return GetAsync<WorkplanModel[]>("workplans");
        }
    }
}