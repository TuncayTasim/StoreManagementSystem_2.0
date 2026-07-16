using Microsoft.EntityFrameworkCore;
using StoreManagementSystem.API.Data;
using StoreManagementSystem.API.Interfaces;
using StoreManagementSystem.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StoreManagementSystem.API.Repositories
{
    public class RejectionRepository : IRejectionRepository
    {
        private readonly StoreDbContext _context;
        public RejectionRepository(StoreDbContext context) => _context = context;

        public async Task AddRejectionAsync(Rejection rejection)
        {
            await _context.Rejections.AddAsync(rejection);
        }

        public async Task<IEnumerable<Rejection>> GetAllRejectionsAsync()
        {
            return await _context.Rejections.ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
