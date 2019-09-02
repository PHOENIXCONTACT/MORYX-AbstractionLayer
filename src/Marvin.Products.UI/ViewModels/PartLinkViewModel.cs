using System.Linq;
using Caliburn.Micro;
using Marvin.Controls;
using Marvin.Products.UI.ProductService;

namespace Marvin.Products.UI
{
    /// <summary>
    /// View model that represents a single part connector
    /// </summary>
    public class PartLinkViewModel : PropertyChangedBase
    {
        private EntryViewModel _properties;

        /// <summary>
        /// Underlying DTO
        /// </summary>
        public PartModel Model { get; }

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
        /// Product referenced by the part
        /// </summary>
        public ProductInfoViewModel Product { get; }

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