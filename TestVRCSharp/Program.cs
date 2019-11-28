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
                
                
                foreach (var acc in File.ReadAllLines("accounts.txt").ToList())
                    {
                     
                        var username = acc.Split(':')[0];
                        var password = acc.Split(':')[1];

                        VRCSharpSession session = new VRCSharpSession(username, password, false);
                        await session.Login();

                        if (session.Authenticated)
                        {
                            Console.WriteLine("Logged in on: " + username + " || Friending User...");

                            var user = await session.GetAPIUserByID("usr_2a7a8c03-8185-4c97-ae0b-7e40c3a1b9a5");

                        await session.Notify(user, VRCSharp.API.Moderation.NotificationType.invite, "");

                        }
                }
               
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            await Task.Delay(-1);
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
