// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.Runtime.Serialization;

namespace Moryx.Products.Management.Modification
{
    [DataContract]
    internal class ProductCustomization
    {
        [DataMember]
        public ProductDefinitionModel[] ProductTypes { get; set; }

        [DataMember]
        public RecipeDefinitionModel[] RecipeTypes { get; set; }

        [DataMember]
        public ProductImporter[] Importers { get; set; }
    }
}
