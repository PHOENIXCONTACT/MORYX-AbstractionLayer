using System.Collections.Generic;
using System.Collections.ObjectModel;
using Caliburn.Micro;
using System.Linq;
using Marvin.AbstractionLayer.UI;
using Marvin.Products.UI.ProductService;

namespace Marvin.Products.UI.Interaction
{
    /// <summary>
    /// View model for the product structure entries
    /// Will be used within the product tree
    /// </summary>
    public class StructureEntryViewModel :PropertyChangedBase,  ITreeItemViewModel
    {
        private readonly ProductStructureEntry _model;
        private ObservableCollection<StructureEntryViewModel> _branches;

        /// <summary>
        /// Create ViewModel using all its branches
        /// </summary>
        internal StructureEntryViewModel(ProductStructureEntry model) : this(model, true)
        {
        }

        /// <summary>
        /// Create ViewModel and specify weather to include the branches
        /// </summary>
        private StructureEntryViewModel(ProductStructureEntry model, bool includeBranches)
        {
            _model = model;
            if (includeBranches)
                Branches = TransformBranches(model.Branches);
        }

        /// <summary>
        /// Create a clone of the view model
        /// </summary>
        public StructureEntryViewModel Clone()
        {
            return new StructureEntryViewModel(_model, false)
            {
                Branches = Branches
            };
        }

        /// <summary>
        /// Gets the MaterialNumber of the product
        /// </summary>
        public string MaterialNumber => _model.MaterialNumber;

        /// <summary>
        /// Gets the name of the product.
        /// </summary>
        public long Id => _model.Id;

        /// <summary>
        /// Gets the identifier of the product
        /// </summary>
        public string Identifier => $"{_model.MaterialNumber}-{_model.Revision:D2}";

        /// <summary>
        /// Gets the name of the product including identity and revision.
        /// </summary>
        public string Name => _model.BranchType == BranchType.Product ? $"{_model.MaterialNumber}-{_model.Revision:D2} {_model.Name}" : _model.Name;

        /// <summary>
        /// Gets a value indicating whether this instance is a product
        /// </summary>
        public bool IsProduct => _model.BranchType == BranchType.Product;

        /// <summary>
        /// Gets the type of the entry.
        /// </summary>
        public string Type => _model.Type;

        /// <summary>
        /// Transforms the children tree items.
        /// </summary>
        private static ObservableCollection<StructureEntryViewModel> TransformBranches(IEnumerable<ProductStructureEntry> children)
        {
            return new ObservableCollection<StructureEntryViewModel>(children.Select(child => new StructureEntryViewModel(child)
            {
                Branches = TransformBranches(child.Branches)
            }));
        }

        /// <summary>
        /// The children of the current tree item
        /// </summary>
        public ObservableCollection<StructureEntryViewModel> Branches
        {
            get { return _branches; }
            set
            {
                _branches = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this item is expanded.
        /// </summary>
        public bool IsExpanded { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this item is selected.
        /// </summary>
        public bool IsSelected { get; set; }
    }
}