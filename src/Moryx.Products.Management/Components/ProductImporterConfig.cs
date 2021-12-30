// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.Modules;
using Moryx.Serialization;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Moryx.Products.Management.Components
{
    /// <summary>
    /// Config for product importers
    /// </summary>
    [DataContract]
    public class ProductImporterConfig : IPluginConfig
    {
        /// <summary>
        /// Name of the component represented by this entry
        /// </summary>
        [DataMember, Description("PluginName of the importer")]
        [PluginNameSelector(typeof(IProductImporter))]
        public virtual string PluginName { get; set; }
    }
}