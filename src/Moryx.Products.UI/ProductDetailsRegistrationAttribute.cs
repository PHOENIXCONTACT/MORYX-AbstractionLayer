// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.AbstractionLayer.UI;

namespace Moryx.Products.UI
{
    /// <summary>
    /// Registration attribute to register <see cref="IProductDetails"/> for a product group
    /// </summary>
    public class ProductDetailsRegistrationAttribute : DetailsRegistrationAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductDetailsRegistrationAttribute"/> class.
        /// </summary>
        public ProductDetailsRegistrationAttribute(string typeName)
            : base(typeName, typeof(IProductDetails))
        {

        }
    }
}
