using StoreManagementSystem.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StoreManagementSystem.API.Services
{
    public interface IWarehouseService
    {
        Task RestockProductAsync(int productId, int quantity, decimal price, int daysToExpire);
        Task RejectWarehouseBatchAsync(int warehouseId, string reason);
        Task<IEnumerable<Warehouse>> GetHistoryAsync(int productId);
        Task<IEnumerable<Warehouse>> GetAllHistoryAsync(int? productId = null);
        Task<IEnumerable<Warehouse>> GetWarehouseBatchesAsync(int productId);
    }
}
