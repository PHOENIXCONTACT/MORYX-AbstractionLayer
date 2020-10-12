// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Newtonsoft.Json;

namespace Moryx.Products.UI
{
    /// <summary>
    /// Extensions for the entry class to convert from <see cref="Moryx.Products.UI.ProductService.Entry"/> to <see cref="Moryx.Serialization.Entry"/> and back
    /// </summary>
    public static class EntryExtensions
    {
        /// <summary>
        /// Converts the <see cref="Moryx.Products.UI.ProductService.Entry"/> to <see cref="Moryx.Serialization.Entry"/>
        /// </summary>
        public static Moryx.Serialization.Entry ToSerializationEntry(this Moryx.Products.UI.ProductService.Entry serviceEntry)
        {
            var json = JsonConvert.SerializeObject(serviceEntry);
            return JsonConvert.DeserializeObject<Moryx.Serialization.Entry>(json);
        }

        /// <summary>
        /// Converts the <see cref="Moryx.Serialization.Entry"/> to <see cref="Moryx.Products.UI.ProductService.Entry"/>
        /// </summary>
        public static Moryx.Products.UI.ProductService.Entry ToServiceEntry(this Moryx.Serialization.Entry serializationEntry)
        {
            var json = JsonConvert.SerializeObject(serializationEntry);
            return JsonConvert.DeserializeObject<Moryx.Products.UI.ProductService.Entry>(json);
        }

        /// <summary>
        /// Clones an object by serialize it and reads it again
        /// </summary>
        public static TType SerializationClone<TType>(this TType obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            return JsonConvert.DeserializeObject<TType>(json);
        }
    }
}