// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.Collections.Generic;

namespace Moryx.AbstractionLayer.UI.Aspects
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
