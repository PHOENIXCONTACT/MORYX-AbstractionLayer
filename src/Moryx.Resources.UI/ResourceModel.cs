// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.Collections.Generic;
using System.Linq;
using Moryx.AbstractionLayer.UI;

// ReSharper disable once CheckNamespace
namespace Moryx.Resources.UI.ResourceService
{
    public partial class ResourceModel : ITreeItemModel<ResourceModel>
    {
        public IReadOnlyList<ResourceModel> Children => References
            .First(r => r.Name == "Children").Targets;
    }
}
