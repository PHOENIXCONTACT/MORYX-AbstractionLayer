// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0


/* Unmerged change from project 'Moryx.Products.Management (net5.0)'
Before:
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection.Emit;
using Moryx.AbstractionLayer;
After:
using Moryx.AbstractionLayer;
*/

/* Unmerged change from project 'Moryx.Products.Management (netcoreapp3.1)'
Before:
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection.Emit;
using Moryx.AbstractionLayer;
After:
using Moryx.AbstractionLayer;
*/
using Moryx.AbstractionLayer.Products;
using Moryx.Container;
using Moryx.Products.Management.Components;
using Moryx.Products.Management.Implementations.Storage;
using Moryx.Products.Model;
/* Unmerged change from project 'Moryx.Products.Management (net5.0)'
Before:
using Moryx.Tools;
After:
using Moryx.Tools;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection.Emit;
*/

/* Unmerged change from project 'Moryx.Products.Management (netcoreapp3.1)'
Before:
using Moryx.Tools;
After:
using Moryx.Tools;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection.Emit;
*/


namespace Moryx.Products.Management.Plugins.NullStrategies
{
    /// <summary>
    /// Simple link strategy without any properties
    /// </summary>
    [PropertylessStrategyConfiguration(typeof(IProductPartLink), DerivedTypes = true)]
    [Plugin(LifeCycle.Transient, typeof(IProductLinkStrategy), Name = nameof(SimpleLinkStrategy))]
    internal class SimpleLinkStrategy : LinkStrategyBase
    {
        public override void LoadPartLink(IGenericColumns linkEntity, IProductPartLink target)
        {
            // We have no custom properties
        }

        public override void SavePartLink(IProductPartLink source, IGenericColumns target)
        {
            // We have no custom properties
        }
    }
}
