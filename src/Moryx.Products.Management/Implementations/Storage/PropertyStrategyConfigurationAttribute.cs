// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System;

namespace Moryx.Products.Management.Implementations.Storage
{
    /// <summary>
    /// Attribute for <see cref="IPropertyMapper"/> to define their column requirement
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class PropertyStrategyConfigurationAttribute : StrategyConfigurationAttribute
    {
        /// <summary>
        /// Type of column required by the property mapper
        /// </summary>
        public Type ColumnType { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="supportedTypes"></param>
        public PropertyStrategyConfigurationAttribute(params Type[] supportedTypes) : base(supportedTypes)
        {
        }
    }
}