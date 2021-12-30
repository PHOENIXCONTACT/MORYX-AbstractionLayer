// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.Container;
using Moryx.Products.Management.Implementations.Storage;
using System;

namespace Moryx.Products.Management.Plugins.GenericStrategies
{
    /// <summary>
    /// Mapper for columns of type <see cref="double"/>
    /// </summary>
    [FloatStrategyConfiguration]
    [Component(LifeCycle.Transient, typeof(IPropertyMapper), Name = nameof(FloatColumnMapper))]
    internal class FloatColumnMapper : ColumnMapper<double>
    {
        public FloatColumnMapper(Type targetType) : base(targetType)
        {
        }
    }
}