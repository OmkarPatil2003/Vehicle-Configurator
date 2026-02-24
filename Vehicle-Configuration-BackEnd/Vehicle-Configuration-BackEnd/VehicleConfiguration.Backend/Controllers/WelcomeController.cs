using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VehicleConfiguration.Backend.Services;

namespace VehicleConfiguration.Backend.Controllers;

[Route("api/welcome")]
[ApiController]
public class WelcomeController : ControllerBase
{
    private readonly WelcomeService _service;

    public WelcomeController(WelcomeService service)
    {
        _service = service;
    }

    [HttpGet("segments")]
    public async Task<IActionResult> GetSegments()
    {
        return Ok(await _service.GetAllSegmentsAsync());
    }

    [HttpGet("manufacturers/{segId}")]
    public async Task<IActionResult> GetManufacturers(int segId)
    {
        return Ok(await _service.GetManufacturersBySegmentAsync(segId));
    }

    [HttpGet("models")]
    public async Task<IActionResult> GetModels([FromQuery] int segId, [FromQuery] int mfgId)
    {
        return Ok(await _service.GetModelsAsync(segId, mfgId));
    }
}
