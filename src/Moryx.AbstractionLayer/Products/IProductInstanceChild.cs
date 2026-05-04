using System;
using System.Collections.Generic;
using System.Text;

namespace Moryx.AbstractionLayer.Products
{
    /// <summary>
    /// A ProductInstance that is used as a sub instance.
    /// Allows to navigate backwards in the product instance tree.
    /// </summary>
    public interface IProductInstanceChild : IProductInstance
    {
        /// <summary>
        /// Id of the parent ProductInstance.
        /// </summary>
        public long ParentId { get; set; }
    }
}
