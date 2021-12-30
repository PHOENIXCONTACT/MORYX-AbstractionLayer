// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.Notifications.Notifications;
using System;
using System.Collections.Generic;

namespace Moryx.Notifications.Adapters
{
    /// <summary>
    /// Adapter for the <see cref="INotificationSource"/>
    /// </summary>
    public interface INotificationSourceAdapter
    {
        /// <summary>
        /// Returns the currently published notifications
        /// </summary>
        IReadOnlyList<INotification> GetPublished();

        /// <summary>
        /// Informs the sender of the notification to acknowledge it
        /// </summary>
        void Acknowledge(INotification notification);

        /// <summary>
        /// Restore notification on sender adapter
        /// </summary>
        void Sync();

        /// <summary>
        /// Informs a sender of an acknowledgement that it was processed
        /// </summary>
        void AcknowledgeProcessed(INotification notification);

        /// <summary>
        /// Informs a sender of an notification that it was processed
        /// </summary>
        void PublishProcessed(INotification notification);

        /// <summary>
        /// Event to publish a notification
        /// </summary>
        event EventHandler<INotification> Published;

        /// <summary>
        /// Event to publish an acknowledged notification
        /// </summary>
        event EventHandler<INotification> Acknowledged;
    }
}
