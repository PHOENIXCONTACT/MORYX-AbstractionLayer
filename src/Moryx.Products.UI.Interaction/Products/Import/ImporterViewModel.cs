// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using Moryx.Controls;
using Moryx.Products.UI.ProductService;

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
            if (Parameters != null)
            {
                foreach (var entry in Parameters.SubEntries.Cast<ImportParameterViewModel>())
                {
                    entry.ValueChanged -= OnUpdateTriggerChanged;
                }
            }

            Parameters = new EntryViewModel(new Serialization.Entry { DisplayName = "Root" });
            foreach (var parameter in parameters.SubEntries)
            {
                var viewModel = new ImportParameterViewModel(parameter.ToSerializationEntry());
                viewModel.ValueChanged += OnUpdateTriggerChanged;
                Parameters.SubEntries.Add(viewModel);
            }
        }

        /// <summary>
        /// Update parameters if a <see cref="ImportParameterViewModel.ValueChanged"/> was modified
        /// </summary>
        private async void OnUpdateTriggerChanged(object sender, Serialization.Entry importParameter)
        {
            var parameters = Parameters.Entry;
            var updatedParameters = await _productServiceModel.UpdateImportParameters(_importer.Name, parameters.ToServiceEntry());
            CreateParameterViewModel(updatedParameters);
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
            get => _parameters;
            set
            {
                if (Equals(value, _parameters))
                    return;
                _parameters = value;
                NotifyOfPropertyChange();
            }
        }

        public bool ValidateInput()
        {
            return Parameters.SubEntries.All(se => !se.Entry.Validation.IsRequired || !string.IsNullOrWhiteSpace(se.Value));
        }

        /// <summary>
        /// Import product using the current values
        /// </summary>
        /// <returns></returns>
        public Task<ImportStateModel> Import()
        {
            var parameters = Parameters.Entry;
            return _productServiceModel.Import(_importer.Name, parameters.ToServiceEntry());
        }
    }
}
