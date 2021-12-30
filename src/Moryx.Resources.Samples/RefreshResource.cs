// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.AbstractionLayer.Resources;
using Moryx.Serialization;
using System;

namespace Moryx.Resources.Samples
{
    public class RefreshResource : Resource
    {
        [EntrySerialize]
        public string CurrentTime => DateTime.Now.ToLongTimeString();
    }
}