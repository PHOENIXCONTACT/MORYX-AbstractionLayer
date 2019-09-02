using Marvin.AbstractionLayer.UI;

namespace Marvin.Products.UI.Interaction
{
    [ProductDetailsRegistration(DetailsConstants.DefaultType)]
    internal class DefaultDetailsViewModel : ProductDetailsViewModelBase
    {
        protected override bool AspectUsage => true;
    }
}
