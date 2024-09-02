using Social.APi.Models;
using Social.APi.Repository;

namespace Social.APi.Services
{
    public class FriendService : IFriendService
    {
        private readonly IFriendRequestRepository Repository;

        public FriendService(IFriendRequestRepository Repository)
        {
            this.Repository = Repository;
        }
        public async Task<IEnumerable<FriendRequest>> GetFrienrequestAsync(string email)
        {
            return await Repository.GetFrienrequestAsync(email);
        }

        public async Task<IEnumerable<FriendRequest>> GetPendingStatus(string email)
        {
            return await Repository.GetPendingStatus(email);
        }

        public void RespondToFriend(FriendRequest friendRequest)
        {
            Repository.RespondToFriend(friendRequest);
        }
    }
}
