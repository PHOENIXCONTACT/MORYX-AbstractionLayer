// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.Serialization;
using System.Runtime.Serialization;

namespace Moryx.Products.Management.Modification.Models
{
    [DataContract]
    internal class PartConnector
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string DisplayName { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public bool IsCollection { get; set; }

        [DataMember]
        public PartModel[] Parts { get; set; }

        [DataMember]
        public Entry PropertyTemplates { get; set; }
    }
}
