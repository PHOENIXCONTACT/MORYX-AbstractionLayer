// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.Products.Management.Plugins.GenericStrategies;
using System.Collections.Generic;

namespace Moryx.Products.Management.Components
{
    /// <summary>
    /// Interface for <see cref="IProductStrategyConfiguation"/> that contains property mapper configuration
    /// </summary>
    public interface IPropertyMappedConfiguration
    {
        /// <summary>
        /// Configuration for individual property mappers
        /// </summary>
        List<PropertyMapperConfig> PropertyConfigs { get; }
    }
}
