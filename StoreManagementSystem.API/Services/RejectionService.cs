using StoreManagementSystem.API.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;

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
                decimal qty = 0;
                if (r.ShelfId.HasValue)
                {
                    var batch = await _context.Shelves.FindAsync(r.ShelfId.Value);
                    if (batch != null)
                    {
                        var rejectAction = await _context.Shelves
                            .Where(s => s.ProductId == batch.ProductId && s.ActionId == 4 && s.ActionDateTime >= batch.ActionDateTime)
                            .OrderBy(s => s.ActionDateTime)
                            .FirstOrDefaultAsync();
                        
                        if (rejectAction != null) qty = Math.Abs(rejectAction.Quantity);
                    }
                }
                else if (r.WarehouseId.HasValue)
                {
                    var batch = await _context.Warehouses.FindAsync(r.WarehouseId.Value);
                    if (batch != null)
                    {
                        var rejectAction = await _context.Warehouses
                            .Where(w => w.ProductId == batch.ProductId && w.ActionId == 4 && w.ActionDateTime >= batch.ActionDateTime)
                            .OrderBy(w => w.ActionDateTime)
                            .FirstOrDefaultAsync();

                        if (rejectAction != null) qty = Math.Abs(rejectAction.Quantity);
                    }
                }

                result.Add(new
                {
                    id = r.Id,
                    warehouseId = r.WarehouseId,
                    shelfId = r.ShelfId,
                    reason = r.Reason,
                    quantity = qty
                });
            }

            return result;
        }
    }
}