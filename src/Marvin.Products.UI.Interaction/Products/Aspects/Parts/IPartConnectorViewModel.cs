// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.ComponentModel;
using Caliburn.Micro;

namespace Marvin.Products.UI.Interaction.Aspects
{
    internal interface IPartConnectorViewModel : IScreen, IEditableObject
    {
        PartConnectorViewModel PartConnector { get; }
    }
}
