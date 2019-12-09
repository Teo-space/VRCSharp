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
using VRCSharp.API.Extensions;

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
                //LoadProxies();
                Console.Title = "VRCSharp Test";
                
                VRCSharpSession session = new VRCSharpSession("Username", "Password", false);
                session.OnAttemptedLogin += Session_OnAttemptedLogin;

                await session.Login();
               
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            await Task.Delay(-1);
        }

        private void Session_OnAttemptedLogin(VRCSharpSession session, string message)
        {
            Console.WriteLine("Tried to login! Result: " + message);
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
