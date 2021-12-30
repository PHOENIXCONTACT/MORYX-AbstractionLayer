// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.Workflows;
using System;

namespace Moryx.AbstractionLayer.Activities
{
    /// <summary>
    /// Indicates that an result is a failure
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class OutputTypeAttribute : Attribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="outputType"></param>
        public OutputTypeAttribute(OutputType outputType)
        {
            OutputType = outputType;
        }

        /// <summary>
        /// Set output type
        /// </summary>
        public OutputType OutputType { get; }
    }
}
