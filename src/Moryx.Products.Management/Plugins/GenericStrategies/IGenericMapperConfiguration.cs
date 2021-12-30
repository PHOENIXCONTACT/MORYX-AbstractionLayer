// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.Products.Management.Components;

namespace Moryx.Products.Management.Plugins.GenericStrategies
{
    /// <summary>
    /// Config interface for generic strategies
    /// </summary>
    internal interface IGenericMapperConfiguration : IPropertyMappedConfiguration
    {
        /// <summary>
        /// Column that should be used to store all non-configured properties as JSON
        /// </summary>
        string JsonColumn { get; }
    }
}
