// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.AbstractionLayer.Products;
using Moryx.AbstractionLayer.Recipes;
using Moryx.Modules;
using Moryx.Products.Management.Components;
using Moryx.Serialization;
using System.Runtime.Serialization;

namespace Moryx.Products.Management.Implementations.Storage
{
    /// <summary>
    /// Common configuration interface for all strategy configs
    /// </summary>
    public interface IProductStrategyConfiguation : IPluginConfig //TODO: Rename to IProductStrategyConfiguration in the next major
    {
        /// <summary>
        /// Target type of the strategy
        /// </summary>
        string TargetType { get; set; }

        /// <summary>
        /// Changeable plugin name of the strategy
        /// </summary>
        new string PluginName { get; set; }
    }

    [DataContract]
    public class ProductTypeConfiguration : IProductStrategyConfiguation
    {
        /// <inheritdoc />
        [DataMember, PossibleTypes(typeof(ProductType))]
        public string TargetType { get; set; }

        /// <inheritdoc cref="IPluginConfig"/> />
        [DataMember, PluginNameSelector(typeof(IProductTypeStrategy))]
        public virtual string PluginName { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{TargetType} => {PluginName}";
        }
    }

    [DataContract]
    public class ProductInstanceConfiguration : IProductStrategyConfiguation
    {
        /// <inheritdoc />
        [DataMember, PossibleTypes(typeof(ProductInstance))]
        public string TargetType { get; set; }

        /// <inheritdoc cref="IPluginConfig"/> />
        [DataMember, PluginNameSelector(typeof(IProductInstanceStrategy))]
        public virtual string PluginName { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{TargetType} => {PluginName}";
        }
    }

    [DataContract]
    public class ProductLinkConfiguration : IProductStrategyConfiguation
    {
        /// <inheritdoc />
        [DataMember, PossibleTypes(typeof(ProductType))]
        public string TargetType { get; set; }

        /// <summary>
        /// Name of the part property
        /// </summary>
        [DataMember]
        public string PartName { get; set; }

        /// <inheritdoc cref="IPluginConfig"/> />
        [DataMember, PluginNameSelector(typeof(IProductLinkStrategy))]
        public virtual string PluginName { get; set; }

        /// <summary>
        /// Strategy how the part is created
        /// </summary>
        [DataMember]
        public PartSourceStrategy PartCreation { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{TargetType}.{PartName} => {PluginName}";
        }
    }

    [DataContract]
    public class ProductRecipeConfiguration : IProductStrategyConfiguation
    {
        /// <inheritdoc />
        [DataMember, PossibleTypes(typeof(IProductRecipe))]
        public string TargetType { get; set; }

        /// <inheritdoc cref="IPluginConfig"/> />
        [DataMember, PluginNameSelector(typeof(IProductRecipeStrategy))]
        public virtual string PluginName { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{TargetType} => {PluginName}";
        }
    }
}
