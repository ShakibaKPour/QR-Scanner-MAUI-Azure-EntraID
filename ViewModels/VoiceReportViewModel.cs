using RepRepair.Extensions;
using RepRepair.Services.Cognitive;
using RepRepair.Services.DB;
using RepRepair.Services.Language;
using RepRepair.Services.VoiceRecording;
using System.Collections.ObjectModel;
using System.Windows.Input;
using RepRepair.Services.AlertService;
using RepRepair.Models.DatabaseModels;
using RepRepair.Services.ScanningService;
using RepRepair.Pages;
using RepRepair.Services.ReportTypesService;

namespace RepRepair.ViewModels;

public class VoiceReportViewModel : BaseViewModel
{
        private readonly IScanningService _scanningService;
        private readonly IDatabaseService _databaseService;
        private readonly IAlertService _alertService;
        private readonly IVoiceRecordingService _voiceRecordingService;
        private readonly IAzureCognitiveService _cognitiveServices;
        private readonly LanguageSettingsService _languageSettingsService;
        private readonly ReportServiceType _reportServiceType;

        public ObjectInfo ObjectInfo => _scanningService.CurrentScannedObject;

        public ObservableCollection<Languages> AvailableLanguages => _languageSettingsService.AvailableLanguages;

        public List<ReportType> ReportTypes => _reportServiceType.CachedReportTypes;

        public ICommand RecordCommand { get; private set; }
        public ICommand DeleteRecordingCommand { get; private set; }
        public ICommand SubmitCommand { get; private set; }
        public ICommand OnRefresh { get; private set; }
        public ICommand OnGoBack { get; private set; }

        private string _transcribedText;
        public string TranscribedText
        {
            get => _transcribedText;
            set
            {
                _transcribedText = value;
                OnPropertyChanged(nameof(TranscribedText));
                IsTranscriptionVisible = !string.IsNullOrEmpty(value);
            }
        }

        private bool _isTranscriptionVisible;
        public bool IsTranscriptionVisible
        {
            get => _isTranscriptionVisible;
            set
            {
                _isTranscriptionVisible = value;
                OnPropertyChanged(nameof(IsTranscriptionVisible));
            }
        }

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

        public VoiceReportViewModel()
        {
            _scanningService = ServiceHelper.GetService<IScanningService>();
            _languageSettingsService = ServiceHelper.GetService<LanguageSettingsService>();
            _reportServiceType = ServiceHelper.GetService<ReportServiceType>();
            _cognitiveServices = ServiceHelper.GetService<IAzureCognitiveService>();
            _voiceRecordingService = ServiceHelper.GetService<IVoiceRecordingService>();
            _databaseService = ServiceHelper.GetService<IDatabaseService>();
            _alertService = ServiceHelper.GetService<IAlertService>();

            InitializeCommands();
            ValidateIsScanned();
        }

        private void InitializeCommands()
        {
            RecordCommand = new Command(async () => await OnRecord());
            DeleteRecordingCommand = new Command(DeleteRecording);
            SubmitCommand = new Command(async () => await Submit());
            OnGoBack = new Command(async () => await NavigateBackAsync());
            OnRefresh = new Command(async () => await RefreshCommandExecuted());
        }

        private async void ValidateIsScanned()
        {
            if (ObjectInfo == null)
            {
                await _alertService.ShowAlertAsync("Alert", "Start by scanning the QR code", "OK");
                await Shell.Current.GoToAsync("///ScanPage");
            }
        }

        private async Task NavigateBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }

        private async Task OnRecord()
        {
            try
            {
                var transcription = await _cognitiveServices.TranscribeAudioAsync();
                TranscribedText = transcription ?? string.Empty;
                if (string.IsNullOrEmpty(transcription))
                {
                    await _alertService.ShowAlertAsync("Alert", "No input detected. Please try again.", "OK");
                }
            }
            catch (Exception ex)
            {
                await _alertService.ShowAlertAsync("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        private void DeleteRecording()
        {
            TranscribedText = string.Empty;
        }

        private async Task Submit()
        {
            if (!string.IsNullOrEmpty(TranscribedText) && ObjectInfo != null)
            {
            var reportType = ReportTypes.FirstOrDefault(r => r.TypeOfReport == "Voice Message");
            if (reportType == null)
            {
                await _reportServiceType.RefreshReportTypes();
                reportType = ReportTypes.Where(r => r.TypeOfReport == "Voice Message").FirstOrDefault();
            }

            var newReportData = new ReportInfo
                {
                    SelectedLanguage = _languageSettingsService.CurrentLanguage.ID,
                    OriginalFaultReport = TranscribedText,
                    TypeOfReport = reportType?.ID,
                    QRCodeString = ObjectInfo.QRCode,
                };
                var success = await _databaseService.InsertReportAsync(newReportData, SelectedLanguage);
                if (success)
                {
                    await Shell.Current.GoToAsync(nameof(ThankYouPage));
                    _scanningService.ResetScan();
                }
                else
                {
                    await _alertService.ShowAlertAsync("Error", "Failed to submit report.", "OK");
                }
            }
            else
            {
                await _alertService.ShowAlertAsync("Error", "Please ensure all required fields are filled.", "OK");
            }

        }

    public async Task RefreshCommandExecuted()
    {
        await _languageSettingsService.RefreshAvailableLanguages(_databaseService);
        await _reportServiceType.RefreshReportTypes();
    }
}


//    public ObjectInfo ObjectInfo => _scanningService.CurrentScannedObject;
//    public ObservableCollection<Languages> AvailableLanguages
//    {
//        get => _languageSettingsService.AvailableLanguages;
//    }
//    public List<ReportType> ReportTypes { get => _reportServiceType.CachedReportTypes; }
//    public ICommand RecordCommand { get; }
//    public ICommand DeleteRecordingCommand { get; }
//    public ICommand SubmitCommand { get; }
//    public ICommand OnRefresh { get; set; }
//    public ICommand OnGoBack { get; set; }
//    private readonly IScanningService _scanningService;   
//    private readonly IDatabaseService _databaseService;
//    private readonly IAlertService _alertService;
//    private readonly IVoiceRecordingService _voiceRecordingService;
//    private readonly IAzureCognitiveService _cognitiveServices;
//   // private readonly TranslatorService _translatorService;
//    private readonly LanguageSettingsService _languageSettingsService;
//    private readonly ReportServiceType _reportServiceType;
//    private string _transcribedText;
//    public string TranscribedText
//    {
//        get => _transcribedText;
//        set
//        {
//            _transcribedText = value;
//            OnPropertyChanged(nameof(TranscribedText));
//        }
//    }
//    //private string _translatedText;
//    //public string TranslatedText
//    //{
//    //    get=> _translatedText;
//    //    set
//    //    {
//    //        _translatedText = value;
//    //        OnPropertyChanged(nameof(TranslatedText));
//    //    }
//    //}

//    private bool _isTranscriptionVisible;
//    public bool IsTranscriptionVisible
//    {
//        get => _isTranscriptionVisible;
//        set
//        {
//            _isTranscriptionVisible = value;
//            OnPropertyChanged(nameof(IsTranscriptionVisible));
//        }
//    }
//    //private bool _isTranslationVisible;
//    //public bool IsTranslationVisible
//    //{
//    //    get => _isTranslationVisible;
//    //    set
//    //    {
//    //        _isTranslationVisible = value;
//    //        OnPropertyChanged(nameof(IsTranslationVisible));
//    //    }
//    //}

//    public Languages SelectedLanguage
//    {
//        get => _languageSettingsService.CurrentLanguage;
//        set
//        {
//            if (_languageSettingsService.CurrentLanguage != value)
//            {
//                _languageSettingsService.CurrentLanguage = value;
//                OnPropertyChanged(nameof(SelectedLanguage));
//            }
//        }
//    }
//    public VoiceReportViewModel()
//    {
//        _scanningService= ServiceHelper.GetService<IScanningService>();
//        _scanningService.ScannedObjectChanged += (objectInfo) =>
//        {
//            OnPropertyChanged(nameof(ObjectInfo));
//        };
//        _languageSettingsService = ServiceHelper.GetService<LanguageSettingsService>();
//        _reportServiceType = ServiceHelper.GetService<ReportServiceType>();
//       // _translatorService = ServiceHelper.GetService<TranslatorService>();
//        _cognitiveServices = ServiceHelper.GetService<IAzureCognitiveService>();
//        _voiceRecordingService = ServiceHelper.GetService<IVoiceRecordingService>();
//        _databaseService = ServiceHelper.GetService<IDatabaseService>();
//        _alertService = ServiceHelper.GetService<IAlertService>();
//        RecordCommand = new Command(OnRecord);
//        DeleteRecordingCommand = new Command(DeleteRecording);
//        SubmitCommand = new Command(Submit);
//        OnGoBack = new Command(async () => await NavigateBackAsync());
//        OnRefresh = new Command(async () => await RefreshCommandExecuted());
//        ValidateIsScanned();
//    }
//    private async void ValidateIsScanned()
//    {
//        if (ObjectInfo == null)
//        {
//            await _alertService.ShowAlertAsync("Alert", "Start by scanning the QR code", "OK");
//            await Shell.Current.GoToAsync("///ScanPage");
//        }
//    }

//    private async Task NavigateBackAsync()
//    {
//        await Shell.Current.GoToAsync("..");
//    }
//    private async void OnRecord()
//    {
//        try
//        {
//            //var granted = await Permissions.RequestAsync<Permissions.Microphone>();
//            //if(granted == PermissionStatus.Granted)
//            //{
//            var transcription = await _cognitiveServices.TranscribeAudioAsync();
//            if (!string.IsNullOrEmpty(transcription))
//            {
//                TranscribedText = transcription;
//                IsTranscriptionVisible = true;
//               // Translate(transcription);
//            }
//            else
//            {
//                IsTranscriptionVisible = false;
//                await _alertService.ShowAlertAsync("Alert", "No Input. Please press the Record button and talk to the mic", "OK");
//            }

//            //}else if (granted == PermissionStatus.Denied)
//            //{
//            //    _alertService.ShowAlert("Alert", "Allow the microphone to proceed");
//            //}


//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine(ex.ToString());
//        }

//    }
//    //private async void Translate(string text)
//    //{
//    //    if (!string.IsNullOrEmpty(text))
//    //    {
//    //        var textTobeTranslated = text;
//    //        var translate = await _translatorService.TranslateTextAsync(textTobeTranslated, "sv", SelectedLanguage);
//    //        TranslatedText = translate;
//    //        IsTranslationVisible = true;
//    //    }
//    //    else
//    //    {
//    //        IsTranslationVisible = false;
//    //    }
//    //}
//    private void DeleteRecording()
//    {
//        TranscribedText = string.Empty;
//       // TranslatedText = string.Empty;
//      //  IsTranslationVisible=false;
//        IsTranscriptionVisible= false;
//        //also deleting the cached??
//    }

//    private async void Submit()
//    {
//        if (!string.IsNullOrEmpty(TranscribedText) && /*!string.IsNullOrEmpty(TranslatedText) &&*/ ObjectInfo != null)
//        {
//            var reportType = ReportTypes.Where(r => r.TypeOfReport == "Voice Message").FirstOrDefault();
//            if (reportType == null)
//            {
//                await _reportServiceType.RefreshReportTypes();
//                reportType = ReportTypes.Where(r => r.TypeOfReport == "Defect List").FirstOrDefault();
//            }
//            var newReportData = new ReportInfo
//            {
//                SelectedLanguage = _languageSettingsService.CurrentLanguage.ID,
//                OriginalFaultReport = TranscribedText,
//                //TranslatedFaultReport = TranslatedText,
//                TypeOfReport = reportType.ID,
//               QRCodeString = ObjectInfo.QRCode,
//            };
//           var success =  await _databaseService.InsertReportAsync(newReportData, SelectedLanguage);
//            if (success)
//            {
//                await Shell.Current.GoToAsync(nameof(ThankYouPage));
//                _scanningService.ResetScan();
//            }
//            ClearFields();
//        }
//    }

//    private void ClearFields()
//    {
//        TranscribedText=string.Empty;
//       // TranslatedText=string.Empty;
//        IsTranscriptionVisible = false;
//       // IsTranslationVisible=false;
//    }

//    public async Task RefreshCommandExecuted()
//    {
//        await _languageSettingsService.RefreshAvailableLanguages(ServiceHelper.GetService<IDatabaseService>());
//        await _reportServiceType.RefreshReportTypes();
//    }
//}
