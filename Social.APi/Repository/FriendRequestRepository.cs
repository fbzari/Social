using Microsoft.EntityFrameworkCore;
using Social.APi.Data;
using Social.APi.Dtos;
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

        public async Task<IEnumerable<FriendRequest>> GetFrienrequestAsync(string email)
        {
            return await context.FriendRequests.Where(f => f.ReceiverId == email).ToListAsync();

        }

        public async Task<FriendRequest> GetRequestAsync(string senderId, string receiverId)
        {
            return await context.FriendRequests
                        .FirstOrDefaultAsync(fr => fr.SenderId == senderId && fr.ReceiverId == receiverId);
        }
    }
}
