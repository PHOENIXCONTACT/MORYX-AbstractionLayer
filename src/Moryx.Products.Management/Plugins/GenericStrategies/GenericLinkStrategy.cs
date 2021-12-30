// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0


/* Unmerged change from project 'Moryx.Products.Management (net5.0)'
Before:
using System;
using System.Collections.Generic;
using Moryx.AbstractionLayer;
After:
using Moryx.AbstractionLayer;
*/

/* Unmerged change from project 'Moryx.Products.Management (netcoreapp3.1)'
Before:
using System;
using System.Collections.Generic;
using Moryx.AbstractionLayer;
After:
using Moryx.AbstractionLayer;
*/
using Moryx.AbstractionLayer.Products;
using Moryx.Container;
using Moryx.Modules;
using Moryx.Products.Management.Components;
using Moryx.Products.Management.Implementations.Storage;
using Moryx.Products.Model;
/* Unmerged change from project 'Moryx.Products.Management (net5.0)'
Before:
using System;
After:
using System.Collections.Generic;
*/

/* Unmerged change from project 'Moryx.Products.Management (netcoreapp3.1)'
Before:
using System;
After:
using System.Collections.Generic;
*/


namespace Moryx.Products.Management.Plugins.GenericStrategies
{
    /// <summary>
    /// 
    /// </summary>
    [ExpectedConfig(typeof(GenericLinkConfiguration))]
    [StrategyConfiguration(typeof(IProductPartLink), DerivedTypes = true)]
    [Plugin(LifeCycle.Transient, typeof(IProductLinkStrategy), Name = nameof(GenericLinkStrategy))]
    internal class GenericLinkStrategy : LinkStrategyBase<GenericLinkConfiguration>
    {
        /// <summary>
        /// Injected entity mapper
        /// </summary>
        public GenericEntityMapper<ProductPartLink, ProductType> EntityMapper { get; set; }

        /// <summary>
        /// Initialize the type strategy
        /// </summary>
        public override void Initialize(ProductLinkConfiguration config)
        {
            base.Initialize(config);

            var property = TargetType.GetProperty(PropertyName);
            var linkType = property.PropertyType;
            // Extract element type from collections
            if (typeof(IEnumerable<IProductPartLink>).IsAssignableFrom(linkType))
            {
                linkType = linkType.GetGenericArguments()[0];
            }

            EntityMapper.Initialize(linkType, Config);
        }

        public override void LoadPartLink(IGenericColumns linkEntity, IProductPartLink target)
        {
            EntityMapper.ReadValue(linkEntity, target);
        }

        public override void SavePartLink(IProductPartLink source, IGenericColumns target)
        {
            EntityMapper.WriteValue(source, target);
        }
    }
}
