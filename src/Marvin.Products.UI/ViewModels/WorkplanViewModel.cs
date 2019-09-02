using Caliburn.Micro;
using Marvin.Products.UI.ProductService;
using Marvin.Workflows;

namespace Marvin.Products.UI
{
    /// <summary>
    /// ViewModel for workplan of a recipe
    /// </summary>
    public class WorkplanViewModel : PropertyChangedBase
    {
        /// <summary>
        /// Underlying model
        /// </summary>
        public WorkplanModel Model { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecipeViewModel"/> class.
        /// </summary>
        public WorkplanViewModel(WorkplanModel model)
        {
            Model = model;
        }

        /// <summary>
        /// Updates the internal model and raises NotifyPropertyChanged
        /// </summary>
        /// <param name="workplanModel">The updated model instance</param>
        public void UpdateModel(WorkplanModel workplanModel)
        {
            Model = workplanModel;
            NotifyOfPropertyChange(nameof(Name));
            NotifyOfPropertyChange(nameof(State));
            NotifyOfPropertyChange(nameof(Version));
        }

        /// <summary>
        /// Id of the workplan
        /// </summary>
        public long Id => Model.Id;

        /// <summary>
        /// Name of the workplan
        /// </summary>
        public string Name => Model.Name;

        /// <summary>
        /// Current state of the workplan
        /// </summary>
        public WorkplanState State => Model.State;

        /// <summary>
        /// Current version of the workplan
        /// </summary>
        public int Version => Model.Version;
    }
}