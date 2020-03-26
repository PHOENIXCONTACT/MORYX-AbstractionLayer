// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.Linq;
using Caliburn.Micro;
using Marvin.AbstractionLayer.UI;
using Marvin.Controls;
using Marvin.Products.UI.ProductService;

namespace Marvin.Products.UI
{
    /// <summary>
    /// View model that represents a single part connector
    /// </summary>
    public class PartLinkViewModel : PropertyChangedBase, IIdentifiableObject
    {
        private EntryViewModel _properties;

        /// <summary>
        /// Underlying DTO
        /// </summary>
        public PartModel Model { get; private set; }

        /// <summary>
        /// Identifier of the PartLink
        /// </summary>
        public long Id => Model.Id;

        /// <summary>
        /// Product referenced by the part
        /// </summary>
        public ProductInfoViewModel Product { get; private set; }

        /// <summary>
        /// Properties of the link
        /// </summary>
        public EntryViewModel Properties
        {
            get { return _properties; }
            private set
            {
                _properties = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PartLinkViewModel"/> class.
        /// </summary>
        public PartLinkViewModel(PartModel model)
            : this(model, new ProductInfoViewModel(model.Product))
        {
        }

        public PartLinkViewModel(PartModel model, ProductInfoViewModel product)
        {
            Model = model;
            Product = product;

            CopyFromModel();
        }

        /// <summary>
        /// Updates to model
        /// </summary>
        public void UpdateModel(PartModel model)
        {
            Model = model;
            Product = new ProductInfoViewModel(model.Product);
            CopyFromModel();
        }

        /// <summary>
        /// Copies values from model to viewmodel
        /// </summary>
        public void CopyFromModel()
        {
            Properties = new EntryViewModel(Model.Properties.Clone(true));
        }

        /// <summary>
        /// Copies values from viewmodel to model
        /// </summary>
        public void CopyToModel()
        {
            Model.Properties = Properties.Entry;
        }
    }
}
