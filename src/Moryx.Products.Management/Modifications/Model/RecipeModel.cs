// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0


/* Unmerged change from project 'Moryx.Products.Management (net5.0)'
Before:
using System;
using System.Runtime.Serialization;
After:
using Moryx.AbstractionLayer;
using Moryx.Runtime.Serialization;
*/

/* Unmerged change from project 'Moryx.Products.Management (netcoreapp3.1)'
Before:
using System;
using System.Runtime.Serialization;
After:
using Moryx.AbstractionLayer;
using Moryx.Runtime.Serialization;
*/
using Moryx.AbstractionLayer.Recipes;
using Moryx.Serialization;

/* Unmerged change from project 'Moryx.Products.Management (net5.0)'
Before:
using Moryx.AbstractionLayer.Recipes;
using Moryx.Serialization;
After:
using System;
using System.Runtime.Serialization;
*/

/* Unmerged change from project 'Moryx.Products.Management (netcoreapp3.1)'
Before:
using Moryx.AbstractionLayer.Recipes;
using Moryx.Serialization;
After:
using System;
using System.Runtime.Serialization;
*/
using System.Runtime.Serialization;

namespace Moryx.Products.Management.Modification
{
    /// <summary>
    /// DTO representation of a recipe
    /// </summary>
    [DataContract]
    internal class RecipeModel
    {
        /// <summary>
        /// Id of the recipe
        /// </summary>
        [DataMember]
        public long Id { get; set; }

        /// <summary>
        /// Name of the recipe
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// This recipe's state
        /// </summary>
        [DataMember]
        public RecipeState State { get; set; }

        /// <summary>
        /// Revision of the recipe
        /// </summary>
        [DataMember]
        public int Revision { get; set; }

        /// <summary>
        /// Type of the recipe
        /// </summary>
        [DataMember]
        public string Type { get; set; }

        /// <summary>
        /// Properties of the recipe
        /// </summary>
        [DataMember]
        public Entry Properties { get; set; }

        /// <summary>
        /// The id of the current referenced workplan
        /// </summary>
        [DataMember]
        public long WorkplanId { get; set; }

        /// <summary>
        /// Classification of the recipe
        /// </summary>
        [DataMember]
        public RecipeClassificationModel Classification { get; set; }
    }
}
