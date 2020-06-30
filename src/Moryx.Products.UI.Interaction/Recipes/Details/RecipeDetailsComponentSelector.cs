// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.AbstractionLayer.UI;
using Moryx.Container;

namespace Moryx.Products.UI.Interaction
{
    /// <summary>
    /// Component selector for resource view models
    /// </summary>
    [Plugin(LifeCycle.Singleton)]
    internal class RecipeDetailsComponentSelector : DetailsComponentSelector<IRecipeDetails>
    {
        public RecipeDetailsComponentSelector(IContainer container) : base(container)
        {
        }
    }
}
