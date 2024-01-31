using RepRepair.Extensions;
using RepRepair.Models.DatabaseModels;
using RepRepair.Services.Language;
using RepRepair.Services.Navigation;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace RepRepair.ViewModels;

public class ReportViewModel : BaseViewModel
{
    private ObjectInfo _objectInfo;
    private readonly LanguageSettingsService _languageSettingsService;
    public ObservableCollection<string> AvailableLanguages { get; } = new ObservableCollection<string>
    {
        "en-US", "es-ES", "it-IT", "sv-SE", "fr-FR", "fa-IR", "de-DE", "da-DK"
    };
    public ICommand NavigateToVoiceRecordCommand { get; set; }

    public ICommand NavigateToWriteCommand { get; set; }

    public ICommand NavigateToDefectListCommand { get; set; }

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
        _objectInfo = new ObjectInfo();
        SubscribeToMessages();
        NavigateToVoiceRecordCommand = new Command(async () => await NavigateToVoiceRecordCommandAsync());
        NavigateToWriteCommand = new Command(async ()=> await NavigateToWriteCommandAsync());
        NavigateToDefectListCommand= new Command(async()=> await NavigateToDefectListCommandAsync());
    }
    private void UpdateObjectProperties(ObjectInfo objectInfo)
    {
        if (objectInfo != null)
        {
            OnPropertyChanged(nameof(ObjectInfo.Name));
            OnPropertyChanged(nameof(ObjectInfo.ObjectId));
            OnPropertyChanged(nameof(ObjectInfo.Location));
            OnPropertyChanged(nameof(ObjectInfo.QRCode));
        }
    }
    private void SubscribeToMessages()
    {
        MessagingCenter.Subscribe<ScanViewModel, ObjectInfo>(this, "ObjectInfoMessage", (sender, arg) =>
        {
            _objectInfo = arg;
            OnPropertyChanged(nameof(ObjectInfo));
        });
    }
    ~ReportViewModel()
    {
        MessagingCenter.Unsubscribe<ScanViewModel, ObjectInfo>(this, "ObjectInfoMessage");
    }

    private async Task NavigateToVoiceRecordCommandAsync()
    {
        await Shell.Current.GoToAsync("VoiceReportPage");
        MessagingCenter.Send(this, "ObjectInfoMessage", _objectInfo);
    }

    private async Task NavigateToWriteCommandAsync()
    {
        await Shell.Current.GoToAsync("Write to Us!");
        MessagingCenter.Send(this, "ObjectInfoMessage", _objectInfo);
    }

    private async Task NavigateToDefectListCommandAsync()
    {
        await Shell.Current.GoToAsync("Choose a Defect");
        MessagingCenter.Send(this, "ObjectInfoMessage", _objectInfo);
    }

}
