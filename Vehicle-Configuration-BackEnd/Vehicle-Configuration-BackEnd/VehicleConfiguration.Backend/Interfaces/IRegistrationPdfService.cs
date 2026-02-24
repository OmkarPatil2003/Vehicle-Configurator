using VehicleConfiguration.Backend.Models;

namespace VehicleConfiguration.Backend.Services;

public interface IRegistrationPdfService
{
    string GenerateRegistrationPdf(User user);
}
