using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using RepRepair.Extensions;
using RepRepair.Services.Configuration;
using RepRepair.Services.Language;
using System.Text;

namespace RepRepair.Services.Cognitive
{
    public class AzureCognitiveService : IAzureCognitiveService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUri = "https://the-azure-function-app.azurewebsites.net/";
        static string speechKey = "e1e2299f4ccd49e3b2c3859420c5ae25";
        static string speechRegion = "swedencentral";
        //static string speechKey = Environment.GetEnvironmentVariable("SPEECH_KEY");
        //static string speechRegion = Environment.GetEnvironmentVariable("SPEECH_REGION");
        private readonly LanguageSettingsService _languageSettingsService;
        private readonly ConfigurationService _configurationService;
        public AzureCognitiveService()
        {
            _languageSettingsService = ServiceHelper.GetService<LanguageSettingsService>();
            _configurationService = ServiceHelper.GetService<ConfigurationService>();
        }


        public async Task<string> TranscribeAudioAsync()
        {
            var textResult = new StringBuilder();
           // var cancellationTokenSource = new CancellationTokenSource();

            try
            {
                var speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);
                speechConfig.SpeechRecognitionLanguage = _languageSettingsService.CurrentLanguage.Language;
                speechConfig.SetProperty(PropertyId.Speech_SegmentationSilenceTimeoutMs, "300");
                using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();

                using var speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);

                var stopRecognition = new TaskCompletionSource<int>();

                var speechRecognitionResult = await speechRecognizer.RecognizeOnceAsync();
                return speechRecognitionResult.Text;

                //speechRecognizer.Recognized += (s, e) =>
                //{
                //    if (e.Result.Reason == ResultReason.RecognizedSpeech)
                //    {
                //        textResult.AppendLine(e.Result.Text);
                //    }
                //};

                //speechRecognizer.Canceled += (s, e) =>
                //{
                //    stopRecognition.TrySetResult(0);
                //};

                //speechRecognizer.SessionStopped += (s, e) =>
                //{
                //    stopRecognition.TrySetResult(0);
                //};
                //await speechRecognizer.StartContinuousRecognitionAsync();

                //var recognitionLimitTimer = new System.Timers.Timer(20000);
                //recognitionLimitTimer.Elapsed += (sender, args) =>
                //{
                //    recognitionLimitTimer?.Stop();
                //    speechRecognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);
                //};
                //recognitionLimitTimer.Start();
                //await stopRecognition.Task;
                //await speechRecognizer.StopContinuousRecognitionAsync();
                //return textResult.ToString();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
  //          finally { cancellationTokenSource.Cancel(); }

            return "No audio found";
        }

    }
}
