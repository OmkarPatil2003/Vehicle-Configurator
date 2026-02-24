using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VehicleConfiguration.Backend.DTOs;
using VehicleConfiguration.Backend.Services;
using VehicleConfiguration.Backend.Repositories;

namespace VehicleConfiguration.Backend.Controllers;

[Route("api/invoice")]
[ApiController]
public class InvoiceController : ControllerBase
{
    private readonly InvoiceIntegrationService _service;
    private readonly IUserRepository _userRepo;

    public InvoiceController(InvoiceIntegrationService service, IUserRepository userRepo)
    {
        _service = service;
        _userRepo = userRepo;
    }

    [HttpPost("confirm")]
    [Authorize] // Requires Authentication
    public async Task<IActionResult> ConfirmOrder([FromBody] InvoiceRequestDTO dto)
    {
        // Get Current User from Token
        var username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value 
                        ?? User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

        if (string.IsNullOrEmpty(username))
        {
            return Unauthorized("User not identified");
        }

        var user = await _userRepo.FindByUsernameAsync(username);
        if (user == null)
        {
             return Unauthorized("User not found");
        }

        // Force UserID from Auth
        dto.UserId = user.Id;

        // Call Microservice
        try
        {
             var response = await _service.ConfirmOrderAsync(dto);
             return Ok(response);
        }
        catch (Exception ex)
        {
             return StatusCode(500, $"Error Generating Invoice via Microservice: {ex.Message}");
        }
    }
}
