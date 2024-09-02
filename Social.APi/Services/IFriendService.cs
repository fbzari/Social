using Social.APi.Models;

namespace Social.APi.Services
{
    public interface IFriendService
    {
        Task<IEnumerable<FriendRequest>> GetFrienrequestAsync(string email);

    }
}
