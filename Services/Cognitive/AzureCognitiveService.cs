using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech.Translation;
using RepRepair.Extensions;
using RepRepair.Services.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

                //Create an audi config using a file
                //using var audioConfig = AudioConfig.FromWavFileInput(filepath);

                // Create a speech recognizer using the audio configuration
                using var speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);

                // Perform speech recognition on the audio file
                var speechRecognitionResult = await speechRecognizer.RecognizeOnceAsync();
                return speechRecognitionResult.Text;
            }
            //try
            //{
            //    if (recordedAudioStream != null && recordedAudioStream.Length > 0)
            //    {
            //        recordedAudioStream.Position = 0; // Reset stream position to the beginning

            //        // Create a push stream to feed the audio data into the speech service
            //        using var pushStream = AudioInputStream.CreatePushStream();
            //        using (var binaryReader = new BinaryReader(recordedAudioStream))
            //        {
            //            byte[] buffer = new byte[4096];
            //            int bytesRead;
            //            while ((bytesRead = binaryReader.Read(buffer, 0, buffer.Length)) > 0)
            //            {
            //                pushStream.Write(buffer, bytesRead);
            //            }
            //        }
            //        pushStream.Close();

            //        // Create an audio configuration that uses the push stream
            //        using var audioConfig = AudioConfig.FromStreamInput(pushStream);

            //        // Initialize speech configuration
            //        var speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);
            //        speechConfig.SpeechRecognitionLanguage = "en-US";

            //        // Create a speech recognizer using the audio configuration
            //        using var speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);

            //        // Perform speech recognition on the provided audio stream
            //        var speechRecognitionResult = await speechRecognizer.RecognizeOnceAsync();
            //        return speechRecognitionResult.Text;
            //    }
            //}
            //try
            //{
            //    using var audioStream = new MemoryStream();
            //    if (recordedAudioStream != null)
            //    {
            //        await recordedAudioStream.CopyToAsync(audioStream);
            //        audioStream.Position = 0;
            //        // Implement the logic to send the voice file to the Azure Function and get the transcription
            //        var speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);
            //        speechConfig.SpeechRecognitionLanguage = "en-US"; // change hardcoded en-US to the selected language variable

            //        using var pushStream = AudioInputStream.CreatePushStream();
            //        using var audioConfig = AudioConfig.FromStreamInput(pushStream);
            //        using var speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);

            //        var speechRecognitionResult = await speechRecognizer.RecognizeOnceAsync();
            //        return speechRecognitionResult.Text;
            //    }
            //}
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "No audio found";
        }

        //public async Task<string> TranslateTextAsync(string filename)
        //{
        //    // Implement the logic to send the text to the Azure Function for translation
        //    var speechTranslationConfig = SpeechTranslationConfig.FromSubscription(speechKey, speechRegion);
        //    speechTranslationConfig.SpeechRecognitionLanguage = _languageSettingsService.CurrentLanguage;
        //    speechTranslationConfig.AddTargetLanguage("sv"); 

        //    using var audioConfig = AudioConfig.FromWavFileInput(filename);
        //    using var translationRecognizer = new TranslationRecognizer(speechTranslationConfig, audioConfig);

        //    var translationRecognitionResult = await translationRecognizer.RecognizeOnceAsync();
        //    return translationRecognitionResult.Text;
        //}

        // Check the services at the other project reprepairserverside
    }
}
