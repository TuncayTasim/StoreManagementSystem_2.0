using Moq;
using StoreManagementSystem.API.Models;
using StoreManagementSystem.API.Repositories;
using StoreManagementSystem.API.Services;
using Xunit;

namespace StoreManagementSystem.Tests
{
    public class WarehouseServiceTests
    {
        private readonly Mock<IWarehouseRepository> _mockWarehouseRepo;
        private readonly Mock<IProductRepository> _mockProductRepo;
        private readonly Mock<IRejectionRepository> _mockRejectionRepo;
        private readonly WarehouseService _service;

        public WarehouseServiceTests()
        {
            _mockWarehouseRepo = new Mock<IWarehouseRepository>();
            _mockProductRepo = new Mock<IProductRepository>();
            _mockRejectionRepo = new Mock<IRejectionRepository>();
            _service = new WarehouseService(_mockWarehouseRepo.Object, _mockProductRepo.Object, _mockRejectionRepo.Object);
        }

        [Fact]
        public async Task RestockProductAsync_UpdatesQuantityAndAddsAction()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Apple", WarehouseQuantity = 10 };
            _mockProductRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

            // Act
            await _service.RestockProductAsync(1, 5, 1.0m, 30);

            // Assert
            Assert.Equal(15, product.WarehouseQuantity);
            _mockProductRepo.Verify(r => r.UpdateAsync(product), Times.Once);
            _mockWarehouseRepo.Verify(r => r.AddActionAsync(It.IsAny<Warehouse>()), Times.Once);
            _mockWarehouseRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }
    }
}
