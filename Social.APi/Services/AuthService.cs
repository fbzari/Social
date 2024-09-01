using Microsoft.VisualBasic;
using Social.APi.Models;
using Social.APi.Repository;

namespace Social.APi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository userRepository;

        public AuthService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
      
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await userRepository.GetUserByEmailAsync(email);
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await userRepository.GetUserByIdAsync(id);
        }

        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            return await userRepository.IsEmailUniqueAsync(email);
        }

        public async Task CreateUserAsync(User user)
        {
            await userRepository.CreateUserAsync(user);
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await userRepository.GetAllUsers();
        }
    }
}
