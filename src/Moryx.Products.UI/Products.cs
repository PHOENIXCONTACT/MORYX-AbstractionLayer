using System;
using Moryx.Communication;
using Moryx.Logging;
using Moryx.Tools.Wcf;

namespace Moryx.Products.UI
{
    /// <summary>
    /// Static helper class to create product interaction
    /// </summary>
    public static class Products
    {
        /// <summary>
        /// Creates a service model instance
        /// </summary>
        /// <param name="clientFactory">ClientFactory to initialize connections</param>
        /// <param name="logger">Logger for the service model</param>
        /// <returns>A service model instance</returns>
        [Obsolete("Instantiate ProductServiceModel directly")]
        public static IProductServiceModel CreateServiceModel(IWcfClientFactory clientFactory, IModuleLogger logger)
        {
            return new ProductServiceModel(clientFactory, logger);
        }
    }
}