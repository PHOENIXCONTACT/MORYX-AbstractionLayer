using System.Linq;
using Marvin.AbstractionLayer.UI;
using Marvin.Controls;

namespace Marvin.Products.UI.Interaction
{
    [ProductDetailsRegistration(DetailsConstants.DefaultType)]
    internal class DefaultDetailsViewModel : ProductDetailsViewModelBase
    {
        private EntryViewModel _properties;
        /// <summary>
        /// All extended properties of the product
        /// </summary>
        public EntryViewModel Properties
        {
            get { return _properties; }
            set
            {
                if (Equals(value, _properties))
                    return;
                _properties = value;
                NotifyOfPropertyChange();
            }
        }

        protected override void Initialize()
        {
            base.Initialize();

            Properties = new EntryViewModel(ProductProperties);
        }

        public override void BeginEdit()
        {
            base.BeginEdit();

            var clone = ProductProperties.Clone(true);
            Properties = new EntryViewModel(clone);
        }

        public override void EndEdit()
        {
            base.EndEdit();

            ProductProperties = Properties.Entry;
        }

        public override void CancelEdit()
        {
            Properties = new EntryViewModel(ProductProperties);

            base.CancelEdit();
        }
    }
}
