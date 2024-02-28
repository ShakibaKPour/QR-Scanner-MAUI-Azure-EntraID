using RepRepair.Extensions;
using RepRepair.Models.DatabaseModels;
using RepRepair.Services.Language;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace RepRepair.ViewModels;

public partial class HomeViewModel : BaseViewModel
{
    public ICommand ScanCommand { get; set; }

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
        _languageSettingsService = ServiceHelper.GetService<LanguageSettingsService>();
        ScanCommand = new Command(async () => await OnScanAsync());

    }

    private async Task OnScanAsync()
    {
        await Shell.Current.GoToAsync("//ScanPage");
    }
}
