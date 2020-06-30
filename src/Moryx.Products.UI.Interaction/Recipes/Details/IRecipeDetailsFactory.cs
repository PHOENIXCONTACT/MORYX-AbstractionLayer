// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.AbstractionLayer.UI;
using Moryx.Container;

namespace Moryx.Products.UI.Interaction
{
    [PluginFactory(typeof(RecipeDetailsComponentSelector))]
    internal interface IRecipeDetailsFactory : IDetailsFactory<IRecipeDetails>
    {
    }
}
