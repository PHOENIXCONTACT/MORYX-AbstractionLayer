// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.Collections.Generic;
using System.Threading.Tasks;
using Marvin.AbstractionLayer.UI;

namespace Marvin.Products.UI.Interaction
{
    /// <summary>
    /// ViewModel for non selected recipe details
    /// </summary>
    [RecipeDetailsRegistration(DetailsConstants.EmptyType)]
    public class EmptyRecipeDetailsViewModel : EmptyDetailsViewModelBase, IRecipeDetails
    {
        /// <inheritdoc />
        public Task Load(long productId, IReadOnlyCollection<WorkplanViewModel> workplans)
        {
            return SuccessTask;
        }

        /// <inheritdoc />
        public Task Load(RecipeViewModel recipeVm, IReadOnlyCollection<WorkplanViewModel> workplans)
        {
            return SuccessTask;
        }

        /// <inheritdoc />
        public RecipeViewModel EditableObject => null;
    }
}
