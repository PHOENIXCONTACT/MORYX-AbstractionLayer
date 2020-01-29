using System.Collections.Generic;

namespace Marvin.AbstractionLayer.UI.Aspects
{
    /// <summary>
    /// Factory to create the aspect configurator
    /// </summary>
    public interface IAspectConfiguratorFactory
    {
        /// <summary>
        /// Creates a new instance of the aspect configurator
        /// </summary>
        IAspectConfigurator Create(ICollection<TypedAspectConfiguration> current, string[] types);

        /// <summary>
        /// Destroys the aspect configurator instance
        /// </summary>
        void Destroy(IAspectConfigurator configurator);
    }
}