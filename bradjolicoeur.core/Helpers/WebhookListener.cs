﻿using System;

using bradjolicoeur.core.Models;

namespace bradjolicoeur.core.Helpers
{
    public class WebhookListener : IWebhookListener
    {
        public event EventHandler<WebhookNotificationEventArgs> WebhookNotification = delegate { };

        public void RaiseWebhookNotification(object sender, string operation, IdentifierSet identifierSet)
        {
            if (sender == null)
            {
                throw new ArgumentNullException(nameof(sender));
            }

            if (string.IsNullOrEmpty(operation))
            {
                throw new ArgumentException("The 'operation' parameter must be a non-empty string.", nameof(operation));
            }

            if (identifierSet == null)
            {
                throw new ArgumentNullException(nameof(identifierSet));
            }

            WebhookNotification(sender, new WebhookNotificationEventArgs(identifierSet, operation));
        }
    }
}
