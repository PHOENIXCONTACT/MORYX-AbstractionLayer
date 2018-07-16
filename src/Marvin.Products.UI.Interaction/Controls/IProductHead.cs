using System;
using System.Collections.Generic;
using System.Linq;
namespace Marvin.Products.UI.Interaction
{
    /// <summary>
    /// Interface representing the resource head information
    /// </summary>
    public interface IProductHead
    {
        /// <summary>
        /// The name of the Product.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// The current State of the product.
        /// </summary>
        string State { get; }

        /// <summary>
        /// The Type of the product.
        /// </summary>
        string Type { get; }

        /// <summary>
        /// The identifier of the product.
        /// </summary>
        string Identifier { get; }

        /// <summary>
        /// The revision of the product.
        /// </summary>
        short Revision { get; }

    }
}
