using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using RepRepair.Extensions;
using RepRepair.Services.Language;

namespace RepRepair.Services.Cognitive
{
    public class AzureCognitiveService : IAzureCognitiveService
    {
        //private readonly HttpClient _httpClient;
        // private readonly string _baseUri = "https://the-azure-function-app.azurewebsites.net/";
        static string speechKey = Environment.GetEnvironmentVariable("SPEECH_KEY");
        static string speechRegion = Environment.GetEnvironmentVariable("SPEECH_REGION");
        private readonly LanguageSettingsService _languageSettingsService;

        public AzureCognitiveService()
        {
            _languageSettingsService = ServiceHelper.GetService<LanguageSettingsService>();
        }


        public async Task<string> TranscribeAudioAsync()
        {
            try
            {
                // Initialize speech configuration
                var speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);
                speechConfig.SpeechRecognitionLanguage = _languageSettingsService.CurrentLanguage;

                // Create an audio configuration using the mic
                using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();

                // Create a speech recognizer using the audio configuration
                using var speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);

                // Perform speech recognition on the audio file
                var speechRecognitionResult = await speechRecognizer.RecognizeOnceAsync();
                return speechRecognitionResult.Text;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "No audio found";
        }

    }
}
