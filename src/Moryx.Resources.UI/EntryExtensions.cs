// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Newtonsoft.Json;

namespace Moryx.Resources.UI
{
    /// <summary>
    /// Extensions for the entry class to convert from <see cref="Moryx.Resources.UI.ResourceService.Entry"/> to <see cref="Moryx.Serialization.Entry"/> and back
    /// </summary>
    public static class EntryExtensions
    {
        /// <summary>
        /// Converts the <see cref="Moryx.Resources.UI.ResourceService.Entry"/> to <see cref="Moryx.Serialization.Entry"/>
        /// </summary>
        public static Moryx.Serialization.Entry ToSerializationEntry(this Moryx.Resources.UI.ResourceService.Entry serviceEntry)
        {
            var json = JsonConvert.SerializeObject(serviceEntry);
            return JsonConvert.DeserializeObject<Moryx.Serialization.Entry>(json);
        }

        /// <summary>
        /// Converts the <see cref="Moryx.Serialization.Entry"/> to <see cref="Moryx.Resources.UI.ResourceService.Entry"/>
        /// </summary>
        public static Moryx.Resources.UI.ResourceService.Entry ToServiceEntry(this Moryx.Serialization.Entry serializationEntry)
        {
            var json = JsonConvert.SerializeObject(serializationEntry);
            return JsonConvert.DeserializeObject<Moryx.Resources.UI.ResourceService.Entry>(json);
        }

        /// <summary>
        /// Converts the <see cref="Moryx.Resources.UI.ResourceService.MethodEntry"/> to <see cref="Moryx.Serialization.MethodEntry"/>
        /// </summary>
        public static Moryx.Serialization.MethodEntry ToSerializationEntry(this Moryx.Resources.UI.ResourceService.MethodEntry serviceEntry)
        {
            var json = JsonConvert.SerializeObject(serviceEntry);
            return JsonConvert.DeserializeObject<Moryx.Serialization.MethodEntry>(json);
        }

        /// <summary>
        /// Converts the <see cref="Moryx.Serialization.MethodEntry"/> to <see cref="Moryx.Resources.UI.ResourceService.MethodEntry"/>
        /// </summary>
        public static Moryx.Resources.UI.ResourceService.MethodEntry ToServiceEntry(this Moryx.Serialization.MethodEntry serializationEntry)
        {
            var json = JsonConvert.SerializeObject(serializationEntry);
            return JsonConvert.DeserializeObject<Moryx.Resources.UI.ResourceService.MethodEntry>(json);
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