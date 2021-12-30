// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.Container;
using System;

namespace Moryx.AbstractionLayer.Resources.Initializer
{
    /// <summary>
    /// Attribute to register resource initializers
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ResourceInitializerAttribute : PluginAttribute
    {
        /// <summary>
        /// Creates a resource initializer registration
        /// </summary>
        /// <param name="name">Name of this registration</param>
        public ResourceInitializerAttribute(string name)
            : base(LifeCycle.Singleton, typeof(IResourceInitializer))
        {
            Name = name;
        }
    }
}
