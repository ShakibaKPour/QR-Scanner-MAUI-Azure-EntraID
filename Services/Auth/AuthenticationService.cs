using Microsoft.Identity.Client;
using RepRepair.Extensions;
using RepRepair.Pages;

namespace RepRepair.Services.Auth
{
    public class AuthenticationService : IAuthenticationService
    {
        private IPublicClientApplication _publicClientApplication;
        private string[] _scopes = Constants.Scopes;
        public AuthenticationService()
        {

            if (this._publicClientApplication == null)
            {
                if (_publicClientApplication == null)
                {
#if ANDROID
                    _publicClientApplication = PublicClientApplicationBuilder
                        .Create(Constants.ApplicationId)
                        .WithAuthority(AzureCloudInstance.AzurePublic, "common")
                        .WithRedirectUri($"msal{Constants.ApplicationId}://auth")
                        .WithParentActivityOrWindow(() => Platform.CurrentActivity)
                        .Build();
#elif IOS
                    _publicClientApplication = PublicClientApplicationBuilder
                        .Create(Constants.ApplicationId)
                        .WithAuthority(AzureCloudInstance.AzurePublic, "common")
                        .WithIosKeychainSecurityGroup("com.microsoft.adalcache")
                        .WithRedirectUri($"msal{Constants.ApplicationId}://auth")
                        .Build();
#else
                    _publicClientApplication = PublicClientApplicationBuilder
                        .Create(Constants.ApplicationId)
                        .WithAuthority(AzureCloudInstance.AzurePublic, "common")
                        .WithRedirectUri("https://login.microsoftonline.com/common/oauth2/nativeclient")
                        .Build();
#endif
                }

            }
        }
        
        public async Task<AuthenticationResult> AcquireTokenSilentAsync()
        {
            var accounts = await _publicClientApplication.GetAccountsAsync();
            return await _publicClientApplication.AcquireTokenSilent(_scopes, accounts.FirstOrDefault())
                .ExecuteAsync();
        }

        public async Task<AuthenticationResult> SignInAsync()
        {
            return await _publicClientApplication.AcquireTokenInteractive(_scopes).ExecuteAsync();
        }

        public async Task SignOutAsync()
        {
            var accounts = await _publicClientApplication.GetAccountsAsync();
            foreach (var account in accounts)
            {
                await _publicClientApplication.RemoveAsync(account);
            }

            Device.BeginInvokeOnMainThread(() =>
            {
                  Application.Current.MainPage = new NavigationPage(new SignInPage());
            });
        }
    }
}
