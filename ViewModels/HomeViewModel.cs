using Microsoft.Identity.Client;
using RepRepair.Models.DatabaseModels;
using RepRepair.Services.Language;
using System.Windows.Input;

namespace RepRepair.ViewModels;

public partial class HomeViewModel : BaseViewModel
{


    // TODO
    // Move the functions for get langugae, report and defect list to another azure function and set it open, no
    // authorization needed (public info) and load them all in app.xaml.cs
    // Move the signIn and check signIn status to this page and create a button for signIn
    // SignIn btn will check the signIn status and if not signIn will be directed to signIn page
    // If signed in, user gets a message that they are good to go => later on I can change the button to be dynamic
    // then when signed in comes back to home but shell shouldn't be null anymore.
    public ICommand ScanCommand { get; set; }
    public ICommand SignOutCommand { get; }

   // public _authenticationService _authenticationService { get; set; }

    //public string _username
    //{
    //    get => _authenticationService.AcquireTokenSilentAsync().Result.Account.Username;
    //}

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
        //_authenticationService = ServiceHelper.GetService<_authenticationService>();
        //_languageSettingsService = ServiceHelper.GetService<LanguageSettingsService>();
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
            // TODO: display an error message to the user
        }
    }

    private async Task OnScanAsync()
    {
        await Shell.Current.GoToAsync("//ScanPage");
    }
}
