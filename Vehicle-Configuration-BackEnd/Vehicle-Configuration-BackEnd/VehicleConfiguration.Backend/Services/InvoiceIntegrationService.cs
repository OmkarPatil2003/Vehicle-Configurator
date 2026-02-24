using System.Text.Json;
using System.Text;
using VehicleConfiguration.Backend.DTOs;
using Microsoft.Extensions.Logging;

namespace VehicleConfiguration.Backend.Services;

public class InvoiceIntegrationService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<InvoiceIntegrationService> _logger;

    public InvoiceIntegrationService(HttpClient httpClient, IConfiguration configuration, ILogger<InvoiceIntegrationService> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<string> ConfirmOrderAsync(InvoiceRequestDTO dto)
    {
        var url = _configuration["Microservices:InvoiceServiceUrl"] ?? "http://localhost:9003/api/invoice/confirm";
        
        // Use CamelCase naming policy to ensure Java compatibility, though DTO attributes also help.
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        var json = JsonSerializer.Serialize(dto, options);
        
        _logger.LogInformation("Sending Order Confirmation to Invoice Service at {Url}. Payload: {Json}", url, json);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try 
        {
            var response = await _httpClient.PostAsync(url, content);
            
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                _logger.LogError("Invoice Service returned error: {StatusCode} - {Error}", response.StatusCode, error);
                throw new Exception($"Invoice Microservice failed: {response.StatusCode} - {error}");
            }

            var responseBody = await response.Content.ReadAsStringAsync();
             _logger.LogInformation("Invoice Service Success: {Response}", responseBody);
            return responseBody;
        }
        catch (Exception ex)
        {
             _logger.LogError(ex, "Failed to communicate with Invoice Service.");
             throw;
        }
    }
}
