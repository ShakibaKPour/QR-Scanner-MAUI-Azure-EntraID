using Newtonsoft.Json;
using RepRepair.Extensions;
using RepRepair.Models;
using RepRepair.Models.DatabaseModels;
using RepRepair.Services.Cognitive;
using RepRepair.Services.Configuration;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using RepRepair.Services.Auth;

namespace RepRepair.Services.DB;

public class DatabaseService : IDatabaseService
{
    private readonly HttpClient _httpClient;

    private readonly string _baseFunctionUrlGet = "https://reprepair.azurewebsites.net/api/getobjectinfo/qrcode/";

    private readonly string _baseFunctionUrlPost = "https://reprepair.azurewebsites.net/api/InsertToReportAndLinkTables?";

    private readonly string _baseFunctionUrlGetLanguages = "https://reprepair.azurewebsites.net/api/GetAvailableLanguages?";

    private readonly string _baseFunctionUrlGetReportTypes = "https://reprepair.azurewebsites.net/api/GetReportTypes?";

    private readonly string _baseFunctionUrlGetDefectList = "https://reprepair.azurewebsites.net/api/GetDefectList?";


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
            var tokenResult = await _authenticationServices.AcquireTokenSilentAsync();
            Console.WriteLine(tokenResult.ToString());
            var accessToken = tokenResult.AccessToken;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
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

    public async Task<List<DefectList>> GetDefectListAsync()
    {
        try
        {
            var tokenResult = await _authenticationServices.AcquireTokenSilentAsync();
            var accessToken = tokenResult.AccessToken;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var requestUrl = $"{_baseFunctionUrlGetDefectList}";
            var response = await _httpClient.GetAsync(requestUrl);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var defectList = JsonConvert.DeserializeObject<List<DefectList>>(jsonResponse);
                return defectList;
            }
            else if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new List<DefectList>();
            }
            else
            {
                return new List<DefectList>();
            }
        }catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return new List<DefectList>();
        }
    }

    public async Task<List<ReportType>?> GetReportTypesAsync()
    {
        try
        {
            var tokenResult = await _authenticationServices.AcquireTokenSilentAsync();
            Console.WriteLine(tokenResult.ToString());
            var accessToken = tokenResult.AccessToken;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

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
        var tokenResult = await _authenticationServices.AcquireTokenSilentAsync();
        Console.WriteLine(tokenResult.ToString());
        var accessToken = tokenResult.AccessToken;
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        if (selectedLanguage.Language == "sv-SE")
        {
            reportData.TranslatedFaultReport = reportData.OriginalFaultReport;
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