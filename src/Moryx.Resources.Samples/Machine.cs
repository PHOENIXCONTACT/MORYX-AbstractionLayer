// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

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
