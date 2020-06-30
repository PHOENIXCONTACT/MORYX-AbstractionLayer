// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Marvin.Container;

namespace Marvin.Products.UI.Interaction
{
    /// <summary>
    /// Factory to create <see cref="IRecipeWorkspace"/> instances
    /// </summary>
    [PluginFactory]
    public interface IRecipeWorkspaceFactory
    {
        /// <summary>
        /// Creates a <see cref="IRecipeWorkspace"/> with the given title and a set of recipe ids
        /// </summary>
        IRecipeWorkspace CreateRecipeWorkspace(string title, long[] recipeIds);
    }
}
