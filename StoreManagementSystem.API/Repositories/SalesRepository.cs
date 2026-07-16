using StoreManagementSystem.API.Models;
using Microsoft.EntityFrameworkCore;
using StoreManagementSystem.API.Interfaces;

namespace StoreManagementSystem.API.Repositories
{
    public class SalesRepository : ISalesRepository
    {
        private readonly Data.StoreDbContext _context;
        public SalesRepository(Data.StoreDbContext context) => _context = context;
        public async Task AddSaleAsync(Sale sale) => await _context.Sales.AddAsync(sale);
        
        public async Task<IEnumerable<Sale>> GetAllSalesAsync(int? productId = null)
        {
            var query = _context.Sales
                .Include(s => s.Shelf)
                    .ThenInclude(sh => sh!.Product)
                        .ThenInclude(p => p!.Category)
                .AsQueryable();

            if (productId.HasValue)
            {
                query = query.Where(s => s.Shelf!.ProductId == productId.Value);
            }

            return await query
                .OrderByDescending(s => s.Shelf!.ActionDateTime)
                .ToListAsync();
        }

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
