using System.Threading.Tasks;
using System.Windows.Media;
using Marvin.AbstractionLayer.UI;

namespace Marvin.Products.UI.Interaction
{
    [ProductDetailsRegistration(DetailsConstants.EmptyType)]
    internal class EmptyDetailsViewModel : EmptyDetailsViewModelBase, IProductDetails
    {
        public Task Load(long productId) => SuccessTask;

        public ProductViewModel EditableObject => null;

        public Geometry Icon => Geometry.Parse(ModuleController.IconPath);

        public override bool CanBeginEdit() => false;
    }
}