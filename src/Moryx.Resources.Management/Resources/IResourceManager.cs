// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.AbstractionLayer.Capabilities;
using Moryx.AbstractionLayer.Resources.Initializer;

/* Unmerged change from project 'Moryx.Resources.Management (net5.0)'
Before:
using Moryx.Modules;
After:
using Moryx.Modules;
using System;
using System.Collections.Generic;
*/

/* Unmerged change from project 'Moryx.Resources.Management (net45)'
Before:
using Moryx.Modules;
After:
using Moryx.Modules;
using System;
using System.Collections.Generic;
*/
using Moryx.Modules;
using System;

namespace Moryx.AbstractionLayer.Resources
{
    /// <summary>
    /// Major component managing the resource graph
    /// </summary>
    internal interface IResourceManager : IInitializablePlugin
    {
        /// <summary>
        /// Executes the intializer on this creator
        /// </summary>
        void ExecuteInitializer(IResourceInitializer initializer);

        /// <summary>
        /// Event raised when a resource was added at runtime
        /// </summary>
        event EventHandler<IPublicResource> ResourceAdded;

        /// <summary>
        /// Event raised when a resource was removed at runtime
        /// </summary>
        event EventHandler<IPublicResource> ResourceRemoved;

        /// <summary>
        /// Raised when the capabilities have changed.
        /// </summary>
        event EventHandler<ICapabilities> CapabilitiesChanged;
    }
}
