using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using VehicleConfiguration.Backend.Data;
using VehicleConfiguration.Backend.Middleware;
using VehicleConfiguration.Backend.Models;
using VehicleConfiguration.Backend.Repositories;
using VehicleConfiguration.Backend.Services;

/**
 * Program.cs
 * Purpose: The entry point of the ASP.NET Core application.
 * 
 * Responsibilities:
 * - Configures the dependency injection (DI) container (Services, Repositories).
 * - Sets up the database connection (MySQL).
 * - Configures Authentication and Authorization (JWT, Google/Facebook OAuth).
 * - Sets up the HTTP request pipeline (Middleware), including CORS, Serilog, and Internationalization (I18n).
 */

var builder = WebApplication.CreateBuilder(args);

// 1. Logging (Serilog)
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// 2. Database (MySQL via Pomelo)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
           .EnableSensitiveDataLogging()
           .EnableDetailedErrors());

// 2.1 CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// 2.2 Localization
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// 3. Authentication (JWT + Cookies for OAuth)
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie() // Adds the "Cookies" scheme
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true, // We set audience in appsettings.json
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    })
    .AddGoogle(options =>
    {
        options.ClientId = "405893800462-639engko6lq997oq9c89kliq5jdjmo0i.apps.googleusercontent.com";
        options.ClientSecret = "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";
        options.CallbackPath = "/login/oauth2/code/google"; // Match Spring Boot default
    })
    .AddFacebook(options =>
    {
        options.ClientId = "912922921478380";
        options.ClientSecret = "7bddced47b060cc0ab391b179d317d85";
        options.CallbackPath = "/login/oauth2/code/facebook"; // Match Spring Boot default
    });

builder.Services.AddScoped<IUserRepository, VehicleConfiguration.Backend.Repositories.Impl.UserRepository>();
builder.Services.AddScoped<IAuthService, VehicleConfiguration.Backend.Services.AuthService>();
builder.Services.AddScoped<IRegistrationPdfService, VehicleConfiguration.Backend.Services.RegistrationPdfService>();
builder.Services.AddScoped<IEmailService, VehicleConfiguration.Backend.Services.EmailService>();

builder.Services.AddScoped<ISegmentRepository, VehicleConfiguration.Backend.Repositories.Impl.SegmentRepository>();
builder.Services.AddScoped<IManufacturerRepository, VehicleConfiguration.Backend.Repositories.Impl.ManufacturerRepository>();
builder.Services.AddScoped<IModelRepository, VehicleConfiguration.Backend.Repositories.Impl.ModelRepository>();
builder.Services.AddScoped<IComponentRepository, VehicleConfiguration.Backend.Repositories.Impl.ComponentRepository>();
builder.Services.AddScoped<IDefaultConfigRepository, VehicleConfiguration.Backend.Repositories.Impl.DefaultConfigRepository>();
builder.Services.AddScoped<IAlternateComponentRepository, VehicleConfiguration.Backend.Repositories.Impl.AlternateComponentRepository>();
builder.Services.AddScoped<VehicleConfiguration.Backend.Services.WelcomeService>(); // Registered as class

builder.Services.AddScoped<IVehicleDetailRepository, VehicleConfiguration.Backend.Repositories.Impl.VehicleDetailRepository>();
builder.Services.AddScoped<VehicleConfiguration.Backend.Services.VehicleService>();
builder.Services.AddScoped<VehicleConfiguration.Backend.Services.UserService>();

builder.Services.AddHttpClient<VehicleConfiguration.Backend.Services.InvoiceIntegrationService>(); // Registers service and HttpClient

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen(); // Optional, good for testing

var app = builder.Build();

// Middleware Pipeline
app.UseMiddleware<ExceptionMiddleware>();

app.UseSerilogRequestLogging();

var supportedCultures = new[] { "en", "fr", "mr" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture("en")
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
