using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Marvin.ClientFramework.Commands;
using Marvin.ClientFramework.Dialog;
using Marvin.Container;

namespace Marvin.Products.UI.Interaction
{
    [Plugin(LifeCycle.Transient, typeof(IRemoveProductViewModel))]
    internal class RemoveProductViewModel : DialogScreen, IRemoveProductViewModel
    {
        private ICollection<ProductViewModel> _affectedProducts;

        #region Dependencies

        /// <summary>
        /// Injected products controller
        /// </summary>
        public IProductsController ProductsController { get; set; }

        #endregion

        public RemoveProductViewModel(StructureEntryViewModel productToRemove)
        {
            ProductToRemove = productToRemove;

            OkCommand = new AsyncCommand(TryRemoveProduct, o => AffectedProducts?.Count == 0);
            CancelCommand = new DelegateCommand(o => TryClose(false));
        }

        public StructureEntryViewModel ProductToRemove { get; }

        public AsyncCommand OkCommand { get; }

        public ICommand CancelCommand { get; }

        public ICollection<ProductViewModel> AffectedProducts
        {
            get { return _affectedProducts; }
            set
            {
                if (Equals(value, _affectedProducts))
                    return;
                _affectedProducts = value;
                NotifyOfPropertyChange();
                OkCommand.RaiseCanExecuteChanged();
            }
        }

        protected override void OnInitialize()
        {
            DisplayName = "Remove Product";
        }

        private async Task TryRemoveProduct(object unused)
        {
            AffectedProducts = (await ProductsController.RemoveProduct(ProductToRemove.Id))
                .Select(ap => new ProductViewModel(ap)).ToArray();

            if (AffectedProducts.Count == 0)
                TryClose(true);
        }
    }
}