using VehicleConfiguration.Backend.Models;

namespace VehicleConfiguration.Backend.Services;

public interface IEmailService
{
    Task SendRegistrationEmailAsync(User user);
    Task SendInvoiceEmailAsync(string toEmail, InvoiceHeader invoice, List<InvoiceDetail> details); // Simplified signature
}
