// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0


/* Unmerged change from project 'Moryx.Products.Management (net5.0)'
Before:
using System.Threading.Tasks;
using Moryx.AbstractionLayer;
using Moryx.AbstractionLayer.Products;
using Moryx.Modules;
After:
using Moryx.AbstractionLayer;
using Moryx.AbstractionLayer.Products;
using Moryx.Modules;
using System.Threading.Modules;
*/

/* Unmerged change from project 'Moryx.Products.Management (netcoreapp3.1)'
Before:
using System.Threading.Tasks;
using Moryx.AbstractionLayer;
using Moryx.AbstractionLayer.Products;
using Moryx.Modules;
After:
using Moryx.AbstractionLayer;
using Moryx.AbstractionLayer.Products;
using Moryx.Modules;
using System.Threading.Modules;
*/
using Moryx.Modules;
using System.Threading.Tasks;

namespace Moryx.Products.Management.Components
{
    /// <summary>
    /// Interface for plugins that can import products from file
    /// </summary>
    public interface IProductImporter : IConfiguredPlugin<ProductImporterConfig>
    {
        /// <summary>
        /// Name of the importer
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Importer is long running and synchronous execution useless
        /// </summary>
        bool LongRunning { get; }

        /// <summary>
        /// Get the parameters of this importer
        /// </summary>
        object Parameters { get; }

        /// <summary>
        /// Update parameters based on partial input
        /// </summary>
        object Update(object currentParameters);

        /// <summary>
        /// Import products using given parameters
        /// </summary>
        Task<ProductImporterResult> Import(ProductImportContext context, object parameters);
    }
}
