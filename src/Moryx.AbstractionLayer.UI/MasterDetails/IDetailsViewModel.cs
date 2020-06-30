// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

namespace Moryx.AbstractionLayer.UI
{
    /// <summary>
    /// Interface for details view models
    /// </summary>
    public interface IDetailsViewModel
    {
        /// <summary>
        /// Initializes this instance with all needed information
        /// </summary>
        void Initialize(string typeName);
    }
}
