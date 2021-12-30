// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.Notifications.Adapters;
using Moryx.Runtime.Modules;

namespace Moryx.Notifications.Facades
{
    /// <summary>
    /// Facade interface for providing notifications
    /// </summary>
    public interface INotificationSource : INotificationSourceAdapter, ILifeCycleBoundFacade
    {
        /// <summary>
        /// Name of the Source which will publish notifications
        /// </summary>
        string Name { get; }
    }
}
