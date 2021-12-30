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
using Moryx.Modules;
using Moryx.Products.Management.Implementations.Storage;
using Moryx.Products.Model;
using System;
using System.Collections.Generic;

namespace Moryx.Products.Management.Components
{
    /// <summary>
    /// Interface to easily access 
    /// </summary>
    public interface IProductLinkStrategy : IConfiguredPlugin<ProductLinkConfiguration>
    {
        /// <summary>
        /// Target type of this strategy
        /// </summary>
        Type TargetType { get; }

        /// <summary>
        /// Name of the parts property
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        /// Strategy how product instance parts are created during loading
        /// </summary>
        PartSourceStrategy PartCreation { get; }

        /// <summary>
        /// Load typed object and set on product
        /// </summary>
        void LoadPartLink(IGenericColumns linkEntity, IProductPartLink target);

        /// <summary>
        /// Save part link
        /// </summary>
        void SavePartLink(IProductPartLink source, IGenericColumns target);

        /// <summary>
        /// A link between two products was removed, remove the link as well
        /// </summary>
        void DeletePartLink(IReadOnlyList<IGenericColumns> deprecatedEntities);
    }
}
