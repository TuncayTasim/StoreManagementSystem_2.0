using StoreManagementSystem.API.Interfaces;
using StoreManagementSystem.API.Models;

namespace StoreManagementSystem.API.Services
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IWarehouseRepository _repository;
        private readonly IProductRepository _productRepository;
        private readonly IRejectionRepository _rejectionRepository;

        public WarehouseService(IWarehouseRepository repository, IProductRepository productRepository, IRejectionRepository rejectionRepository)
        {
            _repository = repository;
            _productRepository = productRepository;
            _rejectionRepository = rejectionRepository;
        }

        public async Task RejectWarehouseBatchAsync(int warehouseId, string reason)
        {
            var batch = await _repository.GetByIdAsync(warehouseId);
            if (batch == null || batch.CurrentQuantity <= 0)
                throw new Exception("Batch not found or has no quantity to reject.");

            var product = await _productRepository.GetByIdAsync(batch.ProductId);
            if (product == null)
                throw new Exception("Associated product not found.");

            var rejection = new Rejection
            {
                WarehouseId = batch.Id,
                Reason = reason
            };
            await _rejectionRepository.AddRejectionAsync(rejection);

            var rejectionAction = new Warehouse
            {
                ProductId = batch.ProductId,
                ActionId = 4,
                Quantity = -batch.CurrentQuantity,
                CurrentQuantity = 0,
                ActionDateTime = DateTime.Now,
            };
            await _repository.AddActionAsync(rejectionAction);

            product.WarehouseQuantity -= batch.CurrentQuantity;
            batch.CurrentQuantity = 0;

            await _productRepository.UpdateAsync(product);
            await _repository.UpdateAsync(batch);

            await _repository.SaveChangesAsync();
            await _productRepository.SaveChangesAsync();
            await _rejectionRepository.SaveChangesAsync();
        }

        public async Task RestockProductAsync(int productId, decimal quantity, decimal price, int daysToExpire)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null) throw new Exception("Product not found");

            product.WarehouseQuantity += quantity;
            await _productRepository.UpdateAsync(product);

            var action = new Warehouse
            {
                ProductId = productId,
                ActionId = 1,
                Quantity = quantity,
                CurrentQuantity = quantity,
                ActionDateTime = DateTime.Now,
                RestockDetails = new WarehouseRestock
                {
                    PriceBought = price,
                    DaysToExpire = daysToExpire
                }
            };

            await _repository.AddActionAsync(action);
            await _repository.SaveChangesAsync();
            await _productRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Warehouse>> GetHistoryAsync(int productId)
        {
            return await _repository.GetProductHistoryAsync(productId);
        }

        public async Task<IEnumerable<Warehouse>> GetAllHistoryAsync(int? productId = null)
        {
            return await _repository.GetAllHistoryAsync(productId);
        }

        public async Task<IEnumerable<Warehouse>> GetWarehouseBatchesAsync(int productId)
        {
            return await _repository.GetAvailableBatchesAsync(productId);
        }
    }
}
