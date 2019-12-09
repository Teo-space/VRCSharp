using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRCSharp.API.Moderation;

namespace VRCSharp.API.EventArguments
{
    public class NotificationEventArgs : EventArgs
    {
        public NotificationType notificationType { get; set; }

        public string Details { get; set; }

        public APIUser Sender { get; set; }

        public NotificationEventArgs(NotificationType notification, string message, APIUser User)
        {
            notificationType = notification;
            Details = message;
            Sender = User;
        }
    }
}
