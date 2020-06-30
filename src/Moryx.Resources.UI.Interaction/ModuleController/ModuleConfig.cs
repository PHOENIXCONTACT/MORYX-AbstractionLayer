// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.Collections.Generic;
using System.Runtime.Serialization;
using Moryx.AbstractionLayer.UI.Aspects;
using Moryx.ClientFramework;
using Moryx.Resources.UI.Interaction.Aspects;
using Moryx.Resources.UI.Interaction.Aspects.Methods;

namespace Moryx.Resources.UI.Interaction
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
