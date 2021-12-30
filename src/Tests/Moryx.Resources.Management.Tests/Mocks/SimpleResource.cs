// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.AbstractionLayer.Resources;
using Moryx.AbstractionLayer.Resources.Attributes;
using System;

namespace Moryx.Resources.Management.Tests.Mocks
{
    public interface IDuplicateFoo : IPublicResource
    {
        int Foo { get; }
    }

    public interface ISimpleResource : IPublicResource
    {
        int Foo { get; set; }

        int MultiplyFoo(int factor);

        int MultiplyFoo(int factor, ushort offset);

        event EventHandler<int> FooChanged;

        event EventHandler<bool> FooEven;

        void RaiseEvent();

        event EventHandler SomeEvent;
    }

    public interface INonResourceInterface
    {
        void Validate();
    }

    [ResourceAvailableAs(typeof(INonResourceInterface))]
    public class SimpleResource : PublicResource, ISimpleResource, IDuplicateFoo, INonResourceInterface
    {
        private int _foo;

        public int Foo
        {
            get { return _foo; }
            set
            {
                _foo = value;
                FooChanged?.Invoke(this, _foo);
                FooEven?.Invoke(this, value % 2 == 0);
            }
        }

        public virtual int MultiplyFoo(int factor)
        {
            return Foo *= factor;
        }

        int ISimpleResource.MultiplyFoo(int factor, ushort offset)
        {
            return Foo = Foo * factor + offset;
        }

        public event EventHandler<int> FooChanged;

        public event EventHandler<bool> FooEven;

        public event EventHandler SomeEvent;
        public void RaiseEvent()
        {
            SomeEvent?.Invoke(this, EventArgs.Empty);
        }

        public void Validate()
        {
        }
    }
}
