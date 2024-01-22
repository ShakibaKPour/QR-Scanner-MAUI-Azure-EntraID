using Plugin.Maui.Audio;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public async Task StartRecordingAsync()
        {
            if (await Permissions.RequestAsync<Permissions.Microphone>() != PermissionStatus.Granted)
            {
                // Inform user about the permission status
                return;
            }

            if (!_audioRecorder.IsRecording)
            {
                await _audioRecorder.StartAsync(); // maybe providing a file path?
            }
            
        }

        public async Task<Stream> StopRecordingAsync()
        {    
            if (!_audioRecorder.IsRecording)
            {
                var recordedAudio = await _audioRecorder.StopAsync();
                return recordedAudio.GetAudioStream();
            }
            return null;
        }
    }
}
