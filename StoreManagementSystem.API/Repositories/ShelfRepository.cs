using Microsoft.EntityFrameworkCore;
using StoreManagementSystem.API.Interfaces;
using StoreManagementSystem.API.Data.Entities;

namespace StoreManagementSystem.API.Repositories
{
    public class ShelfRepository : IShelfRepository
    {
        private readonly Data.StoreDbContext _context;
        public ShelfRepository(Data.StoreDbContext context) => _context = context;
        public async Task AddActionAsync(Shelf shelfAction) => await _context.Shelves.AddAsync(shelfAction);

        public async Task<IEnumerable<Shelf>> GetAvailableBatchesAsync(int productId)
        {
            return await _context.Shelves
                .Where(s => s.ProductId == productId && s.ActionId == 2 && s.CurrentQuantity > 0)
                .Include(s => s.RestockDetails)
                .OrderBy(s => s.ActionDateTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Shelf>> GetAllHistoryAsync(int? productId = null)
        {
            var query = _context.Shelves
                .Include(s => s.ActionType)
                .Include(s => s.Product)
                .AsQueryable();

            if (productId.HasValue)
            {
                query = query.Where(s => s.ProductId == productId.Value);
            }

            return await query
                .OrderByDescending(s => s.ActionDateTime)
                .ToListAsync();
        }

        public async Task<Shelf?> GetByIdAsync(int id)
        {
            return await _context.Shelves.FindAsync(id);
        }

        public Task UpdateAsync(Shelf shelf)
        {
            _context.Shelves.Update(shelf);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
