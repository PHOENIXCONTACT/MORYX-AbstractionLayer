// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.Runtime.Serialization;
using Moryx.Modules;

namespace Moryx.AbstractionLayer.UI.Aspects
{
    /// <summary>
    /// Aspect configuration
    /// </summary>
    [DataContract]
    public class AspectConfiguration : IPluginConfig
    {
        /// <summary>
        /// Aspect full type name
        /// </summary>
        [DataMember]
        public string PluginName { get; set; }
    }
}
