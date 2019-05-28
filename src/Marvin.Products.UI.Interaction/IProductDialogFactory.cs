using System.Collections.Generic;
using System.Collections.ObjectModel;
using Caliburn.Micro;
using Marvin.Container;
using Marvin.Products.UI.Recipes;

namespace Marvin.Products.UI.Interaction
{
    /// <summary>
    /// Factory for ImportProductsViewModel view model
    /// </summary>
    [PluginFactory]
    internal interface IProductDialogFactory
    {
        /// <summary>
        /// Create ImportViewModel instance
        /// </summary>
        IImportViewModel CreateImportDialog();

        /// <summary>
        /// Create a dialog to remove a product
        /// </summary>
        IRemoveProductViewModel CreateRemoveProductViewModel(StructureEntryViewModel productToRemove);

        /// <summary>
        /// Creates a dialog to show revisions
        /// </summary>
        IRevisionsViewModel CreateShowRevisionsDialog(string identifier);

        /// <summary>
        /// Create CreateRevisionViewModel instance
        /// </summary>
        ICreateRevisionViewModel CreateCreateRevisionDialog(StructureEntryViewModel structureEntry);

        /// <summary>
        /// Creates a dialog to a add recipes
        /// </summary>
        IAddRecipeDialog CreateAddRecipeDialog(string productName, long productId,
            ICollection<WorkplanViewModel> workplans);

        /// <summary>
        /// Destroy DiagramInfoViewModel instance
        /// </summary>
        void Destroy(IScreen instance);
    }
}
