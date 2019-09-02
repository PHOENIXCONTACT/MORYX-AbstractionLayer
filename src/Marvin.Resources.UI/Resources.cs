using Marvin.Logging;
using Marvin.Tools.Wcf;

namespace Marvin.Resources.UI
{
    /// <summary>
    /// Static helper class to create resource interaction
    /// </summary>
    public static class Resources
    {
        /// <summary>
        /// Creates a service model instance
        /// </summary>
        /// <param name="clientFactory">ClientFactory to initialize connections</param>
        /// <param name="logger">Logger</param>
        /// <returns>A service model instance</returns>
        public static IResourceServiceModel CreateServiceModel(IWcfClientFactory clientFactory, IModuleLogger logger)
        {
            return new ResourceServiceModel(clientFactory);
        }
    }
}
