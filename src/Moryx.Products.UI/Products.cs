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
        /// <returns>A service model instance</returns>
        public static IProductServiceModel CreateServiceModel(string host, int port, IProxyConfig config)
        {
            return new ProductServiceModel(host, port, config);
        }
    }
}