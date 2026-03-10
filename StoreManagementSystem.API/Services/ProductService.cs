using StoreManagementSystem.API.Helpers;
using StoreManagementSystem.API.Models;
using StoreManagementSystem.API.Repositories;

namespace StoreManagementSystem.API.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task CreateProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
    }

    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task CreateProductAsync(Product product)
        {
            if (string.IsNullOrEmpty(product.Barcode))
            {
                product.Barcode = BarcodeGenerator.GenerateEan13();
            }
            await _repository.AddAsync(product);
            await _repository.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            await _repository.UpdateAsync(product);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            await _repository.DeleteAsync(id);
            await _repository.SaveChangesAsync();
        }
    }
}
