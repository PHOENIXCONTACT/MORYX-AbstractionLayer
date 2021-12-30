// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

namespace Moryx.Resources.Management.Tests.Mocks
{
    public class DerivedResource : SimpleResource
    {
        public override int MultiplyFoo(int factor)
        {
            return Foo *= factor + 1;
        }
    }
}
