// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Marvin.AbstractionLayer.UI.Aspects;
using Marvin.Container;

namespace Marvin.Resources.UI
{
    /// <summary>
    /// Registration attribute to register <see cref="IResourceAspect"/> for a resource
    /// </summary>
    public class ResourceAspectRegistrationAttribute : PluginAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceAspectRegistrationAttribute"/> class.
        /// </summary>
        /// <param name="name"></param>
        public ResourceAspectRegistrationAttribute(string name)
            : base(LifeCycle.Transient, typeof(IResourceAspect), typeof(IAspect))
        {
            Name = name;
        }
    }
}
