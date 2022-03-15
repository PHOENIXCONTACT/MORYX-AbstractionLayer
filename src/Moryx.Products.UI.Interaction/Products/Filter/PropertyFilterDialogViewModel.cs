using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Moryx.ClientFramework.Commands;
using Moryx.ClientFramework.Dialog;
using Moryx.Controls;
using Moryx.Products.UI.Interaction.Properties;
using Moryx.Serialization;
using Moryx.WpfToolkit;
using Entry = Moryx.Serialization.Entry;

namespace Moryx.Products.UI.Interaction
{
    internal class PropertyFilterDialogViewModel : DialogScreen
    {
        private ProductDefinitionViewModel _productType;
        private EntryViewModel _currentFilter;

        public ICommand ApplyCmd { get; }

        public ICommand CancelCmd { get; }

        public ICommand AddCmd { get; set; }

        public EntryViewModel[] PossibleProperties { get; set; }

        public ProductDefinitionViewModel ProductType
        {
            get => _productType;
            set
            {
                _productType = value;
                NotifyOfPropertyChange();
            }
        }

        public EntryViewModel CurrentFilter
        {
            get => _currentFilter;
            set
            {
                _currentFilter = value;
                NotifyOfPropertyChange();
            }
        }

        public PropertyFilterDialogViewModel(ProductDefinitionViewModel productType)
        {
            ProductType = productType;
            CurrentFilter = new EntryViewModel();

            // Currently no collections or special units are not supported
            PossibleProperties = productType.Properties.SubEntries
                .Where(e => e.Entry.Value.Type != EntryValueType.Collection &&
                            e.Entry.Value.UnitType == EntryUnitType.None).ToArray();

            AddCmd = new RelayCommand(Add, CanAdd);
            ApplyCmd = new AsyncCommand(Apply);
            CancelCmd = new AsyncCommand(Cancel);
        }

        protected override async Task OnInitializeAsync(CancellationToken token)
        {
            await base.OnInitializeAsync(token);

            DisplayName = string.Format(Strings.PropertyFilterDialogViewModel_DisplayName, ProductType.DisplayName);
        }

        private bool CanAdd(object parameters) =>
            parameters is EntryViewModel;

        private void Add(object obj)
        {
            var entry = ((EntryViewModel) obj).Entry;
            CurrentFilter.SubEntries.Add(new EntryViewModel(entry));
        }

        private async Task Apply(object obj)
        {
            await TryCloseAsync(true);
        }

        private async Task Cancel(object obj)
        {
            await TryCloseAsync(false);
        }
    }
}
