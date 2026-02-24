using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using VehicleConfiguration.Backend.Models;

namespace VehicleConfiguration.Backend.Services;

public class RegistrationPdfService : IRegistrationPdfService
{
    private readonly IConfiguration _configuration;

    public RegistrationPdfService(IConfiguration configuration)
    {
        _configuration = configuration;
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public string GenerateRegistrationPdf(User user)
    {
        var path = _configuration["FileStorage:Path"] ?? "E:/vconf/files";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        var fileName = $"registration_{user.RegistrationNo}.pdf";
        var filePath = Path.Combine(path, fileName);

        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(20);
                page.Size(PageSizes.A4);
                page.DefaultTextStyle(x => x.FontSize(11).FontFamily(Fonts.Arial));

                page.Content()
                    .Column(column =>
                    {
                        // Title
                        column.Item().Text("REGISTRATION DETAILS").Bold().FontSize(16).AlignCenter();
                        column.Item().PaddingBottom(15);

                        // Company Info
                        column.Item().Text("Company Information").Bold().FontSize(12);
                        column.Item().PaddingBottom(5);

                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(7);
                            });

                            AddRow(table, "Registration No", user.RegistrationNo);
                            AddRow(table, "Company Name", user.CompanyName);
                            AddRow(table, "Username", user.Username);
                            AddRow(table, "Holding Type", user.HoldingType);
                            AddRow(table, "ST No", user.CompanyStNo);
                            AddRow(table, "VAT No", user.CompanyVatNo);
                            AddRow(table, "PAN", user.TaxPan);
                        });

                        column.Item().PaddingBottom(15);

                        // Authorized Person
                        column.Item().Text("Authorized Person Details").Bold().FontSize(12);
                        column.Item().PaddingBottom(5);

                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(7);
                            });

                            AddRow(table, "Name", user.AuthName);
                            AddRow(table, "Designation", user.Designation);
                            AddRow(table, "Email", user.Email);
                            AddRow(table, "Phone", user.AuthTel);
                        });
                    });
            });
        })
        .GeneratePdf(filePath);

        return filePath;
    }

    private void AddRow(TableDescriptor table, string key, string? value)
    {
        table.Cell().Text(key).Bold();
        table.Cell().Text(value ?? "-");
    }
}
