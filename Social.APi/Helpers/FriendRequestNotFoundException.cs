namespace Social.APi.Helpers
{
    public class FriendRequestNotFoundException : Exception
    {
        public FriendRequestNotFoundException() : base("Friend request not found.") { }
    }
}
