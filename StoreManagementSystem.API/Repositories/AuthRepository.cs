using StoreManagementSystem.API.Models;
using StoreManagementSystem.API.Data;
using Microsoft.EntityFrameworkCore;
using StoreManagementSystem.API.Interfaces;

namespace StoreManagementSystem.API.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly StoreDbContext _context;
        public AuthRepository(StoreDbContext context) => _context = context;

        public async Task<User?> GetUserByUserNameAsync(string userName)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public async Task<User?> GetUserByTokenAsync(string token)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.ActionToken == token);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<bool> UserExistsAsync(string userName, string email)
        {
            return await _context.Users.AnyAsync(u => u.UserName == userName || u.Email == email);
        }

        public async Task AddUserAsync(User user) => await _context.Users.AddAsync(user);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
