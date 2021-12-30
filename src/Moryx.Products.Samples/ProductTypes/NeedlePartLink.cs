// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.AbstractionLayer.Products;
using System.ComponentModel;

namespace Moryx.Products.Samples
{
    public class NeedlePartLink : ProductPartLink<NeedleType>
    {
        public NeedlePartLink()
        {
        }

        public NeedlePartLink(long id) : base(id)
        {
        }

        [Description("Which role does the needle have")]
        public NeedleRole Role { get; set; }

        public override ProductInstance Instantiate()
        {
            var needle = (NeedleInstance)base.Instantiate();
            needle.Role = Role;
            return needle;
        }
    }
}
