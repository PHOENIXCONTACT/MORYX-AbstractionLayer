// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.AbstractionLayer.UI;
using Moryx.Resources.UI.ResourceService;

namespace Moryx.Resources.UI.Interaction
{
    /// <summary>
    /// Tree item view model used for the resources tree
    /// </summary>
    internal class ResourceTreeItemViewModel : TreeItemViewModel
    {
        public ResourceInfoViewModel Resource { get; }

        public override long Id => Resource.Id;

        public override string DisplayName => Resource.Name;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceTreeItemViewModel"/> class.
        /// </summary>
        public ResourceTreeItemViewModel(ResourceModel model)
        {
            Resource = new ResourceInfoViewModel(model);
        }

        public void UpdateModel(ResourceModel model)
        {
            Resource.UpdateModel(model);
            NotifyOfPropertyChange(nameof(DisplayName));
        }
    }
}
