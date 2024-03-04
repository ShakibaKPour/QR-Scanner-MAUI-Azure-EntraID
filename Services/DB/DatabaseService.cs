using Newtonsoft.Json;
using RepRepair.Extensions;
using RepRepair.Models;
using RepRepair.Models.DatabaseModels;
using RepRepair.Services.AlertService;
using RepRepair.Services.Cognitive;
using RepRepair.Services.Configuration;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using RepRepair.Services.Auth;

namespace RepRepair.Services.DB;

public class DatabaseService : IDatabaseService
{
    private readonly HttpClient _httpClient;

    private readonly string _baseFunctionUrlGet = "https://reprepair.azurewebsites.net/api/getobjectinfo/qrcode/";

    private readonly string _baseFunctionUrlPost = "https://reprepair.azurewebsites.net/api/InsertReportInfo?code=0WnwDDQvRERDVleGQpwqQlAkYaeAQ5Nx7O0NCGGXFbD-AzFuE8t5zg==";

    private readonly string _baseFunctionUrlGetLanguages = "https://reprepair.azurewebsites.net/api/GetAvailableLanguagesInputBinding?code=4_iT1wdTbRJd29zlmYxIbEIIgYZaB00tXt78by2Ff3QjAzFuJZSYYA==";

    private readonly string _baseFunctionUrlGetReportTypes = "https://reprepair.azurewebsites.net/api/GetReportTypes?code=CRzo1OhhrWjDM5vhVMRTI8T4ZieB4CGrw7jDu57rVxKDAzFuftu_9g==";


    public AppConfig AppConfig
    {
        get => _configurationService.AppConfig;
    }

    private readonly AuthenticationService _authenticationServices;
        
    private readonly ConfigurationService _configurationService;

    private readonly TranslatorService _translatorService;

    public DatabaseService()
    {
        _authenticationServices = ServiceHelper.GetService<AuthenticationService>();
        _httpClient = new HttpClient();
        _translatorService = ServiceHelper.GetService<TranslatorService>();
        _configurationService = ServiceHelper.GetService<ConfigurationService>();     
    }

    public async Task<List<Languages>> GetAvailableLanguagesAsync()
    {
        try
        {
            var requestUrl = $"{_baseFunctionUrlGetLanguages}";
            //var requestUrl = $"{AppConfig.getLanguagesUrl}";
            var response = await _httpClient.GetAsync(requestUrl);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var languages = JsonConvert.DeserializeObject<List<Languages>>(jsonResponse);
                return languages;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
            else
            {
                return null;
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return new List<Languages>();
        }
    }

    public async Task<ObjectInfo?> GetObjectInfoByQRCodeAsync(string qrCode)
    {
        try
        {
            var tokenResult = await _authenticationServices.AcquireTokenSilentAsync();
            var accessToken = tokenResult.AccessToken;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);


            string encodedQRCode = Uri.EscapeDataString(qrCode);
            var requestUrl = $"{_baseFunctionUrlGet}{encodedQRCode}";
            var response = await _httpClient.GetAsync(requestUrl);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var objectInfo = JsonConvert.DeserializeObject<ObjectInfo>(jsonResponse);
                return objectInfo;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
            else
            {
                return null;
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return new ObjectInfo();
        }

    }

    public async Task<List<ReportType>?> GetReportTypesAsync()
    {
        try
        {
            var requestUrl = $"{_baseFunctionUrlGetReportTypes}";
            var response = await _httpClient.GetAsync(requestUrl);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var reportTypes = JsonConvert.DeserializeObject<List<ReportType>>(jsonResponse);
                return reportTypes;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
            else
            {
                return null;
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return new List<ReportType>();
        }
    }

    public async Task<bool> InsertReportAsync(ReportInfo reportData, Languages selectedLanguage)
    {
        if (selectedLanguage.Language == "sv-SE")
        {
            reportData.TranslatedFaultReport = null;
        }
        else
        {
            reportData.TranslatedFaultReport = await _translatorService.TranslateTextAsync(reportData.OriginalFaultReport, "sv-SE", selectedLanguage.Language);
        }
            reportData.QRCode = Guid.Parse(reportData.QRCodeString);

            var content = JsonContent.Create(reportData);
            var response = await _httpClient.PostAsync(_baseFunctionUrlPost, content);

            return response.IsSuccessStatusCode;

    }
}