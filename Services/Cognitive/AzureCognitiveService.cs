using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using RepRepair.Extensions;
using RepRepair.Services.AlertService;
using System.Text;

namespace RepRepair.Services.Cognitive;

public class AzureCognitiveService : IAzureCognitiveService
{
    private readonly IAlertService _alertService;
    private readonly LanguageSettingsService _languageSettingsService;
    public AzureCognitiveService()
    {
        _languageSettingsService = ServiceHelper.GetService<LanguageSettingsService>();
        _alertService = ServiceHelper.GetService<IAlertService>();
    }
          


    public async Task<string> TranscribeAudioAsync()
    {
        var textResult = new StringBuilder();

        try
        {
            var speechConfig = SpeechConfig.FromSubscription(Constants.speechKey, Constants.speechRegion);
            speechConfig.SpeechRecognitionLanguage = _languageSettingsService.CurrentLanguage.Language;
            speechConfig.SetProperty(PropertyId.Speech_SegmentationSilenceTimeoutMs, "300");

            var granted = await Permissions.RequestAsync<Permissions.Microphone>();
            if (granted == PermissionStatus.Granted)
            {
                using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();

                using var speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);

                var stopRecognition = new TaskCompletionSource<int>();

                var speechRecognitionResult = await speechRecognizer.RecognizeOnceAsync();
                return speechRecognitionResult.Text;
            } else
            {
                _alertService.ShowAlert("Alert", "Allow the microphone to proceed");
            }
        }
        
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return "No audio found";
    }

}
