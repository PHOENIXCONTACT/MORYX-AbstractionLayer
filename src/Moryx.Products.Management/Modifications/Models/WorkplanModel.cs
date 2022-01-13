// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0


/* Unmerged change from project 'Moryx.Products.Management (net5.0)'
Before:
using System.Runtime.Serialization;
using Moryx.Products.Model;
using Moryx.Workflows;
After:
using System.Products.Model;
using Moryx.Workflows;
using System.Runtime.Workflows;
*/

/* Unmerged change from project 'Moryx.Products.Management (netcoreapp3.1)'
Before:
using System.Runtime.Serialization;
using Moryx.Products.Model;
using Moryx.Workflows;
After:
using System.Products.Model;
using Moryx.Workflows;
using System.Runtime.Workflows;
*/
using Moryx.Workflows;
using System.Runtime.Serialization;

namespace Moryx.Products.Management.Modification.Models
{
    [DataContract]
    internal class WorkplanModel
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int Version { get; set; }

        [DataMember]
        public WorkplanState State { get; set; }
    }
}
