// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.Configuration;
using Moryx.Modules;
using Moryx.Serialization;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Moryx.AbstractionLayer.Resources.Initializer
{
    /// <summary>
    /// Configuration base for <see cref="IResourceInitializer"/>
    /// </summary>
    [DataContract]
    public class ResourceInitializerConfig : UpdatableEntry, IPluginConfig
    {
        /// <inheritdoc />
        [DataMember, Description("Name of the resource initializer")]
        [PluginNameSelector(typeof(IResourceInitializer))]
        [DisplayName("Plugin Name")]
        public virtual string PluginName { get; set; }

        /// <summary>
        /// Overrides <see cref="object.ToString"/> for the plugin name
        /// </summary>
        public override string ToString()
        {
            return PluginName;
        }
    }
}
