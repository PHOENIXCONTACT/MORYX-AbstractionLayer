// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.AbstractionLayer.Products;
using System.Collections.Generic;
using System.ComponentModel;

namespace Moryx.Products.Samples
{
    /// <summary>
    /// Product that represents the package used to ship a watch
    /// </summary>
    [DisplayName("Packaging")]
    public class WatchPackageType : ProductType
    {
        /// <summary>
        /// Watches that can be shipped in this package
        /// </summary>
        [DisplayName("Possible Watches")]
        public List<ProductPartLink<WatchType>> PossibleWatches { get; set; }

        protected override ProductInstance Instantiate()
        {
            return new WatchPackageInstance();
        }
    }
}
