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
        /// <returns>A service model instance</returns>
        public static IProductServiceModel CreateServiceModel(IWcfClientFactory clientFactory)
        {
            return new ProductServiceModel(clientFactory);
        }
    }
}