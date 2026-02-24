using Microsoft.AspNetCore.Mvc;
using VehicleConfiguration.Backend.Services;

/**
 * DefaultConfigController.cs
 * Purpose: Handles HTTP requests related to the default configuration of vehicles.
 * 
 * Endpoints:
 * - GET /api/default-config/{modelId}: Retrieves the full default configuration (price, components, etc.) needed for the "Default Configuration" page.
 * - GET /api/default-config/conf/{modelId}: Retrieves a simplified list of default component IDs and Names, primarily used by the frontend for initial state setup.
 */

namespace VehicleConfiguration.Backend.Controllers;

[Route("api/default-config")]
[ApiController]
public class DefaultConfigController : ControllerBase
{
    private readonly VehicleService _service;

    public DefaultConfigController(VehicleService service)
    {
        _service = service;
    }

    [HttpGet("{modelId}")]
    public async Task<IActionResult> GetDefaultConfig(int modelId, [FromQuery] int qty = 1)
    {
        try
        {
             var result = await _service.GetDefaultConfigurationAsync(modelId, qty);
             return Ok(result);
        }
        catch (Exception ex)
        {
             return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("conf/{modelId}")]
    public async Task<IActionResult> GetDefault(int modelId)
    {
        try
        {
            var result = await _service.GetDefaultConfigurationSimpleAsync(modelId);
            return Ok(result);
        }
         catch (Exception ex)
        {
             return StatusCode(500, ex.Message);
        }
    }
}
