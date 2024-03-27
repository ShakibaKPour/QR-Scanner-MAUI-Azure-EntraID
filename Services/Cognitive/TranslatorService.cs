﻿using Newtonsoft.Json;
using System.Text;

namespace RepRepair.Services.Cognitive;

public class TranslatorService
{
    static string translateKey = "fb58a08c2f454884bc7b434f40794193";
    static string translateRegion = "swedencentral";
    static string translateEndpoint = "https://api.cognitive.microsofttranslator.com";
    public TranslatorService()
    {

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
            request.RequestUri = new Uri(translateEndpoint + route);
            request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
            request.Headers.Add("Ocp-Apim-Subscription-Key", translateKey);
            request.Headers.Add("Ocp-Apim-Subscription-Region", translateRegion);
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
