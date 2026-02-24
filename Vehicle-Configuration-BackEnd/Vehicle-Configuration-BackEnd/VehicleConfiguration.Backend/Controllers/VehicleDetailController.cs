using Microsoft.AspNetCore.Mvc;
using VehicleConfiguration.Backend.DTOs;
using VehicleConfiguration.Backend.Services;

namespace VehicleConfiguration.Backend.Controllers;

[Route("api/vehicaldetail")]
[ApiController]
public class VehicleDetailController : ControllerBase
{
    private readonly VehicleService _service;

    public VehicleDetailController(VehicleService service)
    {
        _service = service;
    }

    [HttpPost("config")]
    public async Task<IActionResult> ConfigurableComponent([FromBody] IsConfigurationDTO dto)
    {
        try
        {
            var result = await _service.IsConfigurableAsync(dto.ModelId, dto.CompType ?? "");
            return Ok(result);
        }
         catch (Exception ex)
        {
             return StatusCode(500, ex.Message);
        }
    }
}
