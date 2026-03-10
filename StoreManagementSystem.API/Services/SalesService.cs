using StoreManagementSystem.API.Models;
using StoreManagementSystem.API.Repositories;

namespace StoreManagementSystem.API.Services
{
    public interface ISalesService
    {
        Task ProcessSaleAsync(int productId, decimal quantity, string paymentMethod);
        Task<IEnumerable<Sale>> GetAllSalesAsync(int? productId = null);
    }

    public class SalesService : ISalesService
    {
        private readonly ISalesRepository _repository;
        private readonly IShelfRepository _shelfRepository;
        private readonly IProductRepository _productRepository;

        public SalesService(ISalesRepository repository, IShelfRepository shelfRepository, IProductRepository productRepository)
        {
            _repository = repository;
            _shelfRepository = shelfRepository;
            _productRepository = productRepository;
        }

        public async Task ProcessSaleAsync(int productId, decimal quantity, string paymentMethod)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null || product.ShelfQuantity < quantity)
                throw new Exception("Product not found or insufficient shelf quantity");

            decimal remainingToSell = quantity;
            var batches = await _shelfRepository.GetAvailableBatchesAsync(productId);

            foreach (var batch in batches)
            {
                if (remainingToSell <= 0) break;

                decimal take = Math.Min(batch.CurrentQuantity, remainingToSell);
                
                batch.CurrentQuantity -= take;
                remainingToSell -= take;

                var shelfSellAction = new Shelf
                {
                    ProductId = productId,
                    ActionId = 3, 
                    Quantity = -take,
                    CurrentQuantity = batch.CurrentQuantity, 
                    ActionDateTime = DateTime.Now
                };
                await _shelfRepository.AddActionAsync(shelfSellAction);

                decimal price = batch.RestockDetails?.PriceSell ?? 0;
                var sale = new Sale
                {
                    ShelfId = batch.Id, 
                    QuantitySold = take,
                    PaymentMethod = paymentMethod,
                    PriceSold = price
                };
                await _repository.AddSaleAsync(sale);
            }

            product.ShelfQuantity -= quantity;
            await _productRepository.UpdateAsync(product);

            await _repository.SaveChangesAsync();
            await _shelfRepository.SaveChangesAsync();
            await _productRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Sale>> GetAllSalesAsync(int? productId = null)
        {
            return await _repository.GetAllSalesAsync(productId);
        }
    }
}
