﻿using System.Collections.Generic;
using System.Runtime.Serialization;
using Marvin.AbstractionLayer.UI.Aspects;
using Marvin.ClientFramework;
using Marvin.Resources.UI.Interaction.Aspects;
using Marvin.Resources.UI.Interaction.Aspects.Methods;

namespace Marvin.Resources.UI.Interaction
{
    /// <summary>
    /// Module configuration for the <see cref="ModuleController"/>
    /// </summary>
    public class ModuleConfig : ClientModuleConfigBase
    {
        /// <summary>
        /// Constructor to create a new instance of the <see cref="ModuleConfig"/>
        /// </summary>
        public ModuleConfig()
        {
            DefaultAspects = new List<AspectConfiguration>
            {
                new AspectConfiguration
                {
                    PluginName = nameof(PropertiesAspectViewModel)
                },
                new AspectConfiguration
                {
                    PluginName = nameof(ReferencesAspectViewModel)
                },
                new AspectConfiguration
                {
                    PluginName = nameof(ResourceMethodsAspectViewModel)
                }
            };
        }

        /// <summary>
        /// Configured aspects
        /// </summary>
        [DataMember]
        public List<TypedAspectConfiguration> AspectConfigurations { get; set; }

        /// <summary>
        /// Default list of aspects
        /// </summary>
        [DataMember]
        public List<AspectConfiguration> DefaultAspects { get; set; }
    }
}