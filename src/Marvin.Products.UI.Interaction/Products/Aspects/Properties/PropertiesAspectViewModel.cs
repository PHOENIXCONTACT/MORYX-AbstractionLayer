namespace Marvin.Products.UI.Interaction.Aspects
{
    [ProductAspectRegistration(nameof(PropertiesAspectViewModel))]
    internal class PropertiesAspectViewModel : ProductAspectViewModelBase
    {
        public override string DisplayName => "Properties";

        public override bool IsRelevant(ProductViewModel product)
        {
            return product.Properties.SubEntries.Count > 0;
        }
    }
}