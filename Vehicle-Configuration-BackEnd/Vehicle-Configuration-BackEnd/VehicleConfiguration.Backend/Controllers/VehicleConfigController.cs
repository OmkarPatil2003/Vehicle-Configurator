using Microsoft.AspNetCore.Mvc;
using VehicleConfiguration.Backend.Services;

namespace VehicleConfiguration.Backend.Controllers;

[Route("api/vehicle")]
[ApiController]
public class VehicleConfigController : ControllerBase
{
    private readonly VehicleService _manager;

    public VehicleConfigController(VehicleService manager)
    {
        _manager = manager;
    }

    [HttpGet("{modelId}/standard")]
    public async Task<IActionResult> GetStandardComponents(int modelId)
    {
        return Ok(await _manager.GetConfigurableComponentsAsync(modelId, "S"));
    }

    [HttpGet("{modelId}/interior")]
    public async Task<IActionResult> GetInteriorComponents(int modelId)
    {
        return Ok(await _manager.GetConfigurableComponentsAsync(modelId, "I"));
    }

    [HttpGet("{modelId}/exterior")]
    public async Task<IActionResult> GetExteriorComponents(int modelId)
    {
        return Ok(await _manager.GetConfigurableComponentsAsync(modelId, "E"));
    }

    [HttpGet("{modelId}/accessories")]
    public async Task<IActionResult> GetAccessoryComponents(int modelId)
    {
        return Ok(await _manager.GetConfigurableComponentsAsync(modelId, "C"));
    }
}
