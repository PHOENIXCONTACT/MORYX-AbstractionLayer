// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.Resources.Management.Resources;

namespace Moryx.Resources.Management.States
{
    /// <summary>
    /// State of a <see cref="ResourceWrapper"/> when an error occurred while acting on wrapped resource
    /// </summary>
    internal class ErrorState : ResourceStateBase
    {
        /// <summary>
        /// constructor
        /// </summary>
        public ErrorState(ResourceWrapper context, StateMap stateMap) : base(context, stateMap)
        {
        }

        /// <inheritdoc />
        public override void Initialize()
        {
            // Do nothing
        }

        /// <inheritdoc />
        public override void Start()
        {
            // Do nothing
        }
    }
}
