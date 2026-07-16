using StoreManagementSystem.API.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using StoreManagementSystem.API.Interfaces;

namespace StoreManagementSystem.API.Services
{
    public class RejectionService : IRejectionService
    {
        private readonly StoreDbContext _context;

        public RejectionService(StoreDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<object>> GetAllRejectionsAsync()
        {
            var rejections = await _context.Rejections.ToListAsync();
            var result = new List<object>();

            foreach (var r in rejections)
            {
                string sourceType = "Unknown";
                string productName = "Unknown";
                DateTime? rejectedAt = null;

                if (r.ShelfId.HasValue)
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
                }
                else if (r.WarehouseId.HasValue)
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
                }

                result.Add(new
                {
                    id = r.Id,
                    sourceType = sourceType,
                    reason = r.Reason,
                    rejectedAt = rejectedAt ?? DateTime.Now,
                    productName = productName
                });
            }

            return result;
        }
    }
}