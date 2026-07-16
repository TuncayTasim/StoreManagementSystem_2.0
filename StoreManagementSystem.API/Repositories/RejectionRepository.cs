using Microsoft.EntityFrameworkCore;
using StoreManagementSystem.API.Data;
using StoreManagementSystem.API.Data.Entities;
using StoreManagementSystem.API.DTOs;
using StoreManagementSystem.API.Interfaces;
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
        public async Task<RejectionExternalInfoDTO> GetWarehouseBatch(Rejection r, string sourceType, string productName, DateTime? rejectedAt)
        {
            var batch = await _context.Warehouses.Include(w => w.Product).FirstOrDefaultAsync(w => w.Id == r.WarehouseId.Value);
            if (batch != null)
            {
                sourceType = "Warehouse";
                productName = batch.Product?.Name ?? "Unknown";

                var rejectAction = await _context.Warehouses
                    .Where(w => w.ProductId == batch.ProductId && w.ActionId == 4 && w.ActionDateTime >= batch.ActionDateTime)
                    .OrderBy(w => w.ActionDateTime)
                    .FirstOrDefaultAsync();

                if (rejectAction != null) rejectedAt = rejectAction.ActionDateTime;
            }

            return new RejectionExternalInfoDTO(sourceType, productName, rejectedAt);
        }

        public async Task<RejectionExternalInfoDTO> GetShelfBatch(Rejection r, string sourceType, string productName, DateTime? rejectedAt)
        {
            var batch = await _context.Shelves.Include(s => s.Product).FirstOrDefaultAsync(s => s.Id == r.ShelfId.Value);
            if (batch != null)
            {
                sourceType = "Shelf";
                productName = batch.Product?.Name ?? "Unknown";

                var rejectAction = await _context.Shelves
                    .Where(s => s.ProductId == batch.ProductId && s.ActionId == 4 && s.ActionDateTime >= batch.ActionDateTime)
                    .OrderBy(s => s.ActionDateTime)
                    .FirstOrDefaultAsync();

                if (rejectAction != null) rejectedAt = rejectAction.ActionDateTime;
            }

            return new RejectionExternalInfoDTO(sourceType, productName, rejectedAt);
        }
    }
}
