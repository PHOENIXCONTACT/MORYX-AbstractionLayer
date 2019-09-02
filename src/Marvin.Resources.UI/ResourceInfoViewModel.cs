using Caliburn.Micro;
using Marvin.Resources.UI.ResourceService;

namespace Marvin.Resources.UI
{
    public class ResourceInfoViewModel : PropertyChangedBase, IResourceViewModel
    {
        public ResourceModel Model { get; private set; }

        public ResourceInfoViewModel(ResourceModel model)
        {
            UpdateModel(model);
        }

        public void UpdateModel(ResourceModel model)
        {
            Model = model;
            NotifyOfPropertyChange(nameof(Name));
            NotifyOfPropertyChange(nameof(Description));
        }

        public long Id => Model.Id;

        public string Name => Model.Name;

        public string Description => Model.Description;

        public string TypeName => Model.Type;
    }
}