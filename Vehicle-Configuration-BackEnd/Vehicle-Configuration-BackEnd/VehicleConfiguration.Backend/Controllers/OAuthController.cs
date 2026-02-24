using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VehicleConfiguration.Backend.Services;

namespace VehicleConfiguration.Backend.Controllers;

[ApiController]
public class OAuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public OAuthController(IAuthService authService)
    {
        _authService = authService;
    }

    // Matches Spring Boot's default authorization URL structure
    [HttpGet("oauth2/authorization/{provider}")]
    public IActionResult ExternalLogin(string provider)
    {
        string scheme;
        if (provider.ToLower() == "google") scheme = GoogleDefaults.AuthenticationScheme;
        else if (provider.ToLower() == "facebook") scheme = FacebookDefaults.AuthenticationScheme;
        else return BadRequest("Invalid provider");

        var properties = new AuthenticationProperties
        {
            RedirectUri = Url.Action("ExternalLoginCallback", "OAuth")
        };

        return Challenge(properties, scheme);
    }

    [HttpGet("api/auth/oauth/callback")]
    public async Task<IActionResult> ExternalLoginCallback()
    {
        var result = await HttpContext.AuthenticateAsync(User.Identity?.AuthenticationType ?? "Cookies"); 
        
        // If "Cookies" fail, try to get from the specific scheme if needed, but usually the Challenge sets a cookie.
        // Actually, without Cookie scheme setup in Program.cs, External Login info might be transient.
        // Correct approach in stateless API:
        // We need to inspect the result of the remote authentication.
        
        // NOTE: In a pure API backend, we rely on the external provider calling back to /signin-google (handled by middleware)
        // which then redirects to our RedirectUri (this method). 
        // We need to retrieve the user's info here.
        
        if (!User.Identity.IsAuthenticated)
        {
             // Try to authenticate against the external scheme directly to get the principal if it wasn't automatically signed in to a cookie
             var googleAuth = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
             if (googleAuth.Succeeded) {
                 return await ProcessLogin(googleAuth.Principal);
             }
             var fbAuth = await HttpContext.AuthenticateAsync(FacebookDefaults.AuthenticationScheme);
             if (fbAuth.Succeeded) {
                 return await ProcessLogin(fbAuth.Principal);
             }

             return Unauthorized("External authentication failed");
        }

        return await ProcessLogin(User);
    }

    private async Task<IActionResult> ProcessLogin(ClaimsPrincipal principal)
    {
        var email = principal.FindFirst(ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(email))
        {
             return BadRequest("Email not found in external provider info");
        }

        // Generate JWT using our AuthService
        var token = _authService.GenerateTokenForOAuth(email);

        // Redirect to Frontend with Token (matching Java behavior)
        return Redirect($"http://localhost:5173/login?token={token}");
    }
}
