using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using VehicleConfiguration.Backend.DTOs;
using VehicleConfiguration.Backend.Services;
using Xunit;

namespace VehicleConfiguration.Backend.Tests;

public class InvoiceIntegrationServiceTests
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly HttpClient _httpClient;
    private readonly Mock<IConfiguration> _mockConfig;
    private readonly InvoiceIntegrationService _service;

    public InvoiceIntegrationServiceTests()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_mockHttpMessageHandler.Object);
        
        _mockConfig = new Mock<IConfiguration>();
        _mockConfig.Setup(c => c["Microservices:InvoiceServiceUrl"]).Returns("http://test-url/api/invoice/confirm");

        _service = new InvoiceIntegrationService(_httpClient, _mockConfig.Object);
    }

    [Fact]
    public async Task ConfirmOrderAsync_SuccessfulRequest_ReturnsSuccessMessage()
    {
        // Arrange
        var requestDto = new InvoiceRequestDTO { UserId = 1, ModelId = 1, Qty = 1, CustomerDetail = "Test" };
        var expectedResponse = "Invoice Generated";

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(expectedResponse)
            });

        // Act
        var result = await _service.ConfirmOrderAsync(requestDto);

        // Assert
        Assert.Equal(expectedResponse, result);
    }

    [Fact]
    public async Task ConfirmOrderAsync_FailedRequest_ThrowsException()
    {
        // Arrange
        var requestDto = new InvoiceRequestDTO { UserId = 1 };
        var errorResponse = "Service Unavailable";

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent(errorResponse)
            });

        // Act & Assert
        var ex = await Assert.ThrowsAsync<Exception>(() => _service.ConfirmOrderAsync(requestDto));
        Assert.Contains("Invoice Microservice failed", ex.Message);
        Assert.Contains("InternalServerError", ex.Message);
    }
}
