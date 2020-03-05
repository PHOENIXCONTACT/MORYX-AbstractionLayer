using Marvin.Products.UI.Interaction.Properties;

namespace Marvin.Products.UI.Interaction.Aspects
{
    [ProductAspectRegistration(nameof(PropertiesAspectViewModel))]
    internal class PropertiesAspectViewModel : ProductAspectViewModelBase
    {
        public override string DisplayName => Strings.PropertiesAspectViewModel_DisplayName;

        public override bool IsRelevant(ProductViewModel product)
        {
            return product.Properties.SubEntries.Count > 0;
        }
    }
}