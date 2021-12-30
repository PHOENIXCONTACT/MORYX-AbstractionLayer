// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0


/* Unmerged change from project 'Moryx.Resources.Samples (netstandard2.0)'
Before:
using System.ComponentModel;
using System.Runtime.Serialization;
using Moryx.AbstractionLayer.Resources;
using Moryx.AbstractionLayer.Resources.Attributes;
After:
using System.AbstractionLayer.Resources;
using Moryx.AbstractionLayer.Resources.Attributes;
using Moryx.Serialization;
using System.ComponentModel;
*/

/* Unmerged change from project 'Moryx.Resources.Samples (net5.0)'
Before:
using System.ComponentModel;
using System.Runtime.Serialization;
using Moryx.AbstractionLayer.Resources;
using Moryx.AbstractionLayer.Resources.Attributes;
After:
using System.AbstractionLayer.Resources;
using Moryx.AbstractionLayer.Resources.Attributes;
using Moryx.Serialization;
using System.ComponentModel;
*/
using Moryx.AbstractionLayer.Resources;
using Moryx.AbstractionLayer.Resources.Attributes;
using Moryx.Serialization;
using System.Runtime.Serialization;

namespace Moryx.Resources.Samples
{
    [ResourceRegistration]
    public class Machine : Resource
    {
        [DataMember, EntrySerialize]
        public string CurrentState { get; set; }
    }
}
