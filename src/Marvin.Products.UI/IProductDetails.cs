// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.Threading.Tasks;
using Marvin.AbstractionLayer.UI;

namespace Marvin.Products.UI
{
    /// <summary>
    /// Interface for product detail views
    /// </summary>
    public interface IProductDetails : IEditModeViewModel, IDetailsViewModel
    {
        /// <summary>
        /// Product view model which will be presented by this detail view
        /// </summary>
        ProductViewModel EditableObject { get; }

        /// <summary>
        /// Method to load the product details
        /// </summary>
        Task Load(long productId);
    }
}
