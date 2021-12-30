// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.AbstractionLayer.Products;
using Moryx.AbstractionLayer.Recipes;
using Moryx.Modules;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Moryx.Products.Management.Components
{
    /// <summary>
    /// API for the application specific product storage
    /// </summary>
    public interface IProductStorage : IPlugin
    {
        /// <summary>
        /// Get products by query
        /// </summary>
        IReadOnlyList<IProductType> LoadTypes(ProductQuery query);

        /// <summary>
        /// Load product instance by id
        /// </summary>
        IProductType LoadType(long id);

        /// <summary>
        /// Load product by identity. This method supports loading a products latest revision
        /// </summary>
        IProductType LoadType(ProductIdentity identity);

        /// <summary>
        /// Save a type to the storage
        /// </summary>
        long SaveType(IProductType modifiedInstance);

        /// <summary>
        /// Get instances by id
        /// </summary>
        /// <returns>The instance with the id when it exists.</returns>
        IReadOnlyList<ProductInstance> LoadInstances(params long[] id);

        /// <summary>
        /// Load instances using filter expression
        /// </summary>
        IReadOnlyList<TInstance> LoadInstances<TInstance>(Expression<Func<TInstance, bool>> selector);

        /// <summary>
        /// Updates the database from the instance
        /// </summary>
        void SaveInstances(ProductInstance[] productInstance);

        /// <summary>
        /// Loads a recipe from the storage
        /// </summary>
        IProductRecipe LoadRecipe(long recipeId);

        /// <summary>
        /// Loads all recipes from the storage.
        /// </summary>
        IReadOnlyList<IProductRecipe> LoadRecipes(long productId, RecipeClassification classification);

        /// <summary>
        /// Saves the recipe of the product
        /// </summary>
        long SaveRecipe(IProductRecipe recipe);

        /// <summary>
        /// Save multiple recipes at once
        /// </summary>
        void SaveRecipes(long productId, ICollection<IProductRecipe> recipes);
    }

    /// <summary>
    /// Additional interface for type storage to search for product types by expression
    /// TODO: Remove in AL 6
    /// </summary>
    public interface IProductSearchStorage : IProductStorage
    {
        /// <summary>
        /// Load types using filter expression
        /// </summary>
        IReadOnlyList<TType> LoadTypes<TType>(Expression<Func<TType, bool>> selector);
    }
}
