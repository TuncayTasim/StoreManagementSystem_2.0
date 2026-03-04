using StoreManagementSystem.API.Models;
using StoreManagementSystem.API.Repositories;

namespace StoreManagementSystem.API.Services
{
    public interface ISalesService
    {
        Task ProcessSaleAsync(int productId, int quantity, string paymentMethod);
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

        public async Task ProcessSaleAsync(int productId, int quantity, string paymentMethod)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null || product.ShelfQuantity < quantity)
                throw new Exception("Product not found or insufficient shelf quantity");

            int remainingToSell = quantity;
            var batches = await _shelfRepository.GetAvailableBatchesAsync(productId);

            foreach (var batch in batches)
            {
                if (remainingToSell <= 0) break;

                int take = Math.Min(batch.CurrentQuantity, remainingToSell);
                
                // Update original shelf batch remaining amount
                batch.CurrentQuantity -= take;
                remainingToSell -= take;

                // 1. Record NEGATIVE Shelf Action (Ledger entry for the sale)
                var shelfSellAction = new Shelf
                {
                    ProductId = productId,
                    ActionId = 3, // Sell
                    Quantity = -take,
                    CurrentQuantity = batch.CurrentQuantity, // snapshot of what is left in THIS shelf batch
                    ActionDateTime = DateTime.Now
                };
                await _shelfRepository.AddActionAsync(shelfSellAction);

                // 2. Record Sale entry with specific batch price
                decimal price = batch.RestockDetails?.PriceSell ?? 0;
                var sale = new Sale
                {
                    ShelfId = batch.Id, // Linking to the original MoveToShelf record that provided stock
                    QuantitySold = take,
                    PaymentMethod = paymentMethod,
                    PriceSold = price
                };
                await _repository.AddSaleAsync(sale);
            }

            // Update Product-wide totals
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
