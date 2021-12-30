// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.Serialization;
using System.Runtime.Serialization;

namespace Moryx.Products.Management.Modification
{
    [DataContract]
    internal class ProductDefinitionModel
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string DisplayName { get; set; }

        [DataMember]
        public string BaseDefinition { get; set; }

        [DataMember]
        public Entry Properties { get; set; }
    }
}
