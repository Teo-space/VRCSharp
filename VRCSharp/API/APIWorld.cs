using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VRCSharp.API.Extensions;
using VRCSharp.API.Moderation;
using VRCSharp.API.Worlds;
using VRCSharp.Global;

namespace VRCSharp.API
{

    public static class APIWorldHelper
    {
        public static string CurrentWorldID { get; set; }

        public static int CurrentInstanceID { get; set; }
    }
    public class APIWorld
    {
            public string id { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public bool featured { get; set; }
            public string authorId { get; set; }
            public string authorName { get; set; }
            public int capacity { get; set; }
            public string[] tags { get; set; }
            public string releaseStatus { get; set; }
            public string imageUrl { get; set; }
            public string thumbnailImageUrl { get; set; }
            public string assetUrl { get; set; }
            public string pluginUrl { get; set; }
            public string unityPackageUrl { get; set; }
            public string _namespace { get; set; }
            public bool unityPackageUpdated { get; set; }
            public Unitypackage[] unityPackages { get; set; }
            public int version { get; set; }
            public string organization { get; set; }
            public object previewYoutubeId { get; set; }
            public int favorites { get; set; }
            public int visits { get; set; }
            public int popularity { get; set; }
            public int heat { get; set; }
            public int publicOccupants { get; set; }
            public int privateOccupants { get; set; }
            public int occupants { get; set; }
            public object[][] instances { get; set; }

    }

    public class Unitypackage
    {
        public string id { get; set; }
        public string assetUrl { get; set; }
        public string pluginUrl { get; set; }
        public string unityVersion { get; set; }
        public long unitySortNumber { get; set; }
        public int assetVersion { get; set; }
        public string platform { get; set; }
    }

    public static class APIWorldExtensions
    {
        public static async Task<APIWorld> GetAPIWorldByID(this VRCSharpSession session, string WorldID)
        {
            HttpClientHandler handler = null;
            HttpClient client = new HttpClient();

            if (session.UseProxies)
            {
                //Load proxies from Proxies.txt
                handler = new HttpClientHandler();
                handler.Proxy = APIExtensions.GetRandomProxy();
                client = new HttpClient(handler);
            }
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", session.AuthToken);

            var response = await client.GetAsync($"https://vrchat.com/api/1/worlds/{WorldID}?apiKey={GlobalVars.ApiKey}");
            return JsonConvert.DeserializeObject<APIWorld>(await response.Content.ReadAsStringAsync());
        }


        public static async Task<bool> VisitWorld(this VRCSharpSession session, APIWorld World, int InstanceID)
        {
            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("userId", "asd"));
            nvc.Add(new KeyValuePair<string, string>("worldId", $"{World.id}:{InstanceID.ToString()}"));
            var req = new HttpRequestMessage(HttpMethod.Put, "https://vrchat.com/api/1/visits") { Content = new FormUrlEncodedContent(nvc) };

            HttpClientHandler handler = null;
            HttpClient client = new HttpClient();

            if (session.UseProxies)
            {
                //Load proxies from Proxies.txt
                handler = new HttpClientHandler();
                handler.Proxy = APIExtensions.GetRandomProxy();
                client = new HttpClient(handler);
            }
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", session.AuthToken);


            var response = await client.SendAsync(req);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                APIWorldHelper.CurrentWorldID = World.id;
                APIWorldHelper.CurrentInstanceID = InstanceID;
                return true;
            }
            else
            {
                return false;
            }
        }

        public static async Task<bool> VoteKick(this VRCSharpSession session, APIUser User)
        {
            if (APIWorldHelper.CurrentWorldID == null)
            {
                Console.WriteLine("I can't votekick if you haven't made me join a world!");
                return false;
            }
            else
            {
                HttpClientHandler handler = null;
                HttpClient client = new HttpClient();

                if (session.UseProxies)
                {
                    //Load proxies from Proxies.txt
                    handler = new HttpClientHandler();
                    handler.Proxy = APIExtensions.GetRandomProxy();
                    client = new HttpClient(handler);
                }

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Authorization", session.AuthToken);
                var payload = JsonConvert.SerializeObject(new VoteKickPayload() { worldId = APIWorldHelper.CurrentWorldID, instanceId = APIWorldHelper.CurrentInstanceID.ToString()});
                Console.WriteLine(payload.ToString());
                var response = await client.PostAsync($"https://vrchat.com/api/1/user/{User.id}/votekick?apiKey={GlobalVars.ApiKey}", new StringContent(payload, Encoding.UTF8, "application/json"));

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static async Task<bool> Invite(this VRCSharpSession session, APIUser user, APIWorld world, string worldIdWithTags)
        {
            HttpClientHandler handler = null;
            HttpClient client = new HttpClient();

            if (session.UseProxies)
            {
                //Load proxies from Proxies.txt
                handler = new HttpClientHandler();
                handler.Proxy = APIExtensions.GetRandomProxy();
                client = new HttpClient(handler);
            }

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", session.AuthToken);

            var payload = JsonConvert.SerializeObject(new InvitePayload() { message = "", type = "invite", details = new Details(world, worldIdWithTags)});

            var response = await client.PostAsync($"https://vrchat.com/api/1/user/{user.id}/notification?apiKey={GlobalVars.ApiKey}", new StringContent(payload, Encoding.UTF8, "application/json"));
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                
                return false;
            }
        }
        public static async Task<bool> Message(this VRCSharpSession session, APIUser user, string Message, string worldIdWithTags)
        {
            HttpClientHandler handler = null;
            HttpClient client = new HttpClient();

            if (session.UseProxies)
            {
                //Load proxies from Proxies.txt
                handler = new HttpClientHandler();
                handler.Proxy = APIExtensions.GetRandomProxy();
                client = new HttpClient(handler);
            }

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", session.AuthToken);

            var payload = JsonConvert.SerializeObject(new InvitePayload() { message = "", type = "invite", details = new Details($"{session.Info.displayName} said: {Message}", worldIdWithTags) });

            var response = await client.PostAsync($"https://vrchat.com/api/1/user/{user.id}/notification?apiKey={GlobalVars.ApiKey}", new StringContent(payload, Encoding.UTF8, "application/json"));
            Console.WriteLine(payload.ToString());
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}