// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.AbstractionLayer.Products;
using Moryx.AbstractionLayer.Products.Import;
using Moryx.Container;
using Moryx.Modules;
using Moryx.Products.Management.Components;
using Moryx.Products.Management.Implementations.Import;
using Moryx.Serialization;
using Moryx.Tools;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Moryx.Products.Management.Plugins.Importers
{
    /// <summary>
    /// Product importer which can create all types of products
    /// </summary>
    [ExpectedConfig(typeof(ProductImporterConfig))]
    [Plugin(LifeCycle.Transient, typeof(IProductImporter), Name = nameof(DefaultImporter))]
    public class DefaultImporter : ProductImporterBase<ProductImporterConfig, DefaultImporterParameters>
    {
        /// <inheritdoc />
        protected override Task<ProductImporterResult> Import(ProductImportContext context, DefaultImporterParameters parameters)
        {
            // TODO: Use type wrapper
            var type = ReflectionTool.GetPublicClasses<ProductType>(p => p.Name == parameters.ProductType)
                .First();

            var productType = (ProductType)Activator.CreateInstance(type);
            productType.Identity = new ProductIdentity(parameters.Identifier, parameters.Revision);
            productType.Name = parameters.Name;

            return Task.FromResult(new ProductImporterResult
            {
                ImportedTypes = new[] { productType }
            });
        }
    }

    /// <summary>
    /// Parameters for the default importer
    /// </summary>
    public class DefaultImporterParameters : PrototypeParameters
    {
        /// <summary>
        /// Product type to import
        /// </summary>
        [DisplayName("Product type"), Description("Type of product to import")]
        [Required, PossibleTypes(typeof(ProductType))]
        public string ProductType { get; set; }
    }
}
