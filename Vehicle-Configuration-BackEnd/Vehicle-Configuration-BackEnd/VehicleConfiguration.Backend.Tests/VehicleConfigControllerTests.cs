using Moq;
using Microsoft.AspNetCore.Mvc;
using VehicleConfiguration.Backend.Controllers;
using VehicleConfiguration.Backend.DTOs;
using VehicleConfiguration.Backend.Services;
using VehicleConfiguration.Backend.Repositories;
using VehicleConfiguration.Backend.Models; // Added
using Xunit;

namespace VehicleConfiguration.Backend.Tests;

public class VehicleConfigControllerTests
{
    private readonly Mock<IVehicleDetailRepository> _mockRepo;
    private readonly Mock<IModelRepository> _mockModelRepo;
    private readonly Mock<IComponentRepository> _mockCompRepo;
    private readonly Mock<IAlternateComponentRepository> _mockAltRepo;
    private readonly Mock<IDefaultConfigRepository> _mockDefaultRepo;

    private readonly VehicleService _service;
    private readonly VehicleConfigController _controller;

    public VehicleConfigControllerTests()
    {
        // Integration test style (using real Service with Mock Repo) or Unit Test Controller with Mock Service?
        // Controller uses Service concrete class. So I need to Mock Repo and use real Service.
        _mockRepo = new Mock<IVehicleDetailRepository>();
        _mockModelRepo = new Mock<IModelRepository>();
        _mockCompRepo = new Mock<IComponentRepository>();
        _mockAltRepo = new Mock<IAlternateComponentRepository>();
        _mockDefaultRepo = new Mock<IDefaultConfigRepository>();

        _service = new VehicleService(
            _mockRepo.Object, 
            _mockModelRepo.Object, 
            _mockCompRepo.Object, 
            _mockAltRepo.Object, 
            _mockDefaultRepo.Object
        );
        _controller = new VehicleConfigController(_service);
    }

    [Fact]
    public async Task GetStandardComponents_ReturnsOk()
    {
        // Arrange
        _mockRepo.Setup(r => r.FindConfigurableComponentsAsync(1, "S")).ReturnsAsync(new List<VehicleDetail>());

        // Act
        var result = await _controller.GetStandardComponents(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnList = Assert.IsType<List<ComponentDropdownDto>>(okResult.Value);
    }
}
