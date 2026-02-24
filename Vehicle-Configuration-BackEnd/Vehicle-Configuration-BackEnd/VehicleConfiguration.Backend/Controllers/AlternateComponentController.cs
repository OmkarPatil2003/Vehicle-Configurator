using Microsoft.AspNetCore.Mvc;
using VehicleConfiguration.Backend.DTOs;
using VehicleConfiguration.Backend.Services;

namespace VehicleConfiguration.Backend.Controllers;

[Route("api/alternate-component")]
[ApiController]
public class AlternateComponentController : ControllerBase
{
    private readonly VehicleService _service;

    public AlternateComponentController(VehicleService service)
    {
        _service = service;
    }

    [HttpPost("save")]
    public async Task<IActionResult> SaveAlternateComponent([FromBody] AlternateComponentSaveDTO dto)
    {
        try
        {
            await _service.SaveAlternateComponentsAsync(dto);
            return Ok("Alternate components saved successfully");
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
