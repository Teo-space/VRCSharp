using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VRCSharp.API.Moderation
{
    public class NotificationPayload
    {
       public string message { get; set; }
       public string type { get; set; }
    }

    public class InvitePayload
    {
        public string message { get; set; } // so the api doesnt fuss about it

        public string type { get; set; }

        public Details details { get; set; }
    }

    public class Details
    {
        public string worldId { get; set; }

        public string worldName { get; set; }

        public Details(APIWorld world, string worldIDWithTags)
        {
            worldId = worldIDWithTags;
            worldName = world.name.ToString();
        }
    }
}
