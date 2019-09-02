using System.Threading.Tasks;
using Marvin.AbstractionLayer.UI;
using Marvin.Resources.UI.ResourceService;

namespace Marvin.Resources.UI.Interaction
{
    [ResourceDetailsRegistration(DetailsConstants.EmptyType)]
    internal class EmptyDetailsViewModel : EmptyDetailsViewModelBase, IResourceDetails
    {
        public Task Load(long resourceId) =>
            SuccessTask;

        public Task Load(ResourceModel resource) =>
            SuccessTask;

        public ResourceViewModel EditableObject => null;

        public override bool CanBeginEdit() => false;
    }
}