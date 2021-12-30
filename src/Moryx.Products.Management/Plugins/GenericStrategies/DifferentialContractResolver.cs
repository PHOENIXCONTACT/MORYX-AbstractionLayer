// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Moryx.Products.Management.Plugins.GenericStrategies
{
    /// <summary>
    /// Specialized <see cref="IContractResolver"/> for the JSON serializer to determine
    /// only the relevant properties
    /// </summary>
    internal class DifferentialContractResolver<TReference> : DefaultContractResolver
        where TReference : class
    {
        private readonly string[] _ignoredProperties;

        public DifferentialContractResolver(string[] ignoredProperties)
        {
            _ignoredProperties = ignoredProperties;
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var properties = base.CreateProperties(type, memberSerialization);
            properties = properties.Where(ShouldSerialize).ToList();
            return properties;
        }

        private bool ShouldSerialize(JsonProperty property)
        {
            if (_ignoredProperties.Any(prop => prop == property.PropertyName))
                return false;

            if (typeof(TReference).IsAssignableFrom(property.PropertyType))
                return false;

            if (typeof(IEnumerable<TReference>).IsAssignableFrom(property.PropertyType))
                return false;

            return true;
        }
    }
}
