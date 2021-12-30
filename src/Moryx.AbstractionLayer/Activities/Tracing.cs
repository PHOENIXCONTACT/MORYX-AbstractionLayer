// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Moryx.AbstractionLayer.Activities
{
    /// <summary>
    /// Activity trace information
    /// </summary>
    [DataContract]
    public class Tracing : IActivityTracing
    {
        /// <inheritdoc />
        public DateTime? Started { get; set; }

        /// <inheritdoc />
        public DateTime? Completed { get; set; }

        /// <inheritdoc />
        public string Text { get; set; }

        /// <inheritdoc />
        public int ErrorCode { get; set; }

        /// <summary>
        /// Generic progress information
        /// </summary>
        public int Progress { get; set; }

        /// <summary>
        /// Resource that executed the activity
        /// </summary>
        public long ResourceId { get; set; }

        ///
        // ReSharper disable once InconsistentNaming <-- too cool to rename :P
        public Sparta Transform<Sparta>() where Sparta
            : class, IActivityTracing, new()
        {
            if (this is Sparta)
                return this as Sparta;

            var replacement = new Sparta();
            var replacementType = typeof(Sparta);
            var sharedProperties = GetType().GetProperties()
                // ReSharper disable once PossibleNullReferenceException
                .Where(p => p.DeclaringType.IsAssignableFrom(replacementType));

            foreach (var property in sharedProperties)
            {
                var value = property.GetValue(this);
                if (property.CanWrite)
                    property.SetValue(replacement, value);
            }

            return replacement;
        }
    }
}
