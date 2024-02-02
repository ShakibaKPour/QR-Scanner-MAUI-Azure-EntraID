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

namespace RepRepair.ViewModels;

public class VoiceReportViewModel : BaseViewModel
{
    public ObjectInfo ObjectInfo => _scanningService.CurrentScannedObject;
    public ObservableCollection<string> AvailableLanguages { get; } = new ObservableCollection<string>
    {
        "en-US", "es-ES", "it-IT", "sv-SE", "fr-FR", "fa-IR", "de-DE", "da-DK"
    };
    public ICommand RecordCommand { get; }
    public ICommand DeleteRecordingCommand { get; }
    public ICommand SubmitCommand { get; }
    private readonly IScanningService _scanningService;   
    private readonly IDatabaseService _databaseService;
    private readonly IAlertService _alertService;
    private readonly IVoiceRecordingService _voiceRecordingService;
    private readonly IAzureCognitiveService _cognitiveServices;
    private readonly TranslatorService _translatorService;
    private readonly LanguageSettingsService _languageSettingsService;
    private string _transcribedText;
    public string TranscribedText
    {
        get => _transcribedText;
        set
        {
            _transcribedText = value;
            OnPropertyChanged(nameof(TranscribedText));
        }
    }
    private string _translatedText;
    public string TranslatedText
    {
        get=> _translatedText;
        set
        {
            _translatedText = value;
            OnPropertyChanged(nameof(TranslatedText));
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
    private bool _isTranslationVisible;
    public bool IsTranslationVisible
    {
        get => _isTranslationVisible;
        set
        {
            _isTranslationVisible = value;
            OnPropertyChanged(nameof(IsTranslationVisible));
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
    public VoiceReportViewModel()
    {
        _scanningService= ServiceHelper.GetService<IScanningService>();
        _scanningService.ScannedObjectChanged += (objectInfo) =>
        {
            OnPropertyChanged(nameof(ObjectInfo));
        };
        _languageSettingsService = ServiceHelper.GetService<LanguageSettingsService>();
        _translatorService = ServiceHelper.GetService<TranslatorService>();
        _cognitiveServices = ServiceHelper.GetService<IAzureCognitiveService>();
        _voiceRecordingService = ServiceHelper.GetService<IVoiceRecordingService>();
        _databaseService = ServiceHelper.GetService<IDatabaseService>();
        _alertService = ServiceHelper.GetService<IAlertService>();
        RecordCommand = new Command(OnRecord);
        DeleteRecordingCommand = new Command(DeleteRecording);
        SubmitCommand = new Command(Submit);
    }
    private async void OnRecord()
    {
        try
        {
            var transcription = await _cognitiveServices.TranscribeAudioAsync();
            if (!string.IsNullOrEmpty(transcription))
            {
                TranscribedText = transcription;
                IsTranscriptionVisible = true;
                Translate(transcription);
            }
            else
            {
                IsTranscriptionVisible = false;
                await _alertService.ShowAlertAsync("Alert", "No Input. Please press the Record button and talk to the mic", "OK");
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

    }
    private async void Translate(string text)
    {
        if (!string.IsNullOrEmpty(text))
        {
            var textTobeTranslated = text;
            var translate = await _translatorService.TranslateTextAsync(textTobeTranslated, "sv", SelectedLanguage);
            TranslatedText = translate;
            IsTranslationVisible = true;
        }
        else
        {
            IsTranslationVisible = false;
        }
    }
    private void DeleteRecording()
    {
        TranscribedText = string.Empty;
        TranslatedText = string.Empty;
        IsTranslationVisible=false;
        IsTranscriptionVisible= false;
        //also deleting the cached??
    }

    private async void Submit()
    {
        if (!string.IsNullOrEmpty(TranscribedText) && !string.IsNullOrEmpty(TranslatedText) && ObjectInfo != null)
        {
            var newVoiceMessageInfo = new VoiceMessageInfo
            {
                Language = _languageSettingsService.CurrentLanguage,
                Transcription = TranscribedText,
                Translation = TranslatedText,
            };
           var success =  await _databaseService.AddVoiceMessageInfoAsync(newVoiceMessageInfo);
            if (success)
            {
                var allVoiceMessages = await _databaseService.GetAllVoiceMessagesAsync();
                await Shell.Current.GoToAsync("Thank You!");
                _scanningService.ResetScan();
            }
            ClearFields();
            // Updating of the relevant tables will happen automatically via another method (azure functions) that updates related tables => which are defectinfo and report info table
        }
    }

    private void ClearFields()
    {
        TranscribedText=string.Empty;
        TranslatedText=string.Empty;
        IsTranscriptionVisible = false;
        IsTranslationVisible=false;
    }
}
