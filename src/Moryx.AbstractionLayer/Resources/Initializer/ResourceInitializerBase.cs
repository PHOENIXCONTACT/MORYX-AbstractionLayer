// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.Logging;
using Moryx.Modules;
using System.Collections.Generic;

namespace Moryx.AbstractionLayer.Resources.Initializer
{
    /// <summary>
    /// Base class for resource initializers without a custom config
    /// </summary>
    public abstract class ResourceInitializerBase : ResourceInitializerBase<ResourceInitializerConfig>
    {
    }

    /// <summary>
    /// Base class for resource initializers with a custom config
    /// </summary>
    public abstract class ResourceInitializerBase<TConfig> : IResourceInitializer, ILoggingComponent
        where TConfig : ResourceInitializerConfig
    {
        /// <summary>
        /// Configuration of the resourceInitializer
        /// </summary>
        public TConfig Config { get; private set; }

        /// <summary>
        /// Yes, this is a logger!
        /// </summary>
        public IModuleLogger Logger { get; set; }

        /// <inheritdoc />
        public abstract string Name { get; }

        /// <inheritdoc />
        public abstract string Description { get; }

        /// <inheritdoc />
        public void Initialize(ResourceInitializerConfig config)
        {
            // Get child logger
            Logger = Logger.GetChild(Name, GetType());

            // Cast configuration
            Config = (TConfig)config;
        }

        /// <inheritdoc />
        void IPlugin.Start()
        {
        }

        /// <inheritdoc />
        void IPlugin.Stop()
        {
        }

        /// <inheritdoc />
        public abstract IReadOnlyList<Resource> Execute(IResourceGraph graph);
    }
}
