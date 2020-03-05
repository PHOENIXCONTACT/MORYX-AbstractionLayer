using Marvin.Resources.UI.Interaction.Properties;

namespace Marvin.Resources.UI.Interaction.Aspects
{
    [ResourceAspectRegistration(nameof(PropertiesAspectViewModel))]
    internal class PropertiesAspectViewModel : ResourceAspectViewModelBase
    {
        public override string DisplayName => Strings.PropertiesAspectViewModel_DisplayName;

        public override bool IsRelevant(ResourceViewModel resource)
        {
            return resource.Properties.SubEntries.Count > 0;
        }
    }
}
