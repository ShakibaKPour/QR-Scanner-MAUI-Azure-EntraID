using Microsoft.Identity.Client;
using RepRepair.Extensions;
using RepRepair.Models.DatabaseModels;
using RepRepair.Services.Auth;
using RepRepair.Services.Language;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace RepRepair.ViewModels;

public partial class HomeViewModel : BaseViewModel
{
    public ICommand ScanCommand { get; set; }
    public ICommand SignOutCommand { get; }

    public AuthenticationService _authenticationService { get; set; }

    public string _username => _authenticationService.AcquireTokenSilentAsync().Result.Account.Username;

    private readonly LanguageSettingsService _languageSettingsService;
    public Languages SelectedLanguage
    {
        get => _languageSettingsService.CurrentLanguage;
        set
        {
            if (_languageSettingsService.CurrentLanguage != value)
            {
                _languageSettingsService.CurrentLanguage = value;
                OnPropertyChanged(nameof(SelectedLanguage));
            }
        }
    }

    public HomeViewModel()
    {
        _authenticationService = ServiceHelper.GetService<AuthenticationService>();
        _languageSettingsService = ServiceHelper.GetService<LanguageSettingsService>();
        ScanCommand = new Command(async () => await OnScanAsync());
        SignOutCommand = new Command(async () => await SignOutAsync());
    }


    private async Task SignOutAsync()
    {
        try
        {
            await App._authenticationService.SignOutAsync();
        }
        catch (MsalClientException ex)
        {
            // Handle exceptions from MSAL
            Console.WriteLine(ex.Message);
            // Optionally display an error message to the user
        }
    }

    private async Task OnScanAsync()
    {
        await Shell.Current.GoToAsync("//ScanPage");
    }
}
