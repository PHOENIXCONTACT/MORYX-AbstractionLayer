// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System;
using Moryx.Container;

namespace Moryx.AbstractionLayer.UI
{
    /// <summary>
    /// Base registration attribute for detail view models
    /// </summary>
    public abstract class DetailsRegistrationAttribute : PluginAttribute
    {
        /// <summary>
        /// Type of the view model
        /// </summary>
        public string TypeName { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DetailsRegistrationAttribute"/> class.
        /// </summary>
        protected DetailsRegistrationAttribute(string typeName, Type detailsType)
            : base(LifeCycle.Transient, detailsType)
        {
            TypeName = typeName;
        }
    }
}
