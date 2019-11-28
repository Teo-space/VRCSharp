using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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
                VRCSharpSession session = new VRCSharpSession("Username", "Password", false);
                await session.Login();
                if (session.Authenticated)
                {
                    var user = await session.GetAPIUserByID("usr_e28db278-1ccd-4c23-89b9-9933e619000e");

                    if (await session.Moderate(user, VRCSharp.API.Moderation.ModerationType.Mute))
                    {
                        Console.WriteLine("Moderated!");
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
