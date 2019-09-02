using Marvin.Resources.UI.ResourceService;

namespace Marvin.Resources.UI
{
    public interface IResourceViewModel
    {
        ResourceModel Model { get; }

        long Id { get; }

        string Name { get; }

        string Description { get; }

        string TypeName { get; }
    }
}