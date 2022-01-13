// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0


/* Unmerged change from project 'Moryx.Products.Management (net5.0)'
Before:
using System;
using System.ComponentModel;
After:
using Moryx.Configuration;
using Moryx.ComponentModel;
*/

/* Unmerged change from project 'Moryx.Products.Management (netcoreapp3.1)'
Before:
using System;
using System.ComponentModel;
After:
using Moryx.Configuration;
using Moryx.ComponentModel;
*/
using Moryx.Modules;
using Moryx.Serialization;
using System.Runtime.Serialization;

/* Unmerged change from project 'Moryx.Products.Management (net5.0)'
Before:
using System.Text.RegularExpressions;
using Moryx.Configuration;
using Moryx.Modules;
using Moryx.Serialization;
After:
using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Serialization;
*/

/* Unmerged change from project 'Moryx.Products.Management (netcoreapp3.1)'
Before:
using System.Text.RegularExpressions;
using Moryx.Configuration;
using Moryx.Modules;
using Moryx.Serialization;
After:
using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Serialization;
*/

namespace Moryx.Products.Management.Plugins.GenericStrategies
{
    /// <summary>
    /// Configuration how a single property should be stored
    /// </summary>
    [DataContract]
    public class PropertyMapperConfig : IPluginConfig
    {
        /// <summary>
        /// Name of the property on the product
        /// </summary>
        [DataMember]
        public string PropertyName { get; set; }

        /// <summary>
        /// Name of plugin responsible for the property
        /// </summary>
        [DataMember, PluginNameSelector(typeof(IPropertyMapper))]
        public virtual string PluginName { get; set; }

        /// <summary>
        /// Column where the value shall be stored
        /// </summary>
        [DataMember, AvailableColumns]
        public string Column { get; set; }

        public override string ToString()
        {
            return $"{PropertyName}=>{Column}({PluginName})";
        }
    }
}
