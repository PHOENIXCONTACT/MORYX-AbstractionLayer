using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using Marvin.Controls;
using Marvin.Products.UI.ProductService;
using Marvin.Serialization;

namespace Marvin.Products.UI.Interaction
{
    internal class ImporterViewModel : PropertyChangedBase
    {
        private readonly ProductImporter _importer;
        private readonly IProductServiceModel _productsController;

        public ImporterViewModel(ProductImporter importer, IProductServiceModel productsController)
        {
            _importer = importer;
            _productsController = productsController;

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

            Parameters = new EntryViewModel(new Entry { Key = new EntryKey { Name = "Root" } });
            foreach (var parameter in parameters.SubEntries)
            {
                var viewModel = new ImportParameterViewModel(new Entry
                {   // Create clone to clear after every import
                    Key = parameter.Key,
                    Validation = parameter.Validation,
                    Description = parameter.Description,
                    Value = parameter.Value.Clone(false),
                    Prototypes = parameter.Prototypes,
                    SubEntries = parameter.SubEntries.Select(se => se.Clone(true)).ToList()
                });
                viewModel.ValueChanged += OnUpdateTriggerChanged;
                Parameters.SubEntries.Add(viewModel);
            }
        }

        /// <summary>
        /// Update parameters if they were was modified
        /// </summary>
        private async void OnUpdateTriggerChanged(object sender, Entry importParameter)
        {
            var root = new Entry { Key = new EntryKey { Name = "Root" } };
            var parameters = Parameters.SubEntries.Cast<ImportParameterViewModel>().Select(ip => ip.Model).ToList();
            root.SubEntries = parameters;

            root = await _productsController.UpdateParameters(_importer.Name, root);
            CreateParameterViewModel(root);
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
        public Task<ProductModel> Import()
        {
            var root = new Entry { Key = new EntryKey { Name = "Root" } };
            var parameters = Parameters.SubEntries.Cast<ImportParameterViewModel>().Select(ip => ip.Model).ToList();
            root.SubEntries = parameters;

            return _productsController.ImportProduct(_importer.Name, root);
        }
    }
}