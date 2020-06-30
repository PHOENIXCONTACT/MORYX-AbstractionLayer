// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.Threading.Tasks;
using Moryx.AbstractionLayer.UI;

namespace Moryx.Resources.UI.Interaction.Aspects
{
    /// <summary>
    /// Abstract base class for product aspects
    /// </summary>
    public abstract class ResourceAspectViewModelBase : EditModeViewModelBase, IResourceAspect
    {
        /// <summary>
        /// <see cref="ResourceViewModel"/> instance of the aspect
        /// </summary>
        public ResourceViewModel Resource { get; set; }

        /// <inheritdoc />
        public virtual bool IsRelevant(ResourceViewModel resource) => true;

        /// <inheritdoc />
        public virtual Task Load(ResourceViewModel resource)
        {
            Resource = resource;
            return Task.FromResult(true);
        }

        /// <inheritdoc cref="IResourceAspect" />
        public override Task Save() => Task.FromResult(true);
    }
}
