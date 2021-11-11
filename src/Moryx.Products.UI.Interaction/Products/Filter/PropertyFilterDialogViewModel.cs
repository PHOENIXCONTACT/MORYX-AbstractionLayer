using System.Collections.Generic;
using System.Windows.Input;
using Moryx.ClientFramework.Dialog;
using Moryx.Controls;
using Moryx.WpfToolkit;
using Entry = Moryx.Serialization.Entry;

namespace Moryx.Products.UI.Interaction
{
    internal class PropertyFilterDialogViewModel : DialogScreen
    {
        private ProductDefinitionViewModel _selectedProductDefinition;
        private EntryViewModel _currentEntryViewModel;

        public ICommand ApplyCmd { get; }

        public ICommand CancelCmd { get; }

        public ProductDefinitionViewModel ProductType
        {
            get => _selectedProductDefinition;
            set
            {
                _selectedProductDefinition = value;
                NotifyOfPropertyChange();
            }
        }

        public EntryViewModel CurrentEntryViewModel
        {
            get => _currentEntryViewModel;
            set
            {
                _currentEntryViewModel = value;
                NotifyOfPropertyChange();
            }
        }

        public ICommand AddCmd { get; set; }

        public PropertyFilterDialogViewModel(ProductDefinitionViewModel productType)
        {
            ProductType = productType;

            AddCmd = new RelayCommand(Add);
            ApplyCmd = new RelayCommand(Apply);
            CancelCmd = new RelayCommand(Cancel);
        }

        private void Add(object obj)
        {
            var entry = ((EntryViewModel) obj).Entry;

            if (CurrentEntryViewModel == null)
                CurrentEntryViewModel = new EntryViewModel(new List<Entry> { entry });
            else
                CurrentEntryViewModel.SubEntries.Add(new EntryViewModel(entry));

        }

        private void Apply(object obj)
        {

        }

        private void Cancel(object obj)
        {
            TryClose(false);
        }
    }
}
