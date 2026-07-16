using StoreManagementSystem.API.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StoreManagementSystem.API.Interfaces
{
    public interface IShelfService
    {
        Task MoveToShelfAsync(int productId, decimal quantity, decimal sellPrice);
        Task RejectShelfBatchAsync(int shelfId, string reason);
        Task<IEnumerable<Shelf>> GetShelfBatchesAsync(int productId);
        Task<IEnumerable<Shelf>> GetAllHistoryAsync(int? productId = null);
    }
}
