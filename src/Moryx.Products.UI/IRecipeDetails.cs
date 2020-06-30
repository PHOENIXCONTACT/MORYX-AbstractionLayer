// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.Collections.Generic;
using System.Threading.Tasks;
using Moryx.AbstractionLayer.UI;

namespace Moryx.Products.UI
{
    /// <summary>
    /// Recipe details interface
    /// </summary>
    public interface IRecipeDetails : IEditModeViewModel, IDetailsViewModel
    {
        /// <summary>
        /// Recipe view model which will be presented by this detail view
        /// </summary>
        RecipeViewModel EditableObject { get; }

        /// <summary>
        /// Method to load the recipe details
        /// </summary>
        Task Load(long recipeId, IReadOnlyCollection<WorkplanViewModel> workplans);

        /// <summary>
        /// Method to load the recipe details for en existing view model
        /// </summary>
        Task Load(RecipeViewModel recipeVm, IReadOnlyCollection<WorkplanViewModel> workplans);
    }
}
