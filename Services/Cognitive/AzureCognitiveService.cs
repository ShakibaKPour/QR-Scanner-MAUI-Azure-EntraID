using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepRepair.Services.Cognitive
{
    public class AzureCognitiveService : IAzureCognitiveService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUri = "https://the-azure-function-app.azurewebsites.net/";

        public AzureCognitiveService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        //public async Task<string> TranscribeAudioAsync(Stream audioStream)
        //{
        //    // Implement the logic to send the voice file to the Azure Function and get the transcription
        //}

        //public async Task<string> TranslateTextAsync(string text, string targetLanguage)
        //{
        //    // Implement the logic to send the text to the Azure Function for translation
        //}

        // Check the services at the other project reprepairserverside
    }
}
