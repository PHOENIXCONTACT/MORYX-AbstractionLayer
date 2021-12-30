// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.AbstractionLayer.Identity;
using Moryx.AbstractionLayer.Products;
using System;

namespace Moryx.Products.Samples
{
    public class WatchfaceInstance : ProductInstance<WatchfaceTypeBase>, IIdentifiableObject
    {
        public Guid Identifier { get; set; }

        public IIdentity Identity { get; set; }

        public WatchfaceInstance()
        {
            Identifier = Guid.NewGuid();
        }

    }
}
