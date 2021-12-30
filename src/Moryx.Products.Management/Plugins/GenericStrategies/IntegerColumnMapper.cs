// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0


/* Unmerged change from project 'Moryx.Products.Management (net5.0)'
Before:
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Moryx.Container;
After:
using Moryx.Container;
using Moryx.Products.Management.Implementations.Storage;
using Moryx.Tools;
using System;
*/

/* Unmerged change from project 'Moryx.Products.Management (netcoreapp3.1)'
Before:
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Moryx.Container;
After:
using Moryx.Container;
using Moryx.Products.Management.Implementations.Storage;
using Moryx.Tools;
using System;
*/
using Moryx.Container;

/* Unmerged change from project 'Moryx.Products.Management (net5.0)'
Before:
using System.Reflection;
using System.Runtime.InteropServices;
using System;
After:
using System;
using System.Reflection;
using System.Reflection;
*/

/* Unmerged change from project 'Moryx.Products.Management (netcoreapp3.1)'
Before:
using System.Reflection;
using System.Runtime.InteropServices;
using System;
After:
using System;
using System.Reflection;
using System.Reflection;
*/
using System;

namespace Moryx.Products.Management.Plugins.GenericStrategies
{
    /// <summary>
    /// Mapper for columns of type <see cref="long"/>
    /// </summary>
    [IntegerStrategyConfiguration]
    [Component(LifeCycle.Transient, typeof(IPropertyMapper), Name = nameof(IntegerColumnMapper))]
    internal class IntegerColumnMapper : ColumnMapper<long>
    {
        private Type _enumType;

        public IntegerColumnMapper(Type targetType) : base(targetType)
        {

        }

        protected override IPropertyAccessor<object, long> CreatePropertyAccessor(PropertyInfo objectProp)
        {
            if (objectProp.PropertyType.IsEnum)
            {
                _enumType = Enum.GetUnderlyingType(objectProp.PropertyType);
                return new ConversionAccessor<long, object>(objectProp, ReadEnum, WriteEnum);
            }

            if (objectProp.PropertyType == typeof(DateTime))
                return new ConversionAccessor<long, DateTime>(objectProp, dt => dt.Ticks, DateTime.FromBinary);

            if (objectProp.PropertyType == typeof(bool))
                return new ConversionAccessor<long, bool>(objectProp, v => v ? 1 : 0, l => l > 0);

            return base.CreatePropertyAccessor(objectProp);
        }

        protected override object ToExpressionValue(object value)
        {
            if (Property.PropertyType.IsEnum)
                return ReadEnum(value);

            if (Property.PropertyType == typeof(DateTime))
                return ((DateTime)value).Ticks;

            if (Property.PropertyType == typeof(bool))
                return (bool)value ? 1L : 0L;

            return base.ToExpressionValue(value);
        }

        private long ReadEnum(object value)
        {
            var underlyingValue = Convert.ChangeType(value, _enumType);
            return (long)Convert.ChangeType(underlyingValue, TypeCode.Int64);
        }
        private object WriteEnum(long value)
        {
            var underlyingValue = Convert.ChangeType(value, _enumType);
            return Enum.ToObject(Property.PropertyType, underlyingValue);
        }
    }
}