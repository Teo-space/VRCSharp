using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VRCSharp;
using VRCSharp.API;
using VRCSharp.API.EventArguments;

namespace TestVRCSharp
{
    class Program
    {
        static void Main(string[] args) => new Program().Start().GetAwaiter().GetResult();
        public static List<string> Proxies = new List<string>();

        private async Task Start()
        {
            try
            {
                LoadProxies();
                Console.Title = "VRCSharp Test";
                
                VRCSharpSession session = new VRCSharpSession("Username", "Password", false);
                await session.Login();
                var world = await session.GetAPIWorldByID("wrld_fac11e5f-1c73-4436-8936-a70b80961c5a");
                var user = await session.GetAPIUserByID("usr_2a7a8c03-8185-4c97-ae0b-7e40c3a1b9a5");

                if (await session.Invite(user, world, "wrld_fac11e5f-1c73-4436-8936-a70b80961c5a:72264~hidden(usr_2a7a8c03-8185-4c97-ae0b-7e40c3a1b9a5)~nonce(D56CCCFA6FEB965669D977C725E7D1621F511E818F67C20A887A51D1CCC11C21)"))
                {
                    Console.WriteLine("Invited successfully.");
                }

                session.OnNotificationReceived += Session_OnNotificationReceived;
               
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            await Task.Delay(-1);
        }

        private void Session_OnNotificationReceived(VRCSharpSession session, NotificationEventArgs args)
        {
            Console.WriteLine("Notification Received. Details: " + args.notificationType.ToString() + " || Message: " + args.Details);
        }

        private void LoadProxies()
        {
            File.WriteAllText("Proxies.txt", new WebClient().DownloadString("https://api.proxyscrape.com/?request=displayproxies&proxytype=https"));

            Proxies = File.ReadAllLines("Proxies.txt").ToList();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Proxies Scraped.");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
