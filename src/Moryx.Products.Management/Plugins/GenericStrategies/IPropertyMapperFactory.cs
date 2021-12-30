// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.Container;
using System;

namespace Moryx.Products.Management.Plugins.GenericStrategies
{
    /// <summary>
    /// Factory to create instances of <see cref="IPropertyMapper"/>
    /// </summary>
    [PluginFactory(typeof(IConfigBasedComponentSelector))]
    internal interface IPropertyMapperFactory
    {
        /// <summary>
        /// Create a new mapper instance from config
        /// </summary>
        IPropertyMapper Create(PropertyMapperConfig config, Type targetType);

        /// <summary>
        /// Destroy a mapper instance
        /// </summary>
        void Destroy(IPropertyMapper instance);
    }
}
