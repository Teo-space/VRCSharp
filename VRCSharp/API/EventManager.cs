using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VRCSharp.API.Other;
using VRCSharp.Global;

namespace VRCSharp.API
{
    public static class EventManager
    {
        public static VRCSharpSession Session { get; set; }
        public static void Setup(VRCSharpSession session)
        {
            new Thread(() =>
            {
                System.Timers.Timer timer = new System.Timers.Timer(60000);
                timer.AutoReset = true;
                timer.Elapsed += Timer_Elapsed;
                timer.Enabled = true;
            }).Start();
        }

        private static void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", Session.AuthToken);

            var response = client.GetAsync($"https://vrchat.com/api/1/auth/user/notifications?apiKey={GlobalVars.ApiKey}");

            if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var resp = JsonConvert.DeserializeObject<List<NotificationResponse>>(response.Result.Content.ReadAsStringAsync().Result);

                foreach(var res in resp)
                {
                    double result = DateTime.Now.Subtract(res.created_at).TotalSeconds;

                    var amount = Math.Round(result, 0);

                    if (amount < 120)
                    {

                    }
                }
            }
        }
    }
}
