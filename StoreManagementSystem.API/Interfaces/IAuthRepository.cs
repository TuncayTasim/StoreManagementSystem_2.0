using StoreManagementSystem.API.Models;

namespace StoreManagementSystem.API.Interfaces
{
    public interface IAuthRepository
    {
        Task<User?> GetUserByUserNameAsync(string userName);
        Task<User?> GetUserByTokenAsync(string token);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByIdAsync(int id);
        Task<bool> UserExistsAsync(string userName, string email);
        Task AddUserAsync(User user);
        Task SaveChangesAsync();
    }
}
