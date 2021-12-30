// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System;

namespace Moryx.AbstractionLayer.Resources.Attributes
{
    /// <summary>
    /// Attribute to decorate methods that can be used to construct a resource instance
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ResourceConstructorAttribute : Attribute
    {
        /// <summary>
        /// Flag if this method represents the default constructor
        /// </summary>
        public bool IsDefault { get; set; }
    }
}
