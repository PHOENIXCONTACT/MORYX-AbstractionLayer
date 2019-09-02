using System.Runtime.Serialization;
using Marvin.Modules;

namespace Marvin.AbstractionLayer.UI.Aspects
{
    /// <summary>
    /// Aspect configuration
    /// </summary>
    [DataContract]
    public class AspectConfiguration : IPluginConfig
    {
        /// <summary>
        /// Aspect full type name
        /// </summary>
        [DataMember]
        public string PluginName { get; set; }
    }
}