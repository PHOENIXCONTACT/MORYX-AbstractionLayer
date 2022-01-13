// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

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
