using EmocineSveikataServer.Models;

namespace EmocineSveikataServer.Repositories.UserRepository
{
    public interface IUserRepository
    {
        Task<User?> GetUserByUsername(string username);
        Task<User?> GetUserByEmail(string email);
        Task<User?> GetUserById(int id);
        Task<List<User>> GetUsersByIds(List<int> ids);
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> CreateUser(User user);
        Task<bool> UserExists(string username);
        Task<bool> EmailExists(string email);
        Task SaveChanges();
    }
}
