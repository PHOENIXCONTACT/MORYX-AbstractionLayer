// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.AbstractionLayer.Products;
using Moryx.AbstractionLayer.Recipes;
using Moryx.Workflows;
using System;

namespace Moryx.Products.Management.Modification
{
    internal interface IProductConverter
    {
        ProductModel ConvertProduct(IProductType productType, bool flat);

        ProductDefinitionModel ConvertProductType(Type productType);

        IProductType ConvertProductBack(ProductModel source, ProductType target);

        RecipeModel ConvertRecipe(IRecipe recipe);

        IProductRecipe ConvertRecipeBack(RecipeModel recipe, IProductRecipe productRecipe, IProductType productType);

        WorkplanModel ConvertWorkplan(IWorkplan workplan);
    }
}
