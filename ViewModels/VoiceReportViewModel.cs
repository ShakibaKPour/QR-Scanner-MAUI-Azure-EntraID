using CommunityToolkit.Maui.Core.Views;
using RepRepair.Extensions;
using RepRepair.Models;
using RepRepair.Services.Cognitive;
using RepRepair.Services.DB;
using RepRepair.Services.Language;
using RepRepair.Services.VoiceRecording;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Maui.Alerts;
using RepRepair.Services.AlertService;
using RepRepair.Models.DatabaseModels;

namespace RepRepair.ViewModels;

public class VoiceReportViewModel : BaseViewModel
{
    private string _transcribedText;
    private string _translatedText;
    //private bool _isRecording;
    private bool _isTranscriptionVisible;
    private bool _isTranslationVisible;
    private ObjectInfo _objectInfo;
    private readonly IDatabaseService _databaseService;
    private readonly LanguageSettingsService _languageSettingsService;
    private readonly IAlertService _alertService;
    public ObservableCollection<string> AvailableLanguages { get; } = new ObservableCollection<string>
    {
        "en-US", "es-ES", "it-IT", "sv-SE", "fr-FR", "fa-IR", "de-DE", "da-DK"
    };
    private readonly IVoiceRecordingService _voiceRecordingService;
    private readonly IAzureCognitiveService _cognitiveServices;
    private readonly TranslatorService _translatorService;
    public ICommand RecordCommand { get; }
    //public ICommand StopCommand { get; }
    public ICommand DeleteRecordingCommand { get; }
    public ICommand SubmitCommand { get; }

    public string TranscribedText
    {
        get => _transcribedText;
        set
        {
            _transcribedText = value;
            OnPropertyChanged(nameof(TranscribedText));
        }
    }
    public string TranslatedText
    {
        get=> _translatedText;
        set
        {
            _translatedText = value;
            OnPropertyChanged($"{nameof(TranslatedText)}");
        }
    }

    //public bool IsRecording
    //{
    //    get => _isRecording;
    //    set
    //    {
    //        _isRecording = value;
    //        OnPropertyChanged(nameof(IsRecording));
    //    }
    //}

    public bool IsTranscriptionVisible
    {
        get => _isTranscriptionVisible;
        set
        {
            _isTranscriptionVisible = value;
            OnPropertyChanged(nameof(IsTranscriptionVisible));
        }
    }
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



    public VoiceReportViewModel()
    {
        _languageSettingsService = ServiceHelper.GetService<LanguageSettingsService>();
        RecordCommand = new Command(OnRecord);
        _translatorService = ServiceHelper.GetService<TranslatorService>();
        //StopCommand = new Command(OnStop);
        DeleteRecordingCommand = new Command(DeleteRecording);
        SubmitCommand = new Command(Submit);
        _cognitiveServices = ServiceHelper.GetService<IAzureCognitiveService>();
        _voiceRecordingService = ServiceHelper.GetService<IVoiceRecordingService>();
        _databaseService = ServiceHelper.GetService<IDatabaseService>();
        _alertService = ServiceHelper.GetService<IAlertService>();
        _objectInfo = new ObjectInfo();
        SubscribeToMessages();
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
        MessagingCenter.Subscribe<ReportViewModel, ObjectInfo>(this, "ObjectInfoMessage", (sender, arg) =>
        {
            _objectInfo = arg;
            OnPropertyChanged(nameof(ObjectInfo)); // Notify UI about the change
        });
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
        //else
        //{
        //    Task.Run(async () =>
        //    {
        //        await Task.Delay(1000);
        //        App.AlertSvc.ShowAlertAsync("Empty Input", "Press OnRedord and talk into your mic!", "OK");
        //    });
        //}
 
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
        // Logic to submit the transcription and recording
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
