// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0


/* Unmerged change from project 'Moryx.Products.Management (net5.0)'
Before:
using System;
using System.Linq.Expressions;
using Moryx.AbstractionLayer;
After:
using Moryx.AbstractionLayer;
*/

/* Unmerged change from project 'Moryx.Products.Management (netcoreapp3.1)'
Before:
using System;
using System.Linq.Expressions;
using Moryx.AbstractionLayer;
After:
using Moryx.AbstractionLayer;
*/
using Moryx.AbstractionLayer.Products;
using Moryx.Modules;
using Moryx.Products.Management.Implementations.Storage;
using Moryx.Products.Model;
using System;
using System.Linq.Expressions;

namespace Moryx.Products.Management.Components
{
    /// <summary>
    /// Strategy methods for a certain product type
    /// </summary>
    public interface IProductTypeStrategy : IConfiguredPlugin<ProductTypeConfiguration>
    {
        /// <summary>
        /// Target type of this strategy
        /// </summary>
        Type TargetType { get; }

        /// <summary>
        /// Detect changes between business object and current state of the database
        /// </summary>
        bool HasChanged(IProductType current, IGenericColumns dbProperties);

        /// <summary>
        /// Write product properties to database generic columns
        /// </summary>
        void SaveType(IProductType source, IGenericColumns target);

        /// <summary>
        /// Load product from database information
        /// </summary>
        void LoadType(IGenericColumns source, IProductType target);
    }

    /// <summary>
    /// Additional interface for type strategies to search for product types by expression
    /// TODO: Remove in AL 6
    /// </summary>
    public interface IProductTypeSearch : IProductTypeStrategy
    {
        /// <summary>
        /// Transform a product class selector to a database compatible expression
        /// </summary>
        Expression<Func<IGenericColumns, bool>> TransformSelector<TProduct>(Expression<Func<TProduct, bool>> selector);
    }
}
