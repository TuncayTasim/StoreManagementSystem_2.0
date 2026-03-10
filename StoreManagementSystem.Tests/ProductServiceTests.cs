using Moq;
using StoreManagementSystem.API.Models;
using StoreManagementSystem.API.Repositories;
using StoreManagementSystem.API.Services;
using Xunit;

namespace StoreManagementSystem.Tests
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _mockRepo;
        private readonly ProductService _service;

        public ProductServiceTests()
        {
            _mockRepo = new Mock<IProductRepository>();
            _service = new ProductService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAllProductsAsync_ReturnsAllProducts()
        {
            // Arrange
            var products = new List<Product> { new Product { Id = 1, Name = "Test" } };
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(products);

            // Act
            var result = await _service.GetAllProductsAsync();

            // Assert
            Assert.Single(result);
            Assert.Equal("Test", result.First().Name);
        }

        [Fact]
        public async Task CreateProductAsync_CallsAddAndSave()
        {
            // Arrange
            var product = new Product { Name = "New" };

            // Act
            await _service.CreateProductAsync(product);

            // Assert
            Assert.NotNull(product.Barcode);
            Assert.Equal(13, product.Barcode.Length);
            _mockRepo.Verify(r => r.AddAsync(product), Times.Once);
            _mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }
    }
}

