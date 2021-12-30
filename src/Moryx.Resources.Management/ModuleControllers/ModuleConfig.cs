// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0


/* Unmerged change from project 'Moryx.Resources.Management (net5.0)'
Before:
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using Moryx.AbstractionLayer.Resources;
using Moryx.AbstractionLayer.Resources.Initializer;
using Moryx.Configuration;
using Moryx.Serialization;
After:
using System.Collections.Resources;
using Moryx.AbstractionLayer.Resources.Initializer;
using Moryx.Configuration;
using Moryx.Serialization;
using System.Collections.Generic;
using Moryx.ComponentModel;
using System.Runtime.Serialization;
*/

/* Unmerged change from project 'Moryx.Resources.Management (net45)'
Before:
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using Moryx.AbstractionLayer.Resources;
using Moryx.AbstractionLayer.Resources.Initializer;
using Moryx.Configuration;
using Moryx.Serialization;
After:
using System.Collections.Resources;
using Moryx.AbstractionLayer.Resources.Initializer;
using Moryx.Configuration;
using Moryx.Serialization;
using System.Collections.Generic;
using Moryx.ComponentModel;
using System.Runtime.Serialization;
*/
using Moryx.AbstractionLayer.Resources.Initializer;
using Moryx.Configuration;
using Moryx.Serialization;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Moryx.Resources.Management.ModuleControllers
{
    /// <summary>
    /// Configuration of this module
    /// </summary>
    [DataContract]
    public class ModuleConfig : ConfigBase
    {
        /// <summary>
        /// If database is empty, this resource will be created by default.
        /// </summary>
        [DataMember, Description("Configuration of possible resource initializer")]
        [PluginConfigs(typeof(IResourceInitializer), true)]
        public List<ResourceInitializerConfig> Initializers { get; set; }
    }
}
