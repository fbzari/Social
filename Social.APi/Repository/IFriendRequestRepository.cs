﻿using Social.APi.Models;

namespace Social.APi.Repository
{
    public interface IFriendRequestRepository
    {
        Task<FriendRequest> GetRequestAsync(string senderId, string receiverId);
        Task AddAsync(FriendRequest friendRequest);

        Task<IEnumerable<FriendRequest>> GetFrienrequestAsync(string email);
        Task<IEnumerable<FriendRequest>> GetPendingStatus(string email);

        void RespondToFriend(FriendRequest friendRequest);

        Task<List<string>> GetFriendsAsync(string email);
    }
}
