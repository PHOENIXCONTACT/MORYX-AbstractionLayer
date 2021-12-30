// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.Container;
using Moryx.Products.Management.Implementations.Storage;

namespace Moryx.Products.Management.Components
{
    /// <summary>
    /// Factory to instantiate <see cref="IProductTypeStrategy"/>
    /// </summary>
    [PluginFactory(typeof(IConfigBasedComponentSelector))]
    public interface IStorageStrategyFactory
    {
        /// <summary>
        /// Create a new strategy instance
        /// </summary>
        IProductTypeStrategy CreateTypeStrategy(ProductTypeConfiguration config);

        /// <summary>
        /// Create a new strategy instance
        /// </summary>
        IProductInstanceStrategy CreateInstanceStrategy(ProductInstanceConfiguration config);

        /// <summary>
        /// Create a new strategy instance
        /// </summary>
        IProductLinkStrategy CreateLinkStrategy(ProductLinkConfiguration config);

        /// <summary>
        /// Create a new strategy instance
        /// </summary>
        IProductRecipeStrategy CreateRecipeStrategy(ProductRecipeConfiguration config);

        /// <summary>
        /// Destroy an instance
        /// </summary>
        void Destroy(object strategy);
    }
}
