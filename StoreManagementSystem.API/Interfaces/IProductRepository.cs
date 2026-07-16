using StoreManagementSystem.API.Data.Entities;

namespace StoreManagementSystem.API.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}
