// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.AbstractionLayer.Resources;

namespace Moryx.Resources.Management.Tests.Mocks
{
    public interface INonPublicResource : IResource
    {
    }

    public class NonPublicResource : Resource, INonPublicResource
    {
    }
}
