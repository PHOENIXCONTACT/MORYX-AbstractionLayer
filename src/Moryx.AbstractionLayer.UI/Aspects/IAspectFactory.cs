// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.Container;

namespace Moryx.AbstractionLayer.UI.Aspects
{
    [PluginFactory(typeof(INameBasedComponentSelector))]
    public interface IAspectFactory
    {
        IAspect Create(string name);

        void Destroy(IAspect aspect);
    }
}
