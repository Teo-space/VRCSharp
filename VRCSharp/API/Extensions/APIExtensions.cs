using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRCSharp.API.Moderation;

namespace VRCSharp.API.Extensions
{
    public static class APIExtensions
    {
        public static string Convert(this ModerationType type)
        {
            switch (type)
            {
                default:
                    return null;
                case ModerationType.Block:
                    return "block";
                case ModerationType.HideAvatar:
                    return "hideAvatar";
                case ModerationType.Mute:
                    return "mute";
                case ModerationType.ShowAvatar:
                    return "showAvatar";
                case ModerationType.Unmute:
                    return "unmute";
            }
        }

        public static string Convert(this NotificationType type)
        {
            switch (type)
            {
                default:
                    return null;
                case NotificationType.broadcast:
                    return "broadcast";
                case NotificationType.friendRequest:
                    return "friendRequest";
                case NotificationType.invite:
                    return "invite";
                case NotificationType.message:
                    return "message";
                case NotificationType.requestInvite:
                    return "requestInvite";
            }
        }
    }
}
