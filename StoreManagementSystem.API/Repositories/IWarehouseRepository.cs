using StoreManagementSystem.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StoreManagementSystem.API.Repositories
{
    public interface IWarehouseRepository
    {
        Task AddActionAsync(Warehouse warehouseAction);
        Task<IEnumerable<Warehouse>> GetProductHistoryAsync(int productId);
        Task<IEnumerable<Warehouse>> GetAllHistoryAsync(int? productId = null);
        Task<IEnumerable<Warehouse>> GetAvailableBatchesAsync(int productId);
        Task<Warehouse?> GetByIdAsync(int id);
        Task UpdateAsync(Warehouse warehouse);
        Task SaveChangesAsync();
    }
}
