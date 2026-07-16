using StoreManagementSystem.API.Models;

namespace StoreManagementSystem.API.Interfaces
{
    public interface ISalesRepository
    {
        Task AddSaleAsync(Sale sale);
        Task<IEnumerable<Sale>> GetAllSalesAsync(int? productId = null);
        Task SaveChangesAsync();
    }
}
