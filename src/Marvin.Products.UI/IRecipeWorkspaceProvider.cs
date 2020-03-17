// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

namespace Marvin.Products.UI
{
    /// <summary>
    /// Recipe editor facade
    /// </summary>
    public interface IRecipeWorkspaceProvider
    {
        /// <summary>
        /// Creates a workspace to load the given recipes
        /// </summary>
        /// <param name="title">Title of the recipe editor</param>
        /// <param name="recipeIds">List of recipes that can be edited</param>
        /// <returns>An instance that implements <see cref="IRecipeWorkspace"/></returns>
        IRecipeWorkspace CreateWorkspace(string title, params long[] recipeIds);
    }
}
