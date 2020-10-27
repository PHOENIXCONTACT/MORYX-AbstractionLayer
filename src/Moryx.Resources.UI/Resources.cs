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
        /// <param name="clientFactory">ClientFactory to initialize connections</param>
        /// <returns>A service model instance</returns>
        public static IResourceServiceModel CreateServiceModel(IWcfClientFactory clientFactory)
        {
            return new ResourceServiceModel(clientFactory);
        }
    }
}
