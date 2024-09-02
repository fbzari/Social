using Social.APi.Models;

namespace Social.APi.Repository
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByIdAsync(int id);
        Task<bool> IsEmailUniqueAsync(string email);
        Task CreateUserAsync(User user);

        Task<IEnumerable<User>> GetAllUsers();

        Task<FriendRequest> GetRequestAsync(int senderId, int receiverId);
        Task AddAsync(FriendRequest friendRequest);
    }
}
