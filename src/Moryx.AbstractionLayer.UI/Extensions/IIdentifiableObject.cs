// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

namespace Moryx.AbstractionLayer.UI
{
    /// <summary>
    /// Object with an id
    /// </summary>
    public interface IIdentifiableObject
    {
        /// <summary>
        /// Identifier of the object
        /// </summary>
        long Id { get; }
    }
}
