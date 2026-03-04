using System.Threading.Tasks;

using StoreManagementSystem.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StoreManagementSystem.API.Services
{
    public interface IShelfService
    {
        Task MoveToShelfAsync(int productId, int quantity, decimal sellPrice);
        Task RejectShelfBatchAsync(int shelfId, string reason);
        Task<IEnumerable<Shelf>> GetShelfBatchesAsync(int productId);
        Task<IEnumerable<Shelf>> GetAllHistoryAsync(int? productId = null);
    }
}
