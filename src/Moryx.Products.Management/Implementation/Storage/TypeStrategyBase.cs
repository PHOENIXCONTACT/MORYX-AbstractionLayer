// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.Linq;
using Moryx.AbstractionLayer.Products;
using Moryx.Products.Model;
using Moryx.Tools;

namespace Moryx.Products.Management
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
        /// <inheritdoc />
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
