// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System;
using Moryx.AbstractionLayer.UI;

namespace Moryx.Resources.UI
{
    /// <summary>
    /// Registration attribute for the resource details view models
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ResourceDetailsRegistrationAttribute : DetailsRegistrationAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceDetailsRegistrationAttribute"/> class.
        /// </summary>
        /// <param name="typeName">Name of the resource type.</param>
        public ResourceDetailsRegistrationAttribute(string typeName)
            : base(typeName, typeof(IResourceDetails))
        {
        }
    }
}
