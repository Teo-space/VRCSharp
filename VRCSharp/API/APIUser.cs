using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public class APIUser
    {
            public string id { get; set; }
            public string username { get; set; }
            public string displayName { get; set; }
            public string bio { get; set; }
            public object[] bioLinks { get; set; }
            public string currentAvatarImageUrl { get; set; }
            public string currentAvatarThumbnailImageUrl { get; set; }
            public string status { get; set; }
            public string statusDescription { get; set; }
            public string state { get; set; }
            public string[] tags { get; set; }
            public string developerType { get; set; }
            public string last_platform { get; set; }
            public bool allowAvatarCopying { get; set; }
            public bool isFriend { get; set; }
            public string friendKey { get; set; }
            public string location { get; set; }
            public string worldId { get; set; }
            public string instanceId { get; set; }


    }

    public static class APIUserExtensions
    {

        public static async Task<APIUser> GetAPIUserByID(this VRCSharpSession session, string UserID)
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

            var response = await client.GetAsync($"https://vrchat.com/api/1/users/{UserID}?apiKey={GlobalVars.ApiKey}");

            return JsonConvert.DeserializeObject<APIUser>(await response.Content.ReadAsStringAsync());
        }

        public static async Task<FriendStatus> Friend(this VRCSharpSession session, APIUser User)
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
            var payload = JsonConvert.SerializeObject(new FriendRequest() { _params= new FriendRequest.Params() { userId=User.id} });
            var response = await client.PostAsync($"https://vrchat.com/api/1/user/{User.id}/friendRequest?apiKey={GlobalVars.ApiKey}", new StringContent(payload, Encoding.UTF8, "application/json"));

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<FriendStatus>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                return null;
            }
        }

        public static async Task<FriendRequestCancel> Unfriend(this VRCSharpSession session, APIUser User)
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

            var response = await client.DeleteAsync($"https://vrchat.com/api/1/user/{User.id}/friendRequest?apiKey={GlobalVars.ApiKey}");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<FriendRequestCancel>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                return null;
            }
        }
        public static async Task<bool> Notify(this VRCSharpSession session, APIUser user, NotificationType type, string message)
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

            var payload = JsonConvert.SerializeObject(new NotificationPayload() { message = message, type = type.Convert() });

            var response = await client.PostAsync($"https://vrchat.com/api/1/user/{user.id}/notification", new StringContent(payload, Encoding.UTF8, "application/json"));

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static async Task<bool> Moderate(this VRCSharpSession session, APIUser user, ModerationType type)
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
            var payload = JsonConvert.SerializeObject(new ModerationPayload() { moderated = user.id, type = type.Convert() });

            var response = await client.PostAsync($"https://vrchat.com/api/1/auth/user/playermoderations?apiKey={GlobalVars.ApiKey}&userId={user.id}", new StringContent(payload, Encoding.UTF8, "application/json"));

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
