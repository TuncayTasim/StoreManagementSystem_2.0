using StoreManagementSystem.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StoreManagementSystem.API.Interfaces
{
    public interface IWarehouseService
    {
        Task RestockProductAsync(int productId, decimal quantity, decimal price, int daysToExpire);
        Task RejectWarehouseBatchAsync(int warehouseId, string reason);
        Task<IEnumerable<Warehouse>> GetHistoryAsync(int productId);
        Task<IEnumerable<Warehouse>> GetAllHistoryAsync(int? productId = null);
        Task<IEnumerable<Warehouse>> GetWarehouseBatchesAsync(int productId);
    }
}
