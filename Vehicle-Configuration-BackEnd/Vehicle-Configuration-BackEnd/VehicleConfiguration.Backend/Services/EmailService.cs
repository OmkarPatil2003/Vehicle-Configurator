using MailKit.Net.Smtp;
using MimeKit;
using VehicleConfiguration.Backend.Configuration;
using VehicleConfiguration.Backend.Models;

namespace VehicleConfiguration.Backend.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly IRegistrationPdfService _pdfService;

    public EmailService(IConfiguration configuration, IRegistrationPdfService pdfService)
    {
        _configuration = configuration;
        _pdfService = pdfService;
    }

    public async Task SendRegistrationEmailAsync(User user)
    {
        var subject = "Registration Successful";
        var messageText = "Hello " + user.AuthName + ",\n\n" + "Your registration is successful.\n\n"
                        + "Registration No: " + user.RegistrationNo + "\n" + "Company: " + user.CompanyName + "\n\n"
                        + "Regards,\nVehicle Configurator Team";

        var pdfPath = _pdfService.GenerateRegistrationPdf(user);

        await SendEmailAsync(user.Email, subject, messageText, pdfPath);
    }

    public async Task SendInvoiceEmailAsync(string toEmail, InvoiceHeader invoice, List<InvoiceDetail> details)
    {
        // For Invoice, we assume Microservice handles it. User requirement says:
        // "call existing invoiceService microservice instead of re-implementing invoice logic"
        // But the legacy code had `EmailServiceImpl.sendInvoiceEmail`.
        // The Prompt says: "The invoice functionality must NOT be reimplemented. Instead, the .NET backend must call the existing invoiceService microservice".
        // HOWEVER, "Email sending must be implemented... This includes registration emails, forgot password emails, and invoice emails."
        // Wait. "invoice emails" in the "Email sending" requirement vs "invoice functionality must NOT be reimplemented".
        // The Java code had two flows: Legacy and Microservice.
        // `InvoiceController` called microservice.
        // `EmailServiceImpl` had `sendInvoiceEmail`.
        // Does the Microservice send the email?
        // Prompt says: "invoiceService, which will generate the invoice and send the email".
        // So I do NOT need to implement `SendInvoiceEmailAsync` logic here to send the actual email, 
        // unless I am supporting the legacy flow.
        // But the strict requirement says: "Email sending... This includes... invoice emails".
        // This is conflicting. "invoiceService... will generate the invoice and send the email".
        // I should probably follow "invoiceService... will... send the email".
        // So `SendInvoiceEmailAsync` here might be unused or just a wrapper/client call?
        // Actually, the prompt says "Email sending must be implemented... This includes... invoice emails."
        // Maybe it refers to a *notification* that invoice is ready? Or maybe I am misinterpreting.
        // "invoiceService, which will generate the invoice and send the email" -> This is very specific.
        // I will implement `SendInvoiceEmailAsync` but maybe leave it empty or callable if needed, 
        // but primarily rely on the Microservice call as per "invoice functionality must NOT be reimplemented".
        // Wait, if "invoice email" is listed in "Email sending must be implemented", maybe I should implement it?
        // But the prompt *also* says "Do NOT expose invoiceService directly to frontend".
        // And "invoice functionality must NOT be reimplemented... .NET backend must call... invoiceService... which will generate the invoice and send the email".
        // This implies .NET backend initiates the process, but the Microservice does the work.
        // So I should NOT implement SendInvoiceEmailAsync *locally* unless I cannot trust the microservice?
        // But "invoiceService... which will generate the invoice and send the email" -> Microservice sends it.
        // So I will implement `SendRegistrationEmailAsync` fully. For `SendInvoiceEmailAsync`, I might just log or throw NotImplemented because Microservice does it.
        // But "Email sending must be implemented... This includes... invoice emails." implies I *should* implement it?
        // Maybe the user wants me to port existing code fully even if unused by the new flow?
        // I'll implement it just in case, similar to Java, but the Controller will call Microservice.
        // The Java `EmailServiceImpl` sends `invoice_ID.pdf`.
        // I'll implement it but mark it as possibly redundant.
        
        // Actually, looking at `InvoiceController.java`:
        // It calls `http://localhost:9003/api/invoice/confirm`.
        // It does NOT call `emailService.sendInvoiceEmail`.
        // So `EmailServiceImpl.sendInvoiceEmail` in `v-conf` might be dead code or legacy.
        // I'll implement it anyway to satisfy the broad requirement "feature-by-feature parity with the Spring Boot backend".
        // If Spring Boot backend had it, I must have it.
        
        var path = _configuration["FileStorage:Path"] ?? "E:/vconf/files";
        var fileName = $"invoice_{invoice.Id}.pdf";
        var filePath = Path.Combine(path, fileName);
        
        // Logic to generate invoice PDF if expecting to send it?
        // Java had `InvoicePDFExporter` and logic.
        // I'll skip implementing `InvoicePDFExporter` fully unless required, as "invoice functionality must NOT be reimplemented".
        // I'll just implemented sending if file exists.
        
        if (File.Exists(filePath))
        {
             var subject = $"Invoice Generated - {invoice.Id}";
             var message = "Dear Customer,\n\n" + "Please find your invoice attached.\n\n"
                    + "Regards,\nVehicle Configurator Team";
             await SendEmailAsync(toEmail, subject, message, filePath);
        }
        else
        {
             // Log warning: Invoice PDF not found (likely handled by microservice).
             Console.WriteLine($"Invoice PDF not found for email: {filePath}");
        }
    }

    private async Task SendEmailAsync(string to, string subject, string body, string? attachmentPath)
    {
        var emailSettings = new EmailSettings
        {
            Host = _configuration["EmailSettings:Host"]!,
            Port = int.Parse(_configuration["EmailSettings:Port"]!),
            Username = _configuration["EmailSettings:Username"]!,
            Password = _configuration["EmailSettings:Password"]!,
            From = _configuration["EmailSettings:From"]!
        };

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Vehicle Configurator", emailSettings.From));
        message.To.Add(new MailboxAddress("", to));
        message.Subject = subject;

        var builder = new BodyBuilder { TextBody = body };

        if (!string.IsNullOrEmpty(attachmentPath) && File.Exists(attachmentPath))
        {
            builder.Attachments.Add(attachmentPath);
        }

        message.Body = builder.ToMessageBody();

        using (var client = new SmtpClient())
        {
            await client.ConnectAsync(emailSettings.Host, emailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(emailSettings.Username, emailSettings.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
