using Marvin.Container;

namespace Marvin.Products.UI.Interaction.Aspects
{
    [PluginFactory]
    internal interface IPartDialogsFactory
    {
        SelectPartLinkDialogViewModel CreateSelectPartDialog(PartConnectorViewModel partConnector);

        void Destroy(SelectPartLinkDialogViewModel vm);
    }
}
