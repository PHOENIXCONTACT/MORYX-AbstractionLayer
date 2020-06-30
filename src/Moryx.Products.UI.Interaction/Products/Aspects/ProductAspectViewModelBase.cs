// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.Threading.Tasks;
using Moryx.AbstractionLayer.UI;

namespace Moryx.Products.UI.Interaction.Aspects
{
    /// <summary>
    /// Abstract base class for product aspects
    /// </summary>
    public abstract class ProductAspectViewModelBase : EditModeViewModelBase, IProductAspect
    {
        /// <summary>
        /// <see cref="ProductViewModel"/> instance of the aspect
        /// </summary>
        public ProductViewModel Product { get; private set; }

        /// <inheritdoc />
        public virtual bool IsRelevant(ProductViewModel product) => true;

        /// <inheritdoc />
        public virtual Task Load(ProductViewModel product)
        {
            Product = product;
            return Task.FromResult(true);
        }

        /// <inheritdoc cref="IProductAspect" />
        public override Task Save() => Task.FromResult(true);
    }
}
