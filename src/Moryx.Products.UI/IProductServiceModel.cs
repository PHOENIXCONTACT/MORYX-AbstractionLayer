// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System;
using System.Threading.Tasks;
using Moryx.Communication.Endpoints;
using Moryx.Products.UI.ProductService;
using Entry = Moryx.Products.UI.ProductService.Entry;

namespace Moryx.Products.UI
{
    /// <summary>
    /// Model interface for product interaction
    /// </summary>
    public interface IProductServiceModel : IWebServiceConnector
    {
        /// <summary>
        /// Customization of the application, e.g. RecipeCreation, Importers, ....
        /// </summary>
        Task<ProductCustomization> GetCustomization();

        /// <summary>
        /// Returns the customization but forces reload
        /// </summary>
        Task<ProductCustomization> GetCustomization(bool force);

        /// <summary>
        /// Get producible root products
        /// </summary>
        Task<ProductModel[]> GetProducts(ProductQuery query);

        /// <summary>
        /// Create a new instance of the given type
        /// </summary>
        Task<ProductModel> CreateProduct(string type);

        /// <summary>
        /// Will return a full product
        /// </summary>
        Task<ProductModel> GetProductDetails(long id);

        /// <summary>
        /// Will save the product
        /// </summary>
        Task<ProductModel> SaveProduct(ProductModel product);

        /// <summary>
        /// Create a new revision or copy of the product
        /// </summary>
        Task<DuplicateProductResponse> DuplicateProduct(long sourceId, string identifier, short revisionNo);

        /// <summary>
        /// Update importer parameters
        /// </summary>
        /// <param name="importer">Name of the importer</param>
        /// <param name="currentParameters">List of parameters</param>
        /// <returns>List of importer parameters</returns>
        Task<Entry> UpdateImportParameters(string importer, Entry currentParameters);

        /// <summary>
        /// Call an importer with the given parameters
        /// </summary>
        /// <param name="importerName">Name of the importer</param>
        /// <param name="parameters">List of parameters</param>
        /// <returns>State of the import</returns>
        Task<ImportStateModel> Import(string importerName, Entry parameters);

        /// <summary>
        /// Returns the current importer state
        /// </summary>
        /// <param name="session">Guid of the session</param>
        Task<ImportStateModel> FetchImportProgress(Guid session);

        /// <summary>
        /// Remove a product from the database
        /// </summary>
        Task<bool> DeleteProduct(long productId);

        /// <summary>
        /// Gets the recipe by the give id
        /// </summary>
        /// <param name="recipeId">Id of the recipe</param>
        /// <returns><see cref="RecipeModel"/></returns>
        Task<RecipeModel> GetRecipe(long recipeId);

        /// <summary>
        /// Get all recipes for the given product
        /// </summary>
        Task<RecipeModel[]> GetRecipes(long productId);

        /// <summary>
        /// Create a new recipe
        /// </summary>
        Task<RecipeModel> CreateRecipe(string recipeType);

        /// <summary>
        /// Saves a production recipe instance
        /// </summary>
        /// <param name="recipe">Recipe instance to be saved</param>
        /// <returns>True if recipe was saved otherwise false</returns>
        Task<RecipeModel> SaveRecipe(RecipeModel recipe);

        /// <summary>
        /// Gets all workplans
        /// </summary>
        /// <returns>Array of WorkplanModel</returns>
        Task<WorkplanModel[]> GetWorkplans();
    }
}
