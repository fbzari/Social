using Microsoft.EntityFrameworkCore;
using Social.APi.Data;
using Social.APi.Models;

namespace Social.APi.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly SocialApiContext context;

        public UserRepository(SocialApiContext context)
        {
            this.context = context;
        }

        public async Task CreateUserAsync(User user)
        {
            await context.AddAsync(user);
            await context.SaveChangesAsync();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await context.Users.Include(u => u.UserRoles)
                                      .Include(u => u.SentFriendRequests)
                                      .Include(u => u.ReceivedFriendRequests)
                                      .Include(u => u.Friendships)
                                      .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await context.Users.FindAsync(id);
        }

        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            return !await context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await context.Users.Include(u => u.UserRoles)
                                      .Include(u => u.SentFriendRequests)
                                      .Include(u => u.ReceivedFriendRequests)
                                      .Include(u => u.Friendships)
                                      .ToListAsync();
        }

        public Task<FriendRequest> GetRequestAsync(int senderId, int receiverId)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(FriendRequest friendRequest)
        {
            throw new NotImplementedException();
        }
    }
}
