// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System;
using System.ComponentModel;
using Marvin.Controls;
using Marvin.Products.UI.ProductService;
using Marvin.Serialization;

namespace Marvin.Products.UI.Interaction
{
    /// <summary>
    ///     Class holding importer parameter
    /// </summary>
    internal class ImportParameterViewModel : EntryViewModel
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="property">Importer parameter</param>
        public ImportParameterViewModel(Entry property) : base(property)
        {
            Model = property;

            PropertyChanged += OnPropertyChanged;
        }

        /// <summary>
        /// Listen to property changed and check if the value was modified. In that case
        /// invoke the value changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="propertyChangedEventArgs"></param>
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == nameof(Value))
                ValueChanged?.Invoke(this, Model);
        }

        /// <summary>
        ///     Base importer parameter
        /// </summary>
        internal Entry Model { get; }

        /// <summary>
        ///     Event raised when the values was changed
        /// </summary>
        public event EventHandler<Entry> ValueChanged;
    }
}
