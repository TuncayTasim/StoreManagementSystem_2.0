using Moq;
using StoreManagementSystem.API.Data.Entities;
using StoreManagementSystem.API.Interfaces;
using StoreManagementSystem.API.Services;
using Xunit;

namespace StoreManagementSystem.Tests
{
    public class ShelfServiceTests
    {
        private readonly Mock<IShelfRepository> _mockShelfRepo;
        private readonly Mock<IWarehouseRepository> _mockWarehouseRepo;
        private readonly Mock<IProductRepository> _mockProductRepo;
        private readonly Mock<IRejectionRepository> _mockRejectionRepo;
        private readonly ShelfService _service;

        public ShelfServiceTests()
        {
            _mockShelfRepo = new Mock<IShelfRepository>();
            _mockWarehouseRepo = new Mock<IWarehouseRepository>();
            _mockProductRepo = new Mock<IProductRepository>();
            _mockRejectionRepo = new Mock<IRejectionRepository>();
            _service = new ShelfService(_mockShelfRepo.Object, _mockWarehouseRepo.Object, _mockProductRepo.Object, _mockRejectionRepo.Object);
        }

        [Fact]
        public async Task MoveToShelfAsync_DecrementsWarehouseAndIncrementsShelf()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Apple", WarehouseQuantity = 10, ShelfQuantity = 0 };
            var batch = new Warehouse { Id = 1, ProductId = 1, CurrentQuantity = 10, ActionId = 1 };
            
            _mockProductRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);
            _mockWarehouseRepo.Setup(r => r.GetAvailableBatchesAsync(1)).ReturnsAsync(new List<Warehouse> { batch });

            // Act
            await _service.MoveToShelfAsync(1, 5, 2.0m);

            // Assert
            Assert.Equal(5, product.WarehouseQuantity);
            Assert.Equal(5, product.ShelfQuantity);
            Assert.Equal(5, batch.CurrentQuantity);
            
            _mockWarehouseRepo.Verify(r => r.AddActionAsync(It.Is<Warehouse>(w => w.Quantity == -5)), Times.Once);
            _mockShelfRepo.Verify(r => r.AddActionAsync(It.Is<Shelf>(s => s.Quantity == 5)), Times.Once);
            _mockProductRepo.Verify(r => r.UpdateAsync(product), Times.Once);
        }

        [Fact]
        public async Task RejectShelfBatchAsync_ZeroesBatchAndDecrementsProduct()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Apple", ShelfQuantity = 10 };
            var batch = new Shelf { Id = 1, ProductId = 1, CurrentQuantity = 10 };

            _mockShelfRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(batch);
            _mockProductRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

            // Act
            await _service.RejectShelfBatchAsync(1, "Damaged");

            // Assert
            Assert.Equal(0, product.ShelfQuantity);
            Assert.Equal(0, batch.CurrentQuantity);
            _mockRejectionRepo.Verify(r => r.AddRejectionAsync(It.Is<Rejection>(rej => rej.ShelfId == 1 && rej.Reason == "Damaged")), Times.Once);
            _mockShelfRepo.Verify(r => r.AddActionAsync(It.Is<Shelf>(s => s.ActionId == 4 && s.Quantity == -10)), Times.Once);
        }
    }
}
