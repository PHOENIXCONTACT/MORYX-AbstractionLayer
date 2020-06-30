// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.Threading.Tasks;
using Moryx.AbstractionLayer.UI;
using Moryx.Resources.UI.ResourceService;

namespace Moryx.Resources.UI
{
    /// <summary>
    /// Public api for new resource detail view models.
    /// </summary>
    public interface IResourceDetails : IEditModeViewModel, IDetailsViewModel
    {
        /// <summary>
        /// Method to load the needed resource details from the server
        /// </summary>
        Task Load(long resourceId);

        /// <summary>
        /// Loads the details from the given model
        /// </summary>
        Task Load(ResourceModel resource);

        /// <summary>
        /// Current resource to show and edit its details
        /// </summary>
        ResourceViewModel EditableObject { get; }
    }
}
