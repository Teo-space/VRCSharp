using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRCSharp;
using VRCSharp.API;

namespace TestVRCSharp
{
    class Program
    {
        static void Main(string[] args) => new Program().Start().GetAwaiter().GetResult();

        private async Task Start()
        {
            try
            {
                Console.Title = "VRCSharp Test";
                VRCSharpSession session = new VRCSharpSession("Username", "Password");
                await session.Login();
                if (session.Authenticated)
                {
                    var world = await session.GetAPIWorldByID("wrld_4cf554b4-430c-4f8f-b53e-1f294eed230b");

                    if (await session.VisitWorld(world, 76428))
                    {
                        Console.WriteLine("Bot has joined the room. Now votekicking user... ");

                        var user = await session.GetAPIUserByID("usr_e28db278-1ccd-4c23-89b9-9933e619000e");

                        await session.VoteKick(user);
                    }
                    
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            await Task.Delay(-1);
        }
    }
}
