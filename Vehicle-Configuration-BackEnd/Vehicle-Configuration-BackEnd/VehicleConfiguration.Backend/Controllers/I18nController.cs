using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using VehicleConfiguration.Backend.DTOs;
using VehicleConfiguration.Backend.Services;

namespace VehicleConfiguration.Backend.Controllers;

/**
 * I18nController.cs
 * Purpose: Serves localized text strings to the frontend.
 * 
 * Key Features:
 * - Supports English ('en'), French ('fr'), and Marathi ('mr').
 * - returns DTOs (e.g., NavbarI18nDTO, FooterI18nDTO) populated with strings from the .resx resource files.
 * - Used by the frontend to dynamically update the UI language without page reloads (if using client-side state) or on route changes.
 */
[Route("api/i18n")]
[ApiController]
public class I18nController : ControllerBase
{
    private readonly IStringLocalizer<SharedResource> _localizer;

    public I18nController(IStringLocalizer<SharedResource> localizer)
    {
        _localizer = localizer;
    }

    [HttpGet("footer")]
    public ActionResult<FooterI18nDTO> GetFooter()
    {
        return Ok(new FooterI18nDTO
        {
            Sitemap = _localizer["footer.sitemap"],
            Privacy = _localizer["footer.privacy"],
            Terms = _localizer["footer.terms"],
            Contact = _localizer["footer.contact"],
            Support = _localizer["footer.support"]
        });
    }

    [HttpGet("welcome")]
    public ActionResult<WelcomeI18nDTO> GetWelcome()
    {
        return Ok(new WelcomeI18nDTO
        {
            Title = _localizer["welcome.title"],
            Subtitle = _localizer["welcome.subtitle"]
        });
    }

    [HttpGet("login")]
    public ActionResult<LoginI18nDTO> GetLogin()
    {
        return Ok(new LoginI18nDTO
        {
            Title = _localizer["login.title"],
            Subtitle = _localizer["login.subtitle"],
            Username = _localizer["login.username"],
            UsernamePlaceholder = _localizer["login.usernamePlaceholder"],
            Password = _localizer["login.password"],
            PasswordPlaceholder = _localizer["login.passwordPlaceholder"],
            ForgotPassword = _localizer["login.forgotPassword"],
            SignIn = _localizer["login.signIn"],
            OrContinueWith = _localizer["login.orContinueWith"]
        });
    }

    [HttpGet("navbar")]
    public ActionResult<NavbarI18nDTO> GetNavbar()
    {
        return Ok(new NavbarI18nDTO
        {
            Home = _localizer["navbar.home"],
            AboutUs = _localizer["navbar.aboutUs"],
            ContactUs = _localizer["navbar.contactUs"],
            SignIn = _localizer["navbar.signIn"],
            RegisterCompany = _localizer["navbar.registerCompany"],
            Dashboard = _localizer["navbar.dashboard"],
            Logout = _localizer["navbar.logout"]
        });
    }

    [HttpGet("contact")]
    public ActionResult<ContactI18nDTO> GetContact()
    {
        return Ok(new ContactI18nDTO
        {
            Title = _localizer["contact.title"],
            Subtitle = _localizer["contact.subtitle"],
            AddressLabel = _localizer["contact.addressLabel"],
            AddressValue = _localizer["contact.addressValue"],
            EmailLabel = _localizer["contact.emailLabel"],
            PhoneLabel = _localizer["contact.phoneLabel"],
            FormTitle = _localizer["contact.formTitle"],
            FormSubtitle = _localizer["contact.formSubtitle"],
            NamePlaceholder = _localizer["contact.namePlaceholder"],
            EmailPlaceholder = _localizer["contact.emailPlaceholder"],
            MessagePlaceholder = _localizer["contact.messagePlaceholder"],
            SendButton = _localizer["contact.sendButton"]
        });
    }

    [HttpGet("features")]
    public ActionResult<FeaturesI18nDTO> GetFeatures()
    {
        return Ok(new FeaturesI18nDTO
        {
            VehicleSelectionTitle = _localizer["features.vehicleSelection.title"],
            VehicleSelectionDesc = _localizer["features.vehicleSelection.desc"],
            CustomConfigTitle = _localizer["features.customConfig.title"],
            CustomConfigDesc = _localizer["features.customConfig.desc"],
            InvoiceGenTitle = _localizer["features.invoiceGen.title"],
            InvoiceGenDesc = _localizer["features.invoiceGen.desc"],
            SecurePlatformTitle = _localizer["features.securePlatform.title"],
            SecurePlatformDesc = _localizer["features.securePlatform.desc"]
        });
    }

    [HttpGet("registration")]
    public ActionResult<RegistrationI18nDTO> GetRegistration()
    {
        return Ok(new RegistrationI18nDTO
        {
            Title = _localizer["registration.title"],
            Subtitle = _localizer["registration.subtitle"],
            CompanyName = _localizer["registration.companyName"],
            CompanyNamePlaceholder = _localizer["registration.companyNamePlaceholder"],
            RegistrationNo = _localizer["registration.registrationNo"],
            RegistrationNoPlaceholder = _localizer["registration.registrationNoPlaceholder"],
            HoldingType = _localizer["registration.holdingType"],
            AddressLine1 = _localizer["registration.addressLine1"],
            AddressLine1Placeholder = _localizer["registration.addressLine1Placeholder"],
            AddressLine2 = _localizer["registration.addressLine2"],
            AddressLine2Placeholder = _localizer["registration.addressLine2Placeholder"],
            City = _localizer["registration.city"],
            State = _localizer["registration.state"],
            Pin = _localizer["registration.pin"],
            AuthName = _localizer["registration.authName"],
            AuthTel = _localizer["registration.authTel"],
            Email = _localizer["registration.email"],
            Username = _localizer["registration.username"],
            Password = _localizer["registration.password"],
            ConfirmPassword = _localizer["registration.confirmPassword"],
            RegisterButton = _localizer["registration.registerButton"],
            AlreadyHaveAccount = _localizer["registration.alreadyHaveAccount"],
            SignInLink = _localizer["registration.signInLink"]
        });
    }
}
