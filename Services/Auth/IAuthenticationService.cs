using Microsoft.Identity.Client;

namespace RepRepair.Services.Auth;

public interface IAuthenticationService
{
    Task<AuthenticationResult> AcquireTokenSilentAsync();
    Task<AuthenticationResult> SignInAsync();
    Task SignOutAsync();
}
