﻿using Microsoft.EntityFrameworkCore;
using Social.APi.Data;
using Social.APi.Dtos;
using Social.APi.Helpers;
using Social.APi.Models;

namespace Social.APi.Repository
{
    public class FriendRequestRepository : IFriendRequestRepository
    {
        private readonly SocialApiContext context;

        public FriendRequestRepository(SocialApiContext context)
        {
            this.context = context;
        }
        public async Task AddAsync(FriendRequest friendRequest)
        {
            await context.FriendRequests.AddAsync(friendRequest);
            await context.SaveChangesAsync();
        }

        public async Task<List<string>> GetFriendsAsync(string email)
        {
            var friends = await context.FriendRequests
                            .Where(x => (x.ReceiverId == email || x.SenderId == email) && x.Status == nameof(UserActions.Accepted))
                            .Select(x => x.ReceiverId == email ? x.SenderId : x.ReceiverId)
                            .Distinct()
                            .ToListAsync();

            return friends;
        }

        public async Task<IEnumerable<FriendRequest>> GetFrienrequestAsync(string email)
        {
            return await context.FriendRequests.Where(f => f.ReceiverId == email && f.Status == Enum.GetName(UserActions.Pending)).ToListAsync();

        }

        public async Task<IEnumerable<FriendRequest>> GetPendingStatus(string email)
        {
            return await context.FriendRequests
                .Where(f => f.SenderId == email && f.Status == nameof(UserActions.Pending)).ToListAsync();
        }

        public async Task<FriendRequest> GetRequestAsync(string senderId, string receiverId)
        {
            return await context.FriendRequests
                        .FirstOrDefaultAsync(fr => (fr.SenderId == senderId && fr.ReceiverId == receiverId) ||
                                                    (fr.SenderId == receiverId && fr.ReceiverId == senderId)
                        );
        }

        public void RespondToFriend(FriendRequest friendRequest)
        {
            var existingRequest = context.FriendRequests
                .FirstOrDefault(fr => fr.SenderId == friendRequest.SenderId && fr.ReceiverId == friendRequest.ReceiverId);

            if (existingRequest != null)
            {
                existingRequest.Status = friendRequest.Status;
                context.SaveChanges();
            }
            else
            {
                throw new FriendRequestNotFoundException();
            }
        }
    }
}
