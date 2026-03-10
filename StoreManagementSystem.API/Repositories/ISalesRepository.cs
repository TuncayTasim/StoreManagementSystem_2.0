using StoreManagementSystem.API.Models;

namespace StoreManagementSystem.API.Repositories
{
    public interface ISalesRepository
    {
        Task AddSaleAsync(Sale sale);
        Task<IEnumerable<Sale>> GetAllSalesAsync(int? productId = null);
        Task SaveChangesAsync();
    }
}
