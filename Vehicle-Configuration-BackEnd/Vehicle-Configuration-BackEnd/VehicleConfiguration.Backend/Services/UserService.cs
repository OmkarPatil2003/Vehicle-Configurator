using VehicleConfiguration.Backend.Models;
using VehicleConfiguration.Backend.Repositories;

namespace VehicleConfiguration.Backend.Services;

public class UserService
{
    private readonly IUserRepository _repository;
    private readonly IEmailService _emailService;

    public UserService(IUserRepository repository, IEmailService emailService)
    {
        _repository = repository;
        _emailService = emailService;
    }

    public async Task<List<User>> GetAllRegistrationsAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<User> SaveRegistrationAsync(User user)
    {
        string regNo = "VCONF-" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        user.RegistrationNo = regNo;
        user.Role = "USER";
        
        // Save
        await _repository.SaveAsync(user);

        // Send Email (Safe)
        try
        {
            await _emailService.SendRegistrationEmailAsync(user);
        }
        catch (Exception ex)
        {
            // Log error but do not fail registration
            Console.WriteLine($"Failed to send registration email to {user.Email}: {ex.Message}");
        }

        return user;
    }
}
