using Moq;
using Microsoft.Extensions.Configuration;
using VehicleConfiguration.Backend.DTOs;
using VehicleConfiguration.Backend.Models;
using VehicleConfiguration.Backend.Repositories;
using VehicleConfiguration.Backend.Services;
using Xunit;

namespace VehicleConfiguration.Backend.Tests;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _mockRepo;
    private readonly Mock<IConfiguration> _mockConfig;
    private readonly AuthService _service;

    public AuthServiceTests()
    {
        _mockRepo = new Mock<IUserRepository>();
        _mockConfig = new Mock<IConfiguration>();
        _mockConfig.Setup(c => c["Jwt:Key"]).Returns("MySecretKeyForJwt123456789012345");
        _mockConfig.Setup(c => c["Jwt:Issuer"]).Returns("VConfIssuer");
        _mockConfig.Setup(c => c["Jwt:Audience"]).Returns("VConfAudience");

        _service = new AuthService(_mockRepo.Object, _mockConfig.Object);
    }

    [Fact]
    public async Task LoginAsync_ValidCredentials_ReturnsToken()
    {
        // Arrange
        var user = new User { Username = "testuser", Password = "password123", IsBlocked = false };
        _mockRepo.Setup(r => r.FindByUsernameAsync("testuser")).ReturnsAsync(user);

        var request = new LoginRequest { Username = "testuser", Password = "password123" };

        // Act
        var token = await _service.LoginAsync(request);

        // Assert
        Assert.NotNull(token);
        Assert.NotEmpty(token);
    }

    [Fact]
    public async Task LoginAsync_InvalidPassword_ThrowsException()
    {
        // Arrange
        var user = new User { Username = "testuser", Password = "password123", IsBlocked = false, FailedAttempts = 0 };
        _mockRepo.Setup(r => r.FindByUsernameAsync("testuser")).ReturnsAsync(user);

        var request = new LoginRequest { Username = "testuser", Password = "wrongpassword" };

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.LoginAsync(request));
        
        // Verify FailedAttempts incremented
        // Note: Since user object is reference, verification is tricky if not saved.
        // We verify SaveAsync was called.
        _mockRepo.Verify(r => r.SaveAsync(It.Is<User>(u => u.FailedAttempts == 1)), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_BlockedUser_ThrowsException()
    {
        // Arrange
        var user = new User { Username = "testuser", Password = "password123", IsBlocked = true };
        _mockRepo.Setup(r => r.FindByUsernameAsync("testuser")).ReturnsAsync(user);

        var request = new LoginRequest { Username = "testuser", Password = "password123" };

        // Act & Assert
        var ex = await Assert.ThrowsAsync<Exception>(() => _service.LoginAsync(request));
        Assert.Equal("User is Blocked", ex.Message);
    }
}
