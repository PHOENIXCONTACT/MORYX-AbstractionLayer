using Marvin.Logging;
using Marvin.Tools.Wcf;

namespace Marvin.Products.UI
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
        /// <param name="logger">Logger</param>
        /// <returns>A service model instance</returns>
        public static IProductServiceModel CreateServiceModel(IWcfClientFactory clientFactory, IModuleLogger logger)
        {
            return new ProductServiceModel(clientFactory);
        }
    }
}