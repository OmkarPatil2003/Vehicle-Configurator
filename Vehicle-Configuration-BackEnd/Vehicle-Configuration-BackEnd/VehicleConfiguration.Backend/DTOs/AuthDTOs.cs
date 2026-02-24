namespace VehicleConfiguration.Backend.DTOs;

public class LoginRequest
{
    public string? Username { get; set; }
    public string? Password { get; set; }
}

public class ForgotPasswordDto
{
    public string? Email { get; set; }
    public string? NewPassword { get; set; }
}
