// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0


/* Unmerged change from project 'Moryx.Products.Samples (netcoreapp3.1)'
Before:
using System.IO;
using System.Threading.Tasks;
using Moryx.AbstractionLayer;
After:
using Moryx.AbstractionLayer;
*/

/* Unmerged change from project 'Moryx.Products.Samples (net5.0)'
Before:
using System.IO;
using System.Threading.Tasks;
using Moryx.AbstractionLayer;
After:
using Moryx.AbstractionLayer;
*/
using Moryx.AbstractionLayer.Products;
using Moryx.AbstractionLayer.Products.Import;
using Moryx.Container;
using Moryx.Modules;
using Moryx.Products.Management.Components;
using Moryx.Products.Management.Implementations.Import;
using System.IO;
using System.Threading.Tasks;
/* Unmerged change from project 'Moryx.Products.Samples (netcoreapp3.1)'
Before:
using System.IO;
After:
using System.Threading.Tasks;
*/

/* Unmerged change from project 'Moryx.Products.Samples (net5.0)'
Before:
using System.IO;
After:
using System.Threading.Tasks;
*/


namespace Moryx.Products.Samples
{
    [ExpectedConfig(typeof(FileImporterConfig))]
    [Plugin(LifeCycle.Singleton, typeof(IProductImporter), Name = nameof(FileImporter))]
    public class FileImporter : ProductImporterBase<FileImporterConfig, FileImportParameters>
    {
        /// <summary>
        /// Method to generate an instance of the parameter array
        /// </summary>
        /// <returns></returns>
        protected override object GenerateParameters()
        {
            return new FileImportParameters { FileExtension = ".mjb" };
        }

        /// <summary>
        /// Import a product using given parameters
        /// </summary>
        protected override Task<ProductImporterResult> Import(ProductImportContext context, FileImportParameters parameters)
        {
            using (var stream = parameters.ReadFile())
            {
                var textReader = new StreamReader(stream);
                var identifier = textReader.ReadLine();
                var revision = short.Parse(textReader.ReadLine() ?? "0");
                var name = textReader.ReadLine();

                return Task.FromResult(new ProductImporterResult
                {
                    ImportedTypes = new ProductType[]
                    {
                        new NeedleType
                        {
                            Name = name,
                            Identity = new ProductIdentity(identifier, revision)
                        }
                    }
                });
            }
        }
    }
}
