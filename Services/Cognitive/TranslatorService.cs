using RepRepair.Extensions;
using RepRepair.Services.Language;
using System.Net.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepRepair.Models;

namespace RepRepair.Services.Cognitive
{
    public class TranslatorService
    {
        private readonly string _key; // "fb58a08c2f454884bc7b434f40794193";
        private readonly string _endpoint; // "https://api.cognitive.microsofttranslator.com";
        private readonly string _location;
       // private readonly LanguageSettingsService _languageSettingsService;

        // location, also known as region.
        // required if you're using a multi-service or regional (not global) resource. It can be found in the Azure portal on the Keys and Endpoint page.
        //"swedencentral";

        public TranslatorService(string key, string endpoint, string location)
        {
            _key = key;
            _endpoint = endpoint;
            _location = location;
           // _languageSettingsService= ServiceHelper.GetService<LanguageSettingsService>();
        }

        public async Task<string> TranslateTextAsync(string textToTranslate, string targetLanguage, string selectedLanguage)
        {
            string route = $"/translate?api-version=3.0&from={selectedLanguage}&to={targetLanguage}";
            object[] body = new object[] { new { Text = textToTranslate } };
            var requestBody = JsonConvert.SerializeObject(body);

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(_endpoint + route);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", _key);
                request.Headers.Add("Ocp-Apim-Subscription-Region", _location);

                HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
                string result = await response.Content.ReadAsStringAsync();
                try
                {
                    var translationResponses = JsonConvert.DeserializeObject<List<TranslationResponse>>(result);
                    if (translationResponses != null && translationResponses.Count > 0)
                    {
                        return translationResponses[0].translations[0].text;
                    }
                }
                catch (JsonSerializationException ex)
                {
                    Console.WriteLine($"Error deserializing translation response: {ex.Message}");
                }

            }
            return null;

        }

    }
}
