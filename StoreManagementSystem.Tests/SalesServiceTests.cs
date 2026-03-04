using Moq;
using StoreManagementSystem.API.Models;
using StoreManagementSystem.API.Repositories;
using StoreManagementSystem.API.Services;
using Xunit;

namespace StoreManagementSystem.Tests
{
    public class SalesServiceTests
    {
        private readonly Mock<ISalesRepository> _mockSalesRepo;
        private readonly Mock<IShelfRepository> _mockShelfRepo;
        private readonly Mock<IProductRepository> _mockProductRepo;
        private readonly SalesService _service;

        public SalesServiceTests()
        {
            _mockSalesRepo = new Mock<ISalesRepository>();
            _mockShelfRepo = new Mock<IShelfRepository>();
            _mockProductRepo = new Mock<IProductRepository>();
            _service = new SalesService(_mockSalesRepo.Object, _mockShelfRepo.Object, _mockProductRepo.Object);
        }

        [Fact]
        public async Task ProcessSaleAsync_RecordsSaleAndUpdatesProduct()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Apple", ShelfQuantity = 10 };
            var shelfBatch = new Shelf { Id = 1, ProductId = 1, CurrentQuantity = 10, RestockDetails = new ShelfRestock { PriceSell = 2.0m } };

            _mockProductRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);
            _mockShelfRepo.Setup(r => r.GetAvailableBatchesAsync(1)).ReturnsAsync(new List<Shelf> { shelfBatch });

            // Act
            await _service.ProcessSaleAsync(1, 3, "Cash");

            // Assert
            Assert.Equal(7, product.ShelfQuantity);
            Assert.Equal(7, shelfBatch.CurrentQuantity);
            _mockSalesRepo.Verify(r => r.AddSaleAsync(It.Is<Sale>(s => s.QuantitySold == 3 && s.PriceSold == 2.0m && s.PaymentMethod == "Cash")), Times.Once);
            _mockShelfRepo.Verify(r => r.AddActionAsync(It.Is<Shelf>(s => s.Quantity == -3 && s.ActionId == 3)), Times.Once);
            _mockProductRepo.Verify(r => r.UpdateAsync(product), Times.Once);
        }
    }
}
