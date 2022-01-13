// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.Serialization;
using System.Runtime.Serialization;

namespace Moryx.Products.Management.Modification.Models
{
    [DataContract]
    internal class ProductImporter
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public Entry Parameters { get; set; }
    }
}
