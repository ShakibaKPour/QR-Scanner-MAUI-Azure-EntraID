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

    //    private Timer _utteranceTimer;

    //    public AzureCognitiveService()
    //    {
    //        _languageSettingsService = ServiceHelper.GetService<LanguageSettingsService>();
    //    }

    //    public async Task<string> TranscribeAudioAsync()
    //    {
    //        var transcript = new StringBuilder();
    //        try
    //        {
    //            var speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);
    //            speechConfig.SpeechRecognitionLanguage = _languageSettingsService.CurrentLanguage;

    //            using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
    //            using var speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);

    //            var cancellationTokenSource = new CancellationTokenSource();
    //            cancellationTokenSource.CancelAfter(TimeSpan.FromMinutes(0.5));

    //            _utteranceTimer = new Timer(StopRecognition, speechRecognizer, Timeout.Infinite, Timeout.Infinite);

    //            // Subscribe to events
    //            speechRecognizer.Recognized += async (s, e) =>
    //            {
    //                if (!string.IsNullOrEmpty(e.Result.Text))
    //                {
    //                    transcript.AppendLine(e.Result.Text);
    //                    Console.WriteLine($"Recognized: {e.Result.Text}");

    //                    // Stop after the first recognized utterance
    //                    _utteranceTimer.Change(Timeout.Infinite, Timeout.Infinite);
    //                }
    //            };

    //            speechRecognizer.Canceled += (s, e) =>
    //            {
    //                Console.WriteLine($"Recognition canceled. Reason: {e.Reason}");
    //                cancellationTokenSource.Cancel(); // Cancel on any recognition cancellation
    //            };

    //            speechRecognizer.SessionStopped += (s, e) =>
    //            {
    //                Console.WriteLine("Session stopped.");
    //                cancellationTokenSource.Cancel(); // Ensure the task is cancelled when session stops
    //            };

    //            // Start continuous recognition
    //            _utteranceTimer.Change(TimeSpan.FromSeconds(5), Timeout.InfiniteTimeSpan);
    //            await speechRecognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);

    //            // Wait for the cancellation to be requested
    //            try
    //            {
    //                await Task.Delay(Timeout.Infinite, cancellationTokenSource.Token).ConfigureAwait(false);
    //            }
    //            catch (OperationCanceledException)
    //            {
    //                // Expected upon task cancellation, disregard
    //            }

    //            // Stop continuous recognition
    //            await speechRecognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine($"An error occurred: {ex.Message}");
    //            return "Error transcribing audio";
    //        }
    //        finally
    //        {
    //            _utteranceTimer?.Dispose();
    //        }

    //        return transcript.ToString();
    //    }

    //    private void StopRecognition(object state)
    //    {
    //        var recognizer = state as SpeechRecognizer;
    //        Console.WriteLine("Utterance too long, stopping recognition");
    //        recognizer?.StopContinuousRecognitionAsync().ConfigureAwait(false);

    //    }
    //}
//}



