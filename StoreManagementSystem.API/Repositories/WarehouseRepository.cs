using StoreManagementSystem.API.Models;
using Microsoft.EntityFrameworkCore;

namespace StoreManagementSystem.API.Repositories
{
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly Data.StoreDbContext _context;
        public WarehouseRepository(Data.StoreDbContext context) => _context = context;

        public async Task AddActionAsync(Warehouse warehouseAction) => await _context.Warehouses.AddAsync(warehouseAction);

        public async Task<IEnumerable<Warehouse>> GetProductHistoryAsync(int productId)
        {
            return await _context.Warehouses
                .Where(w => w.ProductId == productId)
                .Include(w => w.ActionType)
                .Include(w => w.Product)
                .OrderByDescending(w => w.ActionDateTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Warehouse>> GetAllHistoryAsync(int? productId = null)
        {
            var query = _context.Warehouses
                .Include(w => w.ActionType)
                .Include(w => w.Product)
                .AsQueryable();

            if (productId.HasValue)
            {
                query = query.Where(w => w.ProductId == productId.Value);
            }

            return await query
                .OrderByDescending(w => w.ActionDateTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Warehouse>> GetAvailableBatchesAsync(int productId)
        {
            return await _context.Warehouses
                .Where(w => w.ProductId == productId && w.ActionId == 1 && w.CurrentQuantity > 0)
                .OrderBy(w => w.ActionDateTime)
                .ToListAsync();
        }

        public async Task<Warehouse?> GetByIdAsync(int id)
        {
            return await _context.Warehouses.FindAsync(id);
        }

        public Task UpdateAsync(Warehouse warehouse)
        {
            _context.Warehouses.Update(warehouse);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
