// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.AbstractionLayer.Processes;

namespace Moryx.AbstractionLayer.Activities
{
    /// <summary>
    /// Common interface for all parameters
    /// </summary>
    public interface IParameters
    {
        /// <summary>
        /// Create new parameters object with resolved binding values from process
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        IParameters Bind(IProcess process);
    }
}
