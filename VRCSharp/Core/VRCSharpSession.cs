using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VRCSharp.API;
using VRCSharp.API.EventArguments;
using VRCSharp.API.Extensions;
using VRCSharp.API.Moderation;
using VRCSharp.API.Other;
using VRCSharp.Global;

namespace VRCSharp
{
    public class VRCSharpSession
    {
        public bool Authenticated = false;
        
        public string Details { get; set; }

        private string _username { get; set; }

        private string _password { get; set; }

        public string AuthToken { get; set; }

        public bool UseProxies { get; set; }

        public AccountInfo Info { get; set; }

        #region Events
        public delegate void NotificationHandler(VRCSharpSession session, NotificationEventArgs args);

        public delegate void CustomNotificationHandler(VRCSharpSession session, string message);

        public event NotificationHandler OnNotificationReceived;

        public event NotificationHandler OnFriendshipRequestReceived;

        public event NotificationHandler OnInviteReceived;

        public event NotificationHandler OnRequestInvite;

        public event CustomNotificationHandler OnAttemptedLogin;
        #endregion

        public VRCSharpSession(string username, string password, bool useProxies = false)
        {
            _username = username;
            _password = password;
            UseProxies = useProxies;

            new Thread(() =>
            {
                System.Timers.Timer timer = new System.Timers.Timer(60000);
                timer.AutoReset = true;
                timer.Elapsed += Timer_Elapsed;
                timer.Enabled = true;
            }).Start();
        }

        public async Task<bool> Login()
        {
            AuthToken = "Basic " + GlobalVars.Base64Encode(_username + ":" + _password);

            HttpClientHandler handler = null;
            HttpClient client = new HttpClient();

            if (UseProxies)
            {
                //Load proxies from Proxies.txt
                handler = new HttpClientHandler();
                handler.Proxy = APIExtensions.GetRandomProxy();
                client = new HttpClient(handler);
            }
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", AuthToken);

            var response = await client.GetAsync($"https://api.vrchat.cloud/api/1/auth/user?apiKey={GlobalVars.ApiKey}");
            if (response.StatusCode != HttpStatusCode.OK)
            {
                Authenticated = false;
                Details = "Invalid Authorization.";
                OnAttemptedLogin?.Invoke(this, Details);
            }
            else
            {
                Authenticated = true;
                Details = "Successful Login Attempt.";
                Info = JsonConvert.DeserializeObject<AccountInfo>(await response.Content.ReadAsStringAsync());
                OnAttemptedLogin?.Invoke(this, Details);
            }


            return Authenticated;
        }

        public void Logout()
        {
            AuthToken = "";
            Authenticated = false;
        }

        #region Event Handler
        private async void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (Authenticated)
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Authorization", AuthToken);

                var response = await client.GetAsync($"https://vrchat.com/api/1/auth/user/notifications?apiKey={GlobalVars.ApiKey}");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var resp = JsonConvert.DeserializeObject<List<NotificationResponse>>(await response.Content.ReadAsStringAsync());

                    foreach (var res in resp)
                    {
                        double result = DateTime.Now.Subtract(res.created_at).TotalSeconds;

                        var amount = Math.Round(result, 0);

                        if (amount < 200)
                        {
                            switch(res.type.Convert())
                            {
                                default:
                                    OnNotificationReceived?.Invoke(this, new NotificationEventArgs(res.type.Convert(), res.message, this.GetAPIUserByID(res.senderUserId).Result));
                                    break;
                                case NotificationType.friendRequest:
                                    OnFriendshipRequestReceived?.Invoke(this, new NotificationEventArgs(res.type.Convert(), res.message, this.GetAPIUserByID(res.senderUserId).Result));
                                    break;
                                case NotificationType.invite:
                                    OnInviteReceived?.Invoke(this, new NotificationEventArgs(res.type.Convert(), res.message, this.GetAPIUserByID(res.senderUserId).Result));
                                    break;
                                case NotificationType.requestInvite:
                                    OnRequestInvite?.Invoke(this, new NotificationEventArgs(res.type.Convert(), res.message, this.GetAPIUserByID(res.senderUserId).Result));
                                    break;
                                
                            }
                        }
                    }
                }
            }
           
        }
        #endregion
    }
}
