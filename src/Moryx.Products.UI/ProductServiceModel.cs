// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.Threading.Tasks;
using Moryx.Products.UI.ProductService;
using Moryx.Serialization;
using Moryx.Tools.Wcf;
using Entry = Moryx.Products.UI.ProductService.Entry;

namespace Moryx.Products.UI
{
    internal class ProductServiceModel : HttpServiceConnectorBase<ProductInteractionClient, IProductInteraction>, IProductServiceModel
    {
        public ProductServiceModel(IWcfClientFactory clientFactory)
        {
            ClientFactory = clientFactory;
        }

        protected override string MinServerVersion => "1.1.2.0";

        protected override string ClientVersion => "1.1.2.0";

        protected override void ClientCallback(ConnectionState state, ProductInteractionClient client)
        {
            base.ClientCallback(state, client);

            if (state != ConnectionState.Success)
                _customization = null;
        }

        private ProductCustomization _customization;

        public async Task<ProductCustomization> GetCustomization()
        {
            if (_customization != null)
                return _customization;

            _customization = await WcfClient.GetCustomizationAsync();
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
            return WcfClient.GetProductsAsync(query);
        }

        public Task<ProductModel> CreateProduct(string type)
        {
            return WcfClient.CreateProductAsync(type);
        }

        public Task<ProductModel> GetProductDetails(long id)
        {
            return WcfClient.GetProductDetailsAsync(id);
        }

        public Task<ProductModel> SaveProduct(ProductModel product)
        {
            return WcfClient.SaveProductAsync(product);
        }

        public Task<DuplicateProductResponse> DuplicateProduct(long sourceId, string identifier, short revisionNo)
        {
            return WcfClient.DuplicateProductAsync(sourceId, identifier, revisionNo);
        }

        public Task<Entry> UpdateImportParameters(string importer, Entry currentParameters)
        {
            return WcfClient.UpdateParametersAsync(importer, currentParameters);
        }

        public Task<ProductModel> ImportProduct(string importerName, Entry parameters)
        {
            return WcfClient.ImportProductAsync(importerName, parameters);
        }

        public Task<bool> DeleteProduct(long productId)
        {
            return WcfClient.DeleteProductAsync(productId);
        }

        public Task<RecipeModel> GetRecipe(long recipeId)
        {
            return WcfClient.GetRecipeAsync(recipeId);
        }

        public Task<RecipeModel[]> GetRecipes(long productId)
        {
            return WcfClient.GetRecipesAsync(productId);
        }

        public Task<RecipeModel> CreateRecipe(string recipeType)
        {
            return WcfClient.CreateRecipeAsync(recipeType);
        }

        public Task<RecipeModel> SaveRecipe(RecipeModel recipe)
        {
            return WcfClient.SaveRecipeAsync(recipe);
        }

        public Task<WorkplanModel[]> GetWorkplans()
        {
            return WcfClient.GetWorkplansAsync();
        }
    }
}
