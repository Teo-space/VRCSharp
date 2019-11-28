using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

        public static NotificationType Convert(this string type)
        {
            switch (type)
            {
                default:
                    return NotificationType.broadcast;
                case "broadcast":
                    return NotificationType.broadcast;
                case "friendRequest":
                    return NotificationType.friendRequest;
                case "invite":
                    return NotificationType.invite;
                case "message":
                    return NotificationType.message;
                case "requestInvite":
                    return NotificationType.requestInvite;
            }
        }

        public static IWebProxy GetRandomProxy()
        {
            if (!File.Exists("Proxies.txt"))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Couldn't load proxies because we couldn't find Proxies.txt");
                Console.ForegroundColor = ConsoleColor.White;
                return null;
            }
            else
            {
                return new WebProxy(File.ReadAllLines("Proxies.txt")[new Random().Next(1, File.ReadAllLines("Proxies.txt").Count())]);
            }
        }
    }
}
