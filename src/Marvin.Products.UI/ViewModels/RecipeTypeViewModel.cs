using Caliburn.Micro;
using Marvin.Products.UI.ProductService;

namespace Marvin.Products.UI
{
    /// <summary>
    /// ViewModel representing a recipe type
    /// </summary>
    public class RecipeTypeViewModel : PropertyChangedBase
    {
        /// <summary>
        /// Underlying model
        /// </summary>
        public RecipeTypeModel Model { get; private set; }

        /// <summary>
        /// DisplayName of the recipe type
        /// </summary>
        public string DisplayName => Model.DisplayName;

        /// <summary>
        /// Indicator if the recipe has a workplan
        /// </summary>
        public bool HasWorkplans => Model.HasWorkplans;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecipeTypeViewModel"/> class.
        /// </summary>
        public RecipeTypeViewModel(RecipeTypeModel model)
        {
            Model = model;
            UpdateModel(model);
        }

        public void UpdateModel(RecipeTypeModel model)
        {
            Model = model;

            NotifyOfPropertyChange(nameof(HasWorkplans));
            NotifyOfPropertyChange(nameof(DisplayName));
        }
    }
}