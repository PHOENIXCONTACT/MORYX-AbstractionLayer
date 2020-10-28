// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.Threading.Tasks;
using Moryx.Products.UI.ProductService;
using Moryx.Tools.Wcf;
using Entry = Moryx.Products.UI.ProductService.Entry;

namespace Moryx.Products.UI
{
    /// <summary>
    /// Service model implementation for the products rest service
    /// </summary>
    internal class ProductServiceModel : WebHttpServiceConnectorBase, IProductServiceModel
    {
        private ProductCustomization _customization;

        public override string ServiceName => nameof(IProductInteraction);

        protected override string ClientVersion => "5.0.0";

        public ProductServiceModel(IWcfClientFactory clientFactory) : base(clientFactory)
        {
        }

        public override async Task ConnectionCallback(ConnectionState connectionState)
        {
            if (connectionState != ConnectionState.Success)
                _customization = null;
            else
            {
                await GetCustomization();
            }
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
    }
}