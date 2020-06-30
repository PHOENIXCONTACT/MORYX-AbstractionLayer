// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.Products.UI.Interaction.Properties;

namespace Moryx.Products.UI.Interaction.Aspects
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
