using Plugin.Maui.Audio;

namespace RepRepair.Services.VoiceRecording
{
    public  class VoiceRedordingService : IVoiceRecordingService
    {
        private readonly IAudioManager _audioManager;
        private readonly IAudioRecorder _audioRecorder;

        public VoiceRedordingService(IAudioManager audioManager)
        {
            _audioManager = audioManager;
            _audioRecorder = audioManager.CreateRecorder();
        }
        public async Task<string> StartRecordingAsync()
        {
            if (await Permissions.RequestAsync<Permissions.Microphone>() != PermissionStatus.Granted)
            {
                // Inform user about the permission status
                return "This function needs to use the mic! Allow the permission to proceed";
            }

            string fileName = $"recording_{DateTime.Now:yyyyMMddHHmmss}.wav";
            string filePath = Path.Combine(FileSystem.CacheDirectory, fileName);

            // Start recording and save to the filePath
            await _audioRecorder.StartAsync(filePath);

            return filePath;


        }

        public async Task<IAudioSource> StopRecordingAsync()
        {    

            return await _audioRecorder.StopAsync();
                
        }
    }
}
