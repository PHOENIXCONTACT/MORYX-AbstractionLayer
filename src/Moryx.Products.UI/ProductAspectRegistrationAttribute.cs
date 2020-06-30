// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.AbstractionLayer.UI.Aspects;
using Moryx.Container;

namespace Moryx.Products.UI
{
    /// <summary>
    /// Registration attribute to register <see cref="IProductAspect"/> for a product group
    /// </summary>
    public class ProductAspectRegistrationAttribute : PluginAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductAspectRegistrationAttribute"/> class.
        /// </summary>
        public ProductAspectRegistrationAttribute(string name)
            : base(LifeCycle.Transient, typeof(IProductAspect), typeof(IAspect))
        {
            Name = name;
        }
    }
}
