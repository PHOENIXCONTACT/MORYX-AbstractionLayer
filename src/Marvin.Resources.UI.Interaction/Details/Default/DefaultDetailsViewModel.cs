using Marvin.AbstractionLayer.UI;

namespace Marvin.Resources.UI.Interaction
{
    [ResourceDetailsRegistration(DetailsConstants.DefaultType)]
    internal class DefaultDetailsViewModel : ResourceDetailsViewModelBase
    {
        protected override bool AspectUsage => true;
    }
}