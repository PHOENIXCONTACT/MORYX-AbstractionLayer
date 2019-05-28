using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Marvin.Products.UI.ProductService;
using Marvin.Serialization;

namespace Marvin.Products.UI.Interaction
{
    /// <summary>
    /// View model that represents a single part connector
    /// </summary>
    public class PartViewModel : PropertyChangedBase
    {
        private readonly PartModel _part;

        internal PartViewModel(PartModel part)
            : this(part, new ProductViewModel(part.Product))
        {
        }

        internal PartViewModel(PartModel part, ProductViewModel product)
        {
            _part = part;
            Product = product;
        }

        /// <summary>
        /// Product referenced by the part
        /// </summary>
        public ProductViewModel Product { get; }

        /// <summary>
        /// Properties of the link
        /// </summary>
        public Entry Properties => _part.Properties;
    }
}