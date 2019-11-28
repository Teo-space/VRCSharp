using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VRCSharp.API
{
    public class FriendStatus
    {
            public string id { get; set; }
            public string senderUserId { get; set; }
            public string senderUsername { get; set; }
            public string receiverUserId { get; set; }
            public string type { get; set; }
            public DateTime created_at { get; set; }
            public string jobName { get; set; }
            public string jobColor { get; set; }
    }
}
