using Plugin.Maui.Audio;
using RepRepair.Models;
using System.ComponentModel;
using System.Windows.Input;

namespace RepRepair.ViewModels;

public class VoiceReportViewModel : BaseViewModel
{
    private readonly IAudioManager _audioManager;
    private readonly IAudioRecorder _audioRecorder;
    public VoiceMessageInfo VoiceMessage { get; set; }
    public ICommand RecordCommand { get; private set; }
    public ICommand ReviewTranscriptionCommand { get; private set; }
    public ICommand SubmitCommand { get; private set; }

    public VoiceReportViewModel(IAudioManager audioManager)
    {
        _audioManager = audioManager;
        _audioRecorder = audioManager.CreateRecorder();
        VoiceMessage = new VoiceMessageInfo();
        RecordCommand = new Command(async () => await RecordVoiceMessageAsync());
        ReviewTranscriptionCommand = new Command(ReviewTranscription);
        SubmitCommand = new Command(Submit);
    }
    private async Task RecordVoiceMessageAsync()
    {
        if (await Permissions.RequestAsync<Permissions.Microphone>() != PermissionStatus.Granted)
        {
            // Inform user about the permission status
            return;
        }

        if (!_audioRecorder.IsRecording)
        {
            await _audioRecorder.StartAsync(); // Consider providing a file path
        }
        else
        {
            var recordedAudio = await _audioRecorder.StopAsync();
            var player = _audioManager.CreatePlayer(recordedAudio.GetAudioStream());
            player.Play();
        }
    }

    private void ReviewTranscription() // check what parameter I should send to MS cognitive services and how to add parameter to receive the recordedAudio
    {
        // Logic to submit the voice message to MS Cognitive services
        // do the compution in the azure functions where it is transcripted and if not swedish, translated
        // return the transcription and translation, deserialize or tostring which will be shown on the page
    }
    private void EditTranscription() // add parameter to receive the transcripted
    {
        //edit transcription

         
    }

    private void Submit(object obj) 
    {
        // logic to receive the trasncirption and translation as parameter
        // sending it to the backend. the database shoudl be updating columns in voicemessage table, reportTable, defecttable
        // or maybe all this logic can be in a utils and reused in other ways of reporting like emails and choose from the text too
    }

}
