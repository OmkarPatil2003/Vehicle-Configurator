using Microsoft.AspNetCore.Mvc;
using VehicleConfiguration.Backend.Models;
using VehicleConfiguration.Backend.Services;

namespace VehicleConfiguration.Backend.Controllers;

[Route("api/registration")]
[ApiController]
public class RegisterController : ControllerBase
{
    private readonly UserService _service;

    public RegisterController(UserService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllRegistrationsAsync());
    }

    [HttpPost]
    public async Task<IActionResult> Save([FromBody] User user)
    {
        return Ok(await _service.SaveRegistrationAsync(user));
    }
}
