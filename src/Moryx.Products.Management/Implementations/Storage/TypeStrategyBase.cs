// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.AbstractionLayer.Products;
using Moryx.Products.Management.Components;
using Moryx.Products.Model;

/* Unmerged change from project 'Moryx.Products.Management (net5.0)'
Before:
using Moryx.Tools;
After:
using Moryx.Tools;
using System;
using System.Linq;
using System.Linq.Expressions;
*/

/* Unmerged change from project 'Moryx.Products.Management (netcoreapp3.1)'
Before:
using Moryx.Tools;
After:
using Moryx.Tools;
using System;
using System.Linq;
using System.Linq.Expressions;
*/
using Moryx.Tools;
using System.Linq;

namespace Moryx.Products.Management.Implementations.Storage
{
    /// <summary>
    /// Base class for product strategies
    /// </summary>
    public abstract class TypeStrategyBase : TypeStrategyBase<ProductTypeConfiguration>
    {
    }

    /// <summary>
    /// Base class for product strategies
    /// </summary>
    public abstract class TypeStrategyBase<TConfig> : StrategyBase<TConfig, ProductTypeConfiguration>, IProductTypeStrategy
        where TConfig : ProductTypeConfiguration
    {
        public override void Initialize(ProductTypeConfiguration config)
        {
            base.Initialize(config);

            TargetType = ReflectionTool.GetPublicClasses<ProductType>(p => p.Name == config.TargetType).FirstOrDefault();
        }

        /// <inheritdoc />
        public abstract bool HasChanged(IProductType current, IGenericColumns dbProperties);

        /// <inheritdoc />
        public abstract void LoadType(IGenericColumns source, IProductType target);

        /// <inheritdoc />
        public abstract void SaveType(IProductType source, IGenericColumns target);
    }
}
