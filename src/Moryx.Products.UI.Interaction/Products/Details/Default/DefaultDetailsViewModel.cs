// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.AbstractionLayer.UI;

namespace Moryx.Products.UI.Interaction
{
    [ProductDetailsRegistration(DetailsConstants.DefaultType)]
    internal class DefaultDetailsViewModel : ProductDetailsViewModelBase
    {
        protected override bool AspectUsage => true;
    }
}
