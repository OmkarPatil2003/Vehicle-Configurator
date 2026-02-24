
using System.Text.Json.Serialization;

namespace VehicleConfiguration.Backend.DTOs;

/**
 * I18nDTOs.cs
 * Purpose: Defines DTOs for Internationalization (I18n) responses.
 * 
 * Responsibilities:
 * - Maps localized strings from resource files to deeply structured JSON for the frontend.
 * - Covers all major UI sections: Navbar, Footer, Welcome, Login, Contact, Features, and Registration.
 * - Uses [JsonPropertyName] to match the exact keys expected by the frontend's I18nContext.
 */

public class NavbarI18nDTO
{
    [JsonPropertyName("home")] public string? Home { get; set; }
    [JsonPropertyName("aboutUs")] public string? AboutUs { get; set; }
    [JsonPropertyName("contactUs")] public string? ContactUs { get; set; }
    [JsonPropertyName("signIn")] public string? SignIn { get; set; }
    [JsonPropertyName("registerCompany")] public string? RegisterCompany { get; set; }
    [JsonPropertyName("dashboard")] public string? Dashboard { get; set; }
    [JsonPropertyName("logout")] public string? Logout { get; set; }
}

public class FooterI18nDTO
{
    [JsonPropertyName("sitemap")] public string? Sitemap { get; set; }
    [JsonPropertyName("privacy")] public string? Privacy { get; set; }
    [JsonPropertyName("terms")] public string? Terms { get; set; }
    [JsonPropertyName("contact")] public string? Contact { get; set; }
    [JsonPropertyName("support")] public string? Support { get; set; }
}

public class WelcomeI18nDTO
{
    [JsonPropertyName("title")] public string? Title { get; set; }
    [JsonPropertyName("subtitle")] public string? Subtitle { get; set; }
}

public class LoginI18nDTO
{
    [JsonPropertyName("title")] public string? Title { get; set; }
    [JsonPropertyName("subtitle")] public string? Subtitle { get; set; }
    [JsonPropertyName("username")] public string? Username { get; set; }
    [JsonPropertyName("usernamePlaceholder")] public string? UsernamePlaceholder { get; set; }
    [JsonPropertyName("password")] public string? Password { get; set; }
    [JsonPropertyName("passwordPlaceholder")] public string? PasswordPlaceholder { get; set; }
    [JsonPropertyName("forgotPassword")] public string? ForgotPassword { get; set; }
    [JsonPropertyName("signIn")] public string? SignIn { get; set; }
    [JsonPropertyName("orContinueWith")] public string? OrContinueWith { get; set; }
}

public class ContactI18nDTO
{
    [JsonPropertyName("title")] public string? Title { get; set; }
    [JsonPropertyName("subtitle")] public string? Subtitle { get; set; }
    [JsonPropertyName("addressLabel")] public string? AddressLabel { get; set; }
    [JsonPropertyName("addressValue")] public string? AddressValue { get; set; }
    [JsonPropertyName("emailLabel")] public string? EmailLabel { get; set; }
    [JsonPropertyName("phoneLabel")] public string? PhoneLabel { get; set; }
    [JsonPropertyName("formTitle")] public string? FormTitle { get; set; }
    [JsonPropertyName("formSubtitle")] public string? FormSubtitle { get; set; }
    [JsonPropertyName("namePlaceholder")] public string? NamePlaceholder { get; set; }
    [JsonPropertyName("emailPlaceholder")] public string? EmailPlaceholder { get; set; }
    [JsonPropertyName("messagePlaceholder")] public string? MessagePlaceholder { get; set; }
    [JsonPropertyName("sendButton")] public string? SendButton { get; set; }
}

public class FeaturesI18nDTO
{
    [JsonPropertyName("vehicleSelectionTitle")] public string? VehicleSelectionTitle { get; set; }
    [JsonPropertyName("vehicleSelectionDesc")] public string? VehicleSelectionDesc { get; set; }
    [JsonPropertyName("customConfigTitle")] public string? CustomConfigTitle { get; set; }
    [JsonPropertyName("customConfigDesc")] public string? CustomConfigDesc { get; set; }
    [JsonPropertyName("invoiceGenTitle")] public string? InvoiceGenTitle { get; set; }
    [JsonPropertyName("invoiceGenDesc")] public string? InvoiceGenDesc { get; set; }
    [JsonPropertyName("securePlatformTitle")] public string? SecurePlatformTitle { get; set; }
    [JsonPropertyName("securePlatformDesc")] public string? SecurePlatformDesc { get; set; }
}

public class RegistrationI18nDTO
{
    [JsonPropertyName("title")] public string? Title { get; set; }
    [JsonPropertyName("subtitle")] public string? Subtitle { get; set; }
    [JsonPropertyName("companyName")] public string? CompanyName { get; set; }
    [JsonPropertyName("companyNamePlaceholder")] public string? CompanyNamePlaceholder { get; set; }
    [JsonPropertyName("registrationNo")] public string? RegistrationNo { get; set; }
    [JsonPropertyName("registrationNoPlaceholder")] public string? RegistrationNoPlaceholder { get; set; }
    [JsonPropertyName("holdingType")] public string? HoldingType { get; set; }
    [JsonPropertyName("addressLine1")] public string? AddressLine1 { get; set; }
    [JsonPropertyName("addressLine1Placeholder")] public string? AddressLine1Placeholder { get; set; }
    [JsonPropertyName("addressLine2")] public string? AddressLine2 { get; set; }
    [JsonPropertyName("addressLine2Placeholder")] public string? AddressLine2Placeholder { get; set; }
    [JsonPropertyName("city")] public string? City { get; set; }
    [JsonPropertyName("state")] public string? State { get; set; }
    [JsonPropertyName("pin")] public string? Pin { get; set; }
    [JsonPropertyName("authName")] public string? AuthName { get; set; }
    [JsonPropertyName("authTel")] public string? AuthTel { get; set; }
    [JsonPropertyName("email")] public string? Email { get; set; }
    [JsonPropertyName("username")] public string? Username { get; set; }
    [JsonPropertyName("password")] public string? Password { get; set; }
    [JsonPropertyName("confirmPassword")] public string? ConfirmPassword { get; set; }
    [JsonPropertyName("registerButton")] public string? RegisterButton { get; set; }
    [JsonPropertyName("alreadyHaveAccount")] public string? AlreadyHaveAccount { get; set; }
    [JsonPropertyName("signInLink")] public string? SignInLink { get; set; }
}
