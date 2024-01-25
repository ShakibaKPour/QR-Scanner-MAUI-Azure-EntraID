using RepRepair.Extensions;
using RepRepair.Models;
using RepRepair.Services.Language;
using RepRepair.Services.Navigation;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace RepRepair.ViewModels;

public class ReportViewModel : BaseViewModel
{
    private readonly INavigationService _navigationService;
    private ObjectInfo _objectInfo;
    private readonly LanguageSettingsService _languageSettingsService;
    public ObservableCollection<string> AvailableLanguages { get; } = new ObservableCollection<string>
    {
        "en-US", "es-ES", "it-IT", "sv-SE", "fr-FR", "fa-IR", "de-DE", "da-DK"
    };
    public ICommand NavigateToVoiceRecordCommand { get; set; }

    public ObjectInfo ObjectInfo
    {
        get => _objectInfo;
        set
        {
            _objectInfo = value;
            OnPropertyChanged(nameof(ObjectInfo));
        }
    }

    public string SelectedLanguage
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


    public ReportViewModel()
    {
        _languageSettingsService= ServiceHelper.GetService<LanguageSettingsService>();
        //Setting up a messaging subscriber to receive the objectinfo and then use it to initialize the ReportViewModel
        MessagingCenter.Subscribe<ScanViewModel, ObjectInfo>(this, "ObjectInfoMessage", (sender, arg) =>
        {
            ObjectInfo = arg;
        });
        _navigationService = ServiceHelper.GetService<INavigationService>();
        _objectInfo = new ObjectInfo();
        NavigateToVoiceRecordCommand = new Command(async () => await NavigateToVoiceRecordCommandAsync());
    }

    ~ReportViewModel()
    {
        MessagingCenter.Unsubscribe<ScanViewModel, ObjectInfo>(this, "ObjectInfoMessage");
    }

    public override Task InitializeAsync(object parameter)
    {
        if (parameter is ObjectInfo objectInfo)
        {
            _objectInfo = objectInfo;
            // Notify UI about the property change
            OnPropertyChanged(nameof(_objectInfo));
        }
        return Task.CompletedTask;
    }


    private async Task NavigateToVoiceRecordCommandAsync()
    {
        await _navigationService.NavigateToAsync<VoiceReportViewModel>();
    }

}
