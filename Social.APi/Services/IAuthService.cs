using Social.APi.Models;

namespace Social.APi.Services
{
    public interface IAuthService
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByIdAsync(int id);
        Task<bool> IsEmailUniqueAsync(string email);
        Task CreateUserAsync(User user);

        Task<IEnumerable<User>> GetAllUsers();
    }
}
