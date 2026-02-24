using Microsoft.AspNetCore.Mvc;
using VehicleConfiguration.Backend.DTOs;
using VehicleConfiguration.Backend.Services;

namespace VehicleConfiguration.Backend.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var token = await _authService.LoginAsync(request);
            return Ok(token); // Java returns just the token string, typically. Or use object. 
            // Java: return ResponseEntity.ok(token); -> "eyJ..."
        }
        catch (Exception ex)
        {
            return Unauthorized(ex.Message);
        }
    }

    [HttpPut("forgot")]
    public async Task<IActionResult> Forgot([FromBody] ForgotPasswordDto dto)
    {
        try
        {
            if (string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.NewPassword))
            {
                 return BadRequest("Email and New Password are required");
            }

            await _authService.ChangePasswordAsync(dto.Email, dto.NewPassword);
            return Ok("Password updated successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("oauth")]
    public IActionResult OAuth()
    {
         return StatusCode(501, "OAuth flow not yet implemented. Please use standard login.");
    }
}
