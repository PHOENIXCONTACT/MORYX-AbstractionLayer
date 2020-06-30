// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Marvin.AbstractionLayer.UI;

namespace Marvin.Resources.UI.Interaction
{
    [ResourceDetailsRegistration(DetailsConstants.DefaultType)]
    internal class DefaultDetailsViewModel : ResourceDetailsViewModelBase
    {
        protected override bool AspectUsage => true;
    }
}
