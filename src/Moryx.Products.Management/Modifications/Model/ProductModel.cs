// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.AbstractionLayer.Products;
using Moryx.Serialization;
using System.Runtime.Serialization;

namespace Moryx.Products.Management.Modification
{
    [DataContract]
    internal class ProductModel
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public ProductState State { get; set; }

        [DataMember]
        public string Identifier { get; set; }

        [DataMember]
        public short Revision { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public Entry Properties { get; set; }

        [DataMember]
        public ProductFile[] Files { get; set; }

        [DataMember]
        public PartConnector[] Parts { get; set; }

        [DataMember]
        public RecipeModel[] Recipes { get; set; }
    }
}
