using Moryx.Communication;
using Moryx.Logging;
using Moryx.Tools.Wcf;

namespace Moryx.Resources.UI
{
    /// <summary>
    /// Static helper class to create resource interaction
    /// </summary>
    public static class Resources
    {
        /// <summary>
        /// Creates a service model instance
        /// </summary>
        /// <returns>A service model instance</returns>
        public static IResourceServiceModel CreateServiceModel(string host, int port, IProxyConfig config)
        {
            return new ResourceServiceModel(host, port, config);
        }
    }
}
