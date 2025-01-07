using EventManagementSystem.Models;

namespace EventManagementSystem.Repository
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user, string passwordHash);
        Task<User> GetUserByUsernameAsync(string username);
        Task<List<User>> GetAllUsers();

    }
}
