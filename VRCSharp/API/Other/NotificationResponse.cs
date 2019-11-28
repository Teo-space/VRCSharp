using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VRCSharp.API.Other
{
    public class NotificationResponse
    {
            public string id { get; set; }
            public string senderUserId { get; set; }
            public string senderUsername { get; set; }
            public string type { get; set; }
            public string message { get; set; }
            public string details { get; set; }
            public bool seen { get; set; }
            public DateTime created_at { get; set; }

    }
}
