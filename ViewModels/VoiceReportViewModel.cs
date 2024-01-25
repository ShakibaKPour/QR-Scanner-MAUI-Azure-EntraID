using Plugin.Maui.Audio;
using RepRepair.Extensions;
using RepRepair.Models;
using RepRepair.Services.Cognitive;
using RepRepair.Services.Language;
using RepRepair.Services.VoiceRecording;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace RepRepair.ViewModels;

public class VoiceReportViewModel : BaseViewModel
{
    //Todo: implement the structure for azurecognitiveservice

    private string _transcribedText;
    private string _translatedText;
    //private bool _isRecording;
    private bool _isTranscriptionVisible;
    private bool _isTranslationVisible;
    private readonly LanguageSettingsService _languageSettingsService;
    public ObservableCollection<string> AvailableLanguages { get; } = new ObservableCollection<string>
    {
        "en-US", "es-ES", "it-IT", "sv-SE", "fr-FR", "fa-IR", "de-DE", "da-DK"
    };
    private readonly IVoiceRecordingService _voiceRecordingService;
    private readonly IAzureCognitiveService _cognitiveServices;
    private readonly TranslatorService _translatorService;
    public ICommand RecordCommand { get; }
    //public ICommand StopCommand { get; }
    public ICommand EditTranscriptionCommand { get; }
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

    public VoiceReportViewModel()
    {
        _languageSettingsService = ServiceHelper.GetService<LanguageSettingsService>();
        RecordCommand = new Command(OnRecord);
        _translatorService = ServiceHelper.GetService<TranslatorService>();
        //StopCommand = new Command(OnStop);
        EditTranscriptionCommand = new Command(EditTranscription);
        DeleteRecordingCommand = new Command(DeleteRecording);
        SubmitCommand = new Command(Submit);
        _cognitiveServices = ServiceHelper.GetService<IAzureCognitiveService>();
        _voiceRecordingService = ServiceHelper.GetService<IVoiceRecordingService>();
    }
    private async void OnRecord()
    {
        try
        {

            // var fileName = await _voiceRecordingService.StartRecordingAsync();
            var transcription = await _cognitiveServices.TranscribeAudioAsync();
            TranscribedText = transcription;
            IsTranscriptionVisible = true;
            Translate(transcription);

            //var transcription = await _cognitiveServices.TranscribeAudioAsync();
            //if ( transcription !=null && transcription.Length >0)
            //{
            //    var translation = await _translatorService.TranslateTextAsync(transcription, "se");
            //    TranscribedText = transcription;
            //    IsTranscriptionVisible = true;
            //    TranslatedText = translation;
            //    IsTranslationVisible = true;
            //}
  
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

        //await _voiceRecordingService.StopRecordingAsync();
    }

    //private async void OnRecord()
    //{
    //    if (!IsRecording)
    //    {
    //        await _voiceRecordingService.StartRecordingAsync();
    //        IsRecording = true;
    //    }
    //}

    //private async void OnStop()
    //{
    //    if (_isRecording)
    //    {
    //        var recordedAudioStream = await _voiceRecordingService.StopRecordingAsync();
    //        if (recordedAudioStream != null)
    //        {
    //            // now we can send this stream to Cognitive Services for transcription
    //            var transcription = await _cognitiveServices.TranscribeAudioAsync(recordedAudioStream);
    //            TranscribedText = transcription;
    //            IsTranscriptionVisible = true;
    //        }
    //        IsRecording = false;
    //    }
    //}

    private async void Translate(string text)
    {
        var textTobeTranslated = text;
        var translate = await _translatorService.TranslateTextAsync(textTobeTranslated, "sv", SelectedLanguage);
        TranslatedText = translate;
        IsTranslationVisible = true;
    }

    private void EditTranscription()
    {
        // Logic to edit the transcription
    }

    private void DeleteRecording()
    {
        // Logic to delete the recording
        // Update IsTranscriptionVisible to false
    }

    private void Submit()
    {
        // Logic to submit the transcription and recording
    }

}
