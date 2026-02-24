using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using VehicleConfiguration.Backend.DTOs;
using VehicleConfiguration.Backend.Models;
using VehicleConfiguration.Backend.Repositories;

namespace VehicleConfiguration.Backend.Services;

public interface IAuthService
{
    Task<string> LoginAsync(LoginRequest request);
    Task ChangePasswordAsync(string email, string newPassword);
    string GenerateToken(string username);
    string GenerateTokenForOAuth(string email);
}

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<string> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.FindByUsernameAsync(request.Username ?? "");
        if (user == null)
        {
            throw new Exception("Invalid Username and Password");
        }

        if (user.IsBlocked)
        {
            throw new Exception("User is Blocked");
        }

        if (request.Password != user.Password) // Plain text check as per legacy
        {
            user.FailedAttempts++;
            if (user.FailedAttempts >= 3)
            {
                user.IsBlocked = true;
            }
            await _userRepository.SaveAsync(user);
            throw new Exception("Invalid password");
        }

        user.FailedAttempts = 0;
        await _userRepository.SaveAsync(user);

        return GenerateToken(user.Username!);
    }

    public async Task ChangePasswordAsync(string email, string newPassword)
    {
        var user = await _userRepository.FindByEmailAsync(email);
        if (user == null)
        {
            throw new Exception("Invalid email address");
        }

        user.Password = newPassword;
        await _userRepository.SaveAsync(user);
    }

    public string GenerateToken(string username)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    public string GenerateTokenForOAuth(string email)
    {
        // For OAuth, we trust the email is verified by the provider.
        // We treat the email as the username for token generation.
        return GenerateToken(email);
    }
}
