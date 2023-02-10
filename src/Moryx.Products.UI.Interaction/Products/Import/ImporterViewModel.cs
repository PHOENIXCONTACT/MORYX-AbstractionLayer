// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using Moryx.Controls;
using Moryx.Products.UI.ProductService;
using Moryx.Tools;
using EntryValueType = Moryx.Serialization.EntryValueType;

namespace Moryx.Products.UI.Interaction
{
    internal class ImporterViewModel : PropertyChangedBase
    {
        private readonly ProductImporter _importer;
        private readonly IProductServiceModel _productServiceModel;

        public ImporterViewModel(ProductImporter importer, IProductServiceModel productServiceModel)
        {
            _importer = importer;
            _productServiceModel = productServiceModel;

            // Create fake root
            CreateParameterViewModel(_importer.Parameters);
        }

        private void CreateParameterViewModel(Entry parameters)
        {
            Parameters = new EntryViewModel(parameters.ToSerializationEntry());
            Parameters.SubEntries.ForEach(e => e.PropertyChanged += OnUpdateTriggerChanged);
        }

        private void UpdateParameterViewModel(Entry parameters)
        {
            Parameters.SubEntries.ForEach(e => e.PropertyChanged -= OnUpdateTriggerChanged);
            Parameters.UpdateModel(parameters.ToSerializationEntry());
            Parameters.SubEntries.ForEach(e => e.PropertyChanged += OnUpdateTriggerChanged);
        }

        /// <summary>
        /// Update parameters if a <see cref="EntryViewModel.Value"/> was modified
        /// </summary>
        private async void OnUpdateTriggerChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            var entry = sender as EntryViewModel;
            if (sender is null || propertyChangedEventArgs.PropertyName != nameof(EntryViewModel.Value))
                return;

            Parameters.EndEdit();
            var parameters = Parameters.Entry;
            var updatedParameters = await _productServiceModel.UpdateImportParameters(_importer.Name, parameters.ToServiceEntry());
            UpdateParameterViewModel(updatedParameters);
        }

        /// <summary>
        /// Name of this importer
        /// </summary>
        public string Name => _importer.Name;

        private EntryViewModel _parameters;
        /// <summary>
        /// Parameters of this importer
        /// </summary>
        public EntryViewModel Parameters
        {
            get { return _parameters; }
            set
            {
                if (Equals(value, _parameters))
                    return;
                if (_parameters is null || value is null)
                {
                    _parameters = value;
                    NotifyOfPropertyChange();
                }
                else
                    _parameters.UpdateModel(value.Entry);
            }
        }

        public bool ValidateInput()
        {
            foreach (var subEntry in Parameters.SubEntries)
            {
               if (!ValidateEntry(subEntry)) 
                   return false;
            }
            return true;
        }

        private bool ValidateEntry(EntryViewModel entryViewModel)
        {
            if (!entryViewModel.Entry.Validation.IsRequired)
                return true;

            switch (entryViewModel.ValueType)
            {
                case EntryValueType.Class:
                case EntryValueType.Collection:
                case EntryValueType.Exception:
                case EntryValueType.Stream:
                    foreach (var subEntry in entryViewModel.SubEntries)
                    {
                        if (!ValidateEntry(subEntry))
                            return false;
                    }
                    break;
                default: // all value types
                    if (string.IsNullOrWhiteSpace(entryViewModel.Value))
                        return false;

                    if (entryViewModel.Entry.Validation.Regex != null)
                    {
                        var validator = new RegexStringValidator(entryViewModel.Entry.Validation.Regex);
                        try
                        {
                            validator.Validate(entryViewModel.Value);
                        }
                        catch (ArgumentException exception)
                        {
                            return false;
                        }
                    }
                    break;
            }
            return true;
        }
        

        /// <summary>
        /// Import product using the current values
        /// </summary>
        /// <returns></returns>
        public Task<ImportStateModel> Import()
        {
            Parameters.EndEdit();
            var parameters = Parameters.Entry;
            return _productServiceModel.Import(_importer.Name, parameters.ToServiceEntry());
        }
    }
}
