namespace RepRepair.Services.VoiceRecording
{
    public interface IVoiceRecordingService
    {
        Task StartRecordingAsync();
        Task<Stream> StopRecordingAsync();

    }
}