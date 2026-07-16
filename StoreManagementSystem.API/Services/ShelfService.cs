using StoreManagementSystem.API.Data.Entities;
using StoreManagementSystem.API.Interfaces;

namespace StoreManagementSystem.API.Services
{
    public class ShelfService : IShelfService
    {
        private readonly IShelfRepository _repository;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IProductRepository _productRepository;
        private readonly IRejectionRepository _rejectionRepository;

        public ShelfService(IShelfRepository repository, IWarehouseRepository warehouseRepository, IProductRepository productRepository, IRejectionRepository rejectionRepository)
        {
            _repository = repository;
            _warehouseRepository = warehouseRepository;
            _productRepository = productRepository;
            _rejectionRepository = rejectionRepository;
        }
        
        public async Task RejectShelfBatchAsync(int shelfId, string reason)
        {
            var batch = await _repository.GetByIdAsync(shelfId);
            if (batch == null || batch.CurrentQuantity <= 0)
                throw new Exception("Batch not found or has no quantity to reject.");

            var product = await _productRepository.GetByIdAsync(batch.ProductId);
            if (product == null)
                throw new Exception("Associated product not found.");

            var rejection = new Rejection
            {
                ShelfId = batch.Id,
                Reason = reason
            };
            await _rejectionRepository.AddRejectionAsync(rejection);

            var rejectionAction = new Shelf
            {
                ProductId = batch.ProductId,
                ActionId = 4,
                Quantity = -batch.CurrentQuantity,
                CurrentQuantity = 0,
                ActionDateTime = DateTime.Now,
            };
            await _repository.AddActionAsync(rejectionAction);

            product.ShelfQuantity -= batch.CurrentQuantity;
            batch.CurrentQuantity = 0;

            await _productRepository.UpdateAsync(product);
            await _repository.UpdateAsync(batch);

            await _repository.SaveChangesAsync();
            await _productRepository.SaveChangesAsync();
            await _rejectionRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Shelf>> GetShelfBatchesAsync(int productId)
        {
            return await _repository.GetAvailableBatchesAsync(productId);
        }

        public async Task<IEnumerable<Shelf>> GetAllHistoryAsync(int? productId = null)
        {
            return await _repository.GetAllHistoryAsync(productId);
        }

        public async Task MoveToShelfAsync(int productId, decimal quantity, decimal sellPrice)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null || product.WarehouseQuantity < quantity)
                throw new Exception("Product not found or insufficient warehouse quantity");

            decimal remainingToMove = quantity;
            var batches = await _warehouseRepository.GetAvailableBatchesAsync(productId);

            foreach (var batch in batches)
            {
                if (remainingToMove <= 0) break;

                decimal take = Math.Min(batch.CurrentQuantity, remainingToMove);                
                batch.CurrentQuantity -= take;
                remainingToMove -= take;

                var warehouseMoveAction = new Warehouse
                {
                    ProductId = productId,
                    ActionId = 2,
                    Quantity = -take,
                    CurrentQuantity = batch.CurrentQuantity,
                    ActionDateTime = DateTime.Now
                };
                await _warehouseRepository.AddActionAsync(warehouseMoveAction);

                var shelfAction = new Shelf
                {
                    ProductId = productId,
                    ActionId = 2,
                    Quantity = take,
                    CurrentQuantity = take,
                    ActionDateTime = DateTime.Now,
                    RestockDetails = new ShelfRestock
                    {
                        PriceSell = sellPrice
                    }
                };
                await _repository.AddActionAsync(shelfAction);
            }

            product.WarehouseQuantity -= quantity;
            product.ShelfQuantity += quantity;
            
            await _productRepository.UpdateAsync(product);
            await _repository.SaveChangesAsync();
            await _warehouseRepository.SaveChangesAsync();
            await _productRepository.SaveChangesAsync();
        }
    }
}
