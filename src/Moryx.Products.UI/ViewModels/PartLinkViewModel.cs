// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Caliburn.Micro;
using Moryx.AbstractionLayer.UI;
using Moryx.Controls;
using Moryx.Products.UI.ProductService;
using System.ComponentModel;

namespace Moryx.Products.UI
{
    /// <summary>
    /// View model that represents a single part connector
    /// </summary>
    public class PartLinkViewModel : PropertyChangedBase, IIdentifiableObject, IEditableObject
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
                if (_properties is null)
                {
                    _properties = value;
                    NotifyOfPropertyChange();
                }
                else
                    _properties.UpdateModel(value.Entry);
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
            Properties = new EntryViewModel(Model.Properties.ToSerializationEntry());
        }

        /// <summary>
        /// Copies values from viewmodel to model
        /// </summary>
        public void CopyToModel()
        {
            Model.Properties = Properties.Entry.ToServiceEntry();
        }

        public void BeginEdit()
        {
            Properties.BeginEdit();
        }

        public void EndEdit()
        {
            Properties.EndEdit();
            CopyToModel();
        }

        public void CancelEdit()
        {
            Properties.CancelEdit();
            CopyFromModel();
        }
    }
}
