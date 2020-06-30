// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

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
