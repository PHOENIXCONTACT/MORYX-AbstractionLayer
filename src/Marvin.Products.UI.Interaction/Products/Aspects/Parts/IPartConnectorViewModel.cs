using System.ComponentModel;
using Caliburn.Micro;

namespace Marvin.Products.UI.Interaction.Aspects
{
    internal interface IPartConnectorViewModel : IScreen, IEditableObject
    {
        PartConnectorViewModel PartConnector { get; }
    }
}
