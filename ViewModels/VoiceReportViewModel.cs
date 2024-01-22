using Plugin.Maui.Audio;
using RepRepair.Extensions;
using RepRepair.Models;
using RepRepair.Services.Cognitive;
using RepRepair.Services.VoiceRecording;
using System.ComponentModel;
using System.Windows.Input;

namespace RepRepair.ViewModels;

public class VoiceReportViewModel : BaseViewModel
{
    //Todo: implement the structure for azurecognitiveservice and inject it here

    private string _transcribedText;
    private bool _isRecording;
    private bool _isTranscriptionVisible;
    private readonly IVoiceRecordingService _voiceRecordingService;
    private readonly IAzureCognitiveService _cognitiveServices;

    public string TranscribedText
    {
        get => _transcribedText;
        set
        {
            _transcribedText = value;
            OnPropertyChanged(nameof(TranscribedText));
        }
    }

    public bool IsRecording
    {
        get => _isRecording;
        set
        {
            _isRecording = value;
            OnPropertyChanged(nameof(IsRecording));
        }
    }

    public bool IsTranscriptionVisible
    {
        get => _isTranscriptionVisible;
        set
        {
            _isTranscriptionVisible = value;
            OnPropertyChanged(nameof(IsTranscriptionVisible));
        }
    }

    public ICommand RecordCommand { get; }
    public ICommand EditTranscriptionCommand { get; }
    public ICommand DeleteRecordingCommand { get; }
    public ICommand SubmitCommand { get; }

    public VoiceReportViewModel()
    {
        RecordCommand = new Command(OnRecord);
        EditTranscriptionCommand = new Command(EditTranscription);
        DeleteRecordingCommand = new Command(DeleteRecording);
        SubmitCommand = new Command(Submit);
        _cognitiveServices = ServiceHelper.GetService<IAzureCognitiveService>();
        _voiceRecordingService = ServiceHelper.GetService<IVoiceRecordingService>();
    }

    private async void OnRecord()
    {
        // Logic to start or stop recording
        // Update IsRecording and IsTranscriptionVisible as needed

        if (!IsRecording)
        {
            await _voiceRecordingService.StartRecordingAsync();
        }
        else
        {
            var recordedAudioStream = await _voiceRecordingService.StopRecordingAsync();
            // now we can send this stream to Cognitive Services for transcription
            //var transcription = await _cognitiveServices.TranscribeAudioAsync(audioStream);
            //TranscribedText = transcription;
            //IsTranscriptionVisible = true;
        }

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
