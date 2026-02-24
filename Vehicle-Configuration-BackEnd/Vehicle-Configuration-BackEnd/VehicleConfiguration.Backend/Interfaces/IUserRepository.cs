using System.Collections.Generic;
using System.Threading.Tasks;
using VehicleConfiguration.Backend.Models;

namespace VehicleConfiguration.Backend.Repositories;

public interface IUserRepository
{
    Task<User?> FindByUsernameAsync(string username);
    Task<User?> FindByEmailAsync(string email);
    Task<bool> ExistsByUsernameAsync(string username);
    Task<bool> ExistsByEmailAsync(string email);
    Task SaveAsync(User user);
    Task<List<User>> GetAllAsync();
}
