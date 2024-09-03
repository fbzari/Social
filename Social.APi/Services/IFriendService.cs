using Social.APi.Models;

namespace Social.APi.Services
{
    public interface IFriendService
    {
        Task<IEnumerable<FriendRequest>> GetFrienrequestAsync(string email);
        Task<IEnumerable<FriendRequest>> GetPendingStatus(string email);
        void RespondToFriend(FriendRequest friendRequest);
        Task<List<string>> GetFriendsAsync(string email);
    }
}
