namespace VRCSharp.API
{
    public class FriendRequestCancel
    {
        public Success success { get; set; }
    }

    public class Success
    {
        public string message { get; set; }
        public int status_code { get; set; }
    }

}