using Caliburn.Micro;
using Marvin.Products.UI.ProductService;
using Marvin.Workflows;

namespace Marvin.Products.UI
{
    public class WorkplanViewModel : PropertyChangedBase
    {
        private WorkplanModel _model;

        public WorkplanViewModel(WorkplanModel model)
        {
            _model = model;
        }

        public long Id => _model.Id;

        public string Name => _model.Name;

        public WorkplanState State => _model.State;

        public int Version => _model.Version;

        /// <summary>
        /// Updates the internal model and raises NotifyPropertyChanged
        /// </summary>
        /// <param name="workplanModel">The updated model instance</param>
        public void UpdateModel(WorkplanModel workplanModel)
        {
            _model = workplanModel;
            NotifyOfPropertyChange(nameof(Name));
            NotifyOfPropertyChange(nameof(State));
            NotifyOfPropertyChange(nameof(Version));
        }
    }
}
