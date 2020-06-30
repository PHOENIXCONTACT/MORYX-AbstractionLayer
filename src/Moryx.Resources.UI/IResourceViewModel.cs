// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Marvin.Resources.UI.ResourceService;

namespace Marvin.Resources.UI
{
    public interface IResourceViewModel
    {
        ResourceModel Model { get; }

        long Id { get; }

        string Name { get; }

        string Description { get; }

        string TypeName { get; }
    }
}
