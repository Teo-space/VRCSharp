using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VRCSharp.API;
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

        public AccountInfo Info { get; set; }

        public VRCSharpSession(string username, string password)
        {
            _username = username;
            _password = password;
        }

        public async Task Login()
        {
            AuthToken = "Basic " + GlobalVars.Base64Encode(_username + ":" + _password);                                        
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", AuthToken);

            var response = await client.GetAsync($"https://api.vrchat.cloud/api/1/auth/user?apiKey={GlobalVars.ApiKey}");

            if (response.StatusCode != HttpStatusCode.OK)
            {
                Authenticated = false;
                Details = "Invalid Authorization.";
            }
            else
            {
                Authenticated = true;
                Details = "Successful Login Attempt.";
                Info = JsonConvert.DeserializeObject<AccountInfo>(await response.Content.ReadAsStringAsync());
            }
        }

        public void Logout()
        {
            AuthToken = "";
            Authenticated = false;
        }
    }
}
