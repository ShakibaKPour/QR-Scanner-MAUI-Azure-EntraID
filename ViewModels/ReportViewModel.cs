using RepRepair.Extensions;
using RepRepair.Models.DatabaseModels;
using RepRepair.Services.Language;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace RepRepair.ViewModels;

public class ReportViewModel : BaseViewModel
{
    public ObservableCollection<string> AvailableLanguages { get; } = new ObservableCollection<string>
    {
        "en-US", "es-ES", "it-IT", "sv-SE", "fr-FR", "fa-IR", "de-DE", "da-DK"
    };
    public ICommand NavigateToVoiceRecordCommand { get; set; }

    public ICommand NavigateToWriteCommand { get; set; }

    public ICommand NavigateToDefectListCommand { get; set; }

    private ObjectInfo _objectInfo;
    public ObjectInfo ObjectInfo
    {
        get => _objectInfo;
        set
        {
            if (_objectInfo != value)
            {
                _objectInfo = value;
                OnPropertyChanged(nameof(ObjectInfo));
                UpdateObjectProperties(_objectInfo);
            }
        }
    }
    private readonly LanguageSettingsService _languageSettingsService;
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
        _languageSettingsService = ServiceHelper.GetService<LanguageSettingsService>();
        NavigateToVoiceRecordCommand = new Command(async () => await NavigateToVoiceRecordCommandAsync());
        NavigateToWriteCommand = new Command(async ()=> await NavigateToWriteCommandAsync());
        NavigateToDefectListCommand= new Command(async()=> await NavigateToDefectListCommandAsync());
    }
    private void UpdateObjectProperties(ObjectInfo objectInfo)
    {
        if (objectInfo != null)
        {
            OnPropertyChanged(nameof(ObjectInfo.Name));
            //OnPropertyChanged(nameof(ObjectInfo.ObjectId));
            OnPropertyChanged(nameof(ObjectInfo.Location));
            OnPropertyChanged(nameof(ObjectInfo.QRCode));
        }
    }
    private async Task NavigateToVoiceRecordCommandAsync()
    {
        await Shell.Current.GoToAsync("VoiceReportPage");
    }

    private async Task NavigateToWriteCommandAsync()
    {
        await Shell.Current.GoToAsync("Write to Us!");
    }

    private async Task NavigateToDefectListCommandAsync()
    {
        await Shell.Current.GoToAsync("Choose a Defect");
    }

}
