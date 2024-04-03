using Microsoft.Identity.Client;
using RepRepair.Extensions;
using RepRepair.Services.AlertService;
using System.Windows.Input;

namespace RepRepair.ViewModels;

public class SignInViewModel : BaseViewModel
{
    private readonly IAlertService _alertService;

    public ICommand SignInCommand { get; private set; }

    public SignInViewModel()
    {
        _alertService = ServiceHelper.GetService<IAlertService>();

        SignInCommand = new Command(async () => await SignInAsync());
    }

    private async Task SignInAsync()
    {
        try
        {
            //Todo: try to inject the authenticationservice directly and see if it solves the problem with the token on windows machine
            var authResult = await App._authenticationService.SignInAsync();
            if (authResult != null)
            {
                MessagingCenter.Send(Application.Current as App, "SignInSuccessful");
            }
            else
            {
                await _alertService.ShowAlertAsync("Sign In Failed", "Authentication result was not obtained.", "OK");
            }
        }
        catch (MsalClientException ex)
        {
            await _alertService.ShowAlertAsync("Sign In Error", $"MSAL client error occurred: {ex.Message}", "OK");
        }
        catch (Exception ex)
        {
            await _alertService.ShowAlertAsync("Sign In Error", $"An unexpected error occurred during sign-in: {ex.Message}", "OK");
        }
    }
}


    //public ICommand SignInCommand { get; }

    //public SignInViewModel()
    //{
    //    SignInCommand = new Command(async () => await SignInAsync());
    //}

    //private async Task SignInAsync()
    //{
    //    try
    //    {
    //        var authResult =  await App._authenticationService.SignInAsync();
    //        if (authResult != null)
    //        {
    //            MessagingCenter.Send<App>(Application.Current as App, "SignInSuccessful");
    //        }
    //    }
    //    catch (MsalClientException ex)
    //    {
    //        Console.WriteLine(ex.Message);
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine($"An unexpected error occurred during sign-in: {ex.Message}");
    //    }
    //}

