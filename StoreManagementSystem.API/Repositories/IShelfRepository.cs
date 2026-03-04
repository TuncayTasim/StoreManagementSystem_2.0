using StoreManagementSystem.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StoreManagementSystem.API.Repositories
{
    public interface IShelfRepository
    {
        Task AddActionAsync(Shelf shelfAction);
        Task<IEnumerable<Shelf>> GetAvailableBatchesAsync(int productId);
        Task<IEnumerable<Shelf>> GetAllHistoryAsync(int? productId = null);
        Task<Shelf?> GetByIdAsync(int id);
        Task UpdateAsync(Shelf shelf);
        Task SaveChangesAsync();
    }
}
