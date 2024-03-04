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
    public ICommand AuthenticateCommand { get; set; }

    public AuthenticationService _authenticationService { get; set; }

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
        AuthenticateCommand = new Command(async () => await OnAuthenticationAsync());
    }

    private async Task OnAuthenticationAsync()
    {
        await _authenticationService.AcquireTokenSilentAsync();
    }

    private async Task OnScanAsync()
    {
        await Shell.Current.GoToAsync("//ScanPage");
    }
}
