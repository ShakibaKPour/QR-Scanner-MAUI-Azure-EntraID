using Plugin.Maui.Audio;

namespace RepRepair.Services.VoiceRecording
{
    public interface IVoiceRecordingService
    {
        Task<string> StartRecordingAsync();
        Task<IAudioSource> StopRecordingAsync();

    }
}