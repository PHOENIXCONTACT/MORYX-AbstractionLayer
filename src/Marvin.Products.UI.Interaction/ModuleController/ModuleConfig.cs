using System.Collections.Generic;
using System.Runtime.Serialization;
using Marvin.AbstractionLayer.UI.Aspects;
using Marvin.ClientFramework;
using Marvin.Products.UI.Interaction.Aspects;

namespace Marvin.Products.UI.Interaction
{
    /// <summary>
    /// Config file for the products module
    /// </summary>
    public class ModuleConfig : ClientModuleConfigBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleConfig"/> class.
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
                    PluginName = nameof(PartsAspectViewModel)
                },
                new AspectConfiguration
                {
                    PluginName = nameof(RecipesAspectViewModel)
                },
                new AspectConfiguration
                {
                    PluginName = nameof(RelationsAspectViewModel)
                }
            };

            AspectConfigurations = new List<TypedAspectConfiguration>
            {
                new TypedAspectConfiguration
                {
                    TypeName = "Sample",
                    Aspects = new List<AspectConfiguration>
                    {
                        new AspectConfiguration
                        {
                            PluginName = nameof(PropertiesAspectViewModel)
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Configured aspects
        /// </summary>
        [DataMember]
        public List<TypedAspectConfiguration> AspectConfigurations { get; set; }

        /// <summary>
        /// Default aspects as fallback for unconfigured types
        /// </summary>
        [DataMember]
        public List<AspectConfiguration> DefaultAspects { get; set; }
    }
}