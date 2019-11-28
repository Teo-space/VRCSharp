using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VRCSharp.API
{
    public class FriendRequest
    {
        public Params _params { get; set; }

        public class Params
        {
            public string userId { get; set; }
        }
    }
}
