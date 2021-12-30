// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.AbstractionLayer.Products;

namespace Moryx.Products.Management.Components
{
    /// <summary>
    /// Return value for <see cref="IProductImporter"/>
    /// </summary>
    public class ProductImporterResult : ProductImportResult
    {
        /// <summary>
        /// Flag if all objects were saved
        /// </summary>
        public bool Saved { get; set; }
    }
}