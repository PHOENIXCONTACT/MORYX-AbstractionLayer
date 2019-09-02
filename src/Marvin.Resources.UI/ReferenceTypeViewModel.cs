using Caliburn.Micro;
using Marvin.Resources.UI.ResourceService;

namespace Marvin.Resources.UI
{
    public class ReferenceTypeViewModel : PropertyChangedBase
    {
        public ReferenceTypeModel Model { get; }

        public ResourceTypeViewModel ResourceType { get; }

        public string DisplayName => Model.DisplayName;

        public string Description => Model.Description;

        public bool IsCollection => Model.IsCollection;

        public bool IsRequired => Model.IsRequired;

        public ReferenceTypeViewModel(ResourceTypeViewModel resourceType, ReferenceTypeModel model)
        {
            ResourceType = resourceType;
            Model = model;
        }
    }
}