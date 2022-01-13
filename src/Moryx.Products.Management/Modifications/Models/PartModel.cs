// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.Serialization;
using System.Runtime.Serialization;

namespace Moryx.Products.Management.Modification.Models
{
    [DataContract]
    internal class PartModel
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public Entry Properties { get; set; }

        [DataMember]
        public ProductModel Product { get; set; }
    }
}
