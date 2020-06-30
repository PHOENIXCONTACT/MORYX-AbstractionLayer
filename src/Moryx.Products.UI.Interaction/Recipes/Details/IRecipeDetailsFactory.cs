// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Marvin.AbstractionLayer.UI;
using Marvin.Container;

namespace Marvin.Products.UI.Interaction
{
    [PluginFactory(typeof(RecipeDetailsComponentSelector))]
    internal interface IRecipeDetailsFactory : IDetailsFactory<IRecipeDetails>
    {
    }
}
