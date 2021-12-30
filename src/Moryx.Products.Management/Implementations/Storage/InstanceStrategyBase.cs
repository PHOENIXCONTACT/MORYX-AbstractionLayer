// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.AbstractionLayer.Products;
using Moryx.Products.Management.Components;
using Moryx.Products.Model;
using Moryx.Tools;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Moryx.Products.Management.Implementations.Storage
{
    /// <summary>
    /// Non-generic base class for <see cref="IProductInstanceStrategy"/>
    /// </summary>
    public abstract class InstanceStrategyBase : InstanceStrategyBase<ProductInstanceConfiguration>
    {
        /// <summary>
        /// Empty constructor for derived types
        /// </summary>
        protected InstanceStrategyBase()
        {
        }

        /// <summary>
        /// Create a new instance of the simple strategy
        /// </summary>
        protected InstanceStrategyBase(bool skipArticles) : base(skipArticles)
        {
        }
    }

    /// <summary>
    /// Base class for all <see cref="IProductInstanceStrategy"/>
    /// </summary>
    /// <typeparam name="TConfig"></typeparam>
    public abstract class InstanceStrategyBase<TConfig> : StrategyBase<TConfig, ProductInstanceConfiguration>, IProductInstanceStrategy
        where TConfig : ProductInstanceConfiguration
    {
        /// <inheritdoc />
        public bool SkipInstances { get; protected set; }

        /// <summary>
        /// Empty constructor for derived types
        /// </summary>
        protected InstanceStrategyBase()
        {
        }

        /// <summary>
        /// Create a new instance of the simple strategy
        /// </summary>
        protected InstanceStrategyBase(bool skipArticles) : this()
        {
            SkipInstances = skipArticles;
        }

        /// <inheritdoc />
        public override void Initialize(ProductInstanceConfiguration config)
        {
            base.Initialize(config);

            TargetType = ReflectionTool.GetPublicClasses<ProductInstance>(p => p.Name == config.TargetType).FirstOrDefault();
        }

        /// <inheritdoc />
        public abstract Expression<Func<IGenericColumns, bool>> TransformSelector<TInstance>(Expression<Func<TInstance, bool>> selector);

        /// <inheritdoc />
        public abstract void SaveInstance(ProductInstance source, IGenericColumns target);

        /// <inheritdoc />
        public abstract void LoadInstance(IGenericColumns source, ProductInstance target);
    }
}
