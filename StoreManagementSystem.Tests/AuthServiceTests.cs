using Moq;
using StoreManagementSystem.API.Models;
using StoreManagementSystem.API.Repositories;
using StoreManagementSystem.API.Services;
using StoreManagementSystem.API.DTOs;
using Microsoft.Extensions.Configuration;
using Xunit;
using System.Threading.Tasks;

namespace StoreManagementSystem.Tests
{
    public class AuthServiceTests
    {
        private readonly Mock<IAuthRepository> _mockRepo;
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly AuthService _service;

        public AuthServiceTests()
        {
            _mockRepo = new Mock<IAuthRepository>();
            _mockConfig = new Mock<IConfiguration>();
            
            _mockConfig.Setup(c => c["Jwt:Key"]).Returns("TestKey1234567890123456789012345");
            _mockConfig.Setup(c => c["EmailSettings:Password"]).Returns("test");
            
            _service = new AuthService(_mockRepo.Object, _mockConfig.Object);
        }

        [Fact]
        public async Task RegisterAsync_AddsUserAndReturnsConfirmationToken()
        {
            // Arrange
            var dto = new UserRegisterDTO { UserName = "new", Email = "e@test.com", Password = "pw", FirstName = "F", LastName = "L", RoleId = 1 };
            _mockRepo.Setup(r => r.UserExistsAsync("new", "e@test.com")).ReturnsAsync(false);

            // Act
            var result = await _service.RegisterAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("CONFIRMATION_SENT", result.Token);
            _mockRepo.Verify(r => r.AddUserAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task LoginAsync_ReturnsTokenForActiveUser()
        {
            // Arrange
            var user = new User { UserName = "u", PasswordHash = BCrypt.Net.BCrypt.HashPassword("pw"), StatusId = 1, RoleId = 1 };
            _mockRepo.Setup(r => r.GetUserByUserNameAsync("u")).ReturnsAsync(user);

            // Act
            var result = await _service.LoginAsync(new UserLoginDTO { UserName = "u", Password = "pw" });

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.Token);
        }

        [Fact]
        public async Task LoginAsync_ThrowsForInactiveUser()
        {
            // Arrange
            var user = new User { UserName = "u", PasswordHash = BCrypt.Net.BCrypt.HashPassword("pw"), StatusId = 2 };
            _mockRepo.Setup(r => r.GetUserByUserNameAsync("u")).ReturnsAsync(user);

            // Act & Assert
            await Assert.ThrowsAsync<System.Exception>(() => _service.LoginAsync(new UserLoginDTO { UserName = "u", Password = "pw" }));
        }
    }
}

