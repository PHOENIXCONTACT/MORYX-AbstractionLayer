// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.AbstractionLayer.Activities;
using Moryx.Workflows;

namespace Moryx.AbstractionLayer.Tasks
{
    /// <summary>
    /// Interface for the different generic derived types of <see cref="TaskStep{TActivity,TProcParam,TParam}"/>
    /// </summary>
    public interface ITaskStep<out TParam> : IWorkplanStep
        where TParam : IParameters
    {
        /// <summary>
        /// Parameters of this step. This only offers a getter to use covariance and update the object instead of replacing it
        /// </summary>
        TParam Parameters { get; }
    }
}
