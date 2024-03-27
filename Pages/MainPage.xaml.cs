using Microsoft.Identity.Client;
using RepRepair.Extensions;
using RepRepair.Services.Auth;

namespace RepRepair.Pages;

public partial class MainPage : ContentPage
{
    public static AuthenticationService _authenticationService { get; private set; }
    public MainPage()
	{
		InitializeComponent();
        _authenticationService = ServiceHelper.GetService<AuthenticationService>();
        CheckSignInStatus();
    }

    private async void CheckSignInStatus()
    {
        try
        {
            await _authenticationService.AcquireTokenSilentAsync();
            await Shell.Current.GoToAsync("//Home");
        }
        catch (MsalUiRequiredException)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Shell.Current.GoToAsync(nameof(SignInPage));
                //MainPage = new NavigationPage(new SignInPage());
            });

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    private async void SignIn_Clicked(object sender, EventArgs e)
    {
        await _authenticationService.SignInAsync();
        await Shell.Current.GoToAsync(nameof(HomePage));
    }
}