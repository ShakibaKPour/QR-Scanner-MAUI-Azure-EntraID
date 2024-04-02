using Newtonsoft.Json;
using RepRepair.Extensions;
using RepRepair.Models;
using RepRepair.Models.DatabaseModels;
using RepRepair.Services.Cognitive;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using RepRepair.Services.Auth;
using RepRepair.Services.AlertService;

namespace RepRepair.Services.DB;

public class DatabaseService : IDatabaseService
{
    private readonly HttpClient _httpClient;

    private readonly IAuthenticationService _authenticationServices;

    private readonly IAlertService _alertService;

    private readonly TranslatorService _translatorService;
    private readonly AppConfiguration _config;

    public DatabaseService()
    {
        _authenticationServices = ServiceHelper.GetService<IAuthenticationService>();
        _config = ServiceHelper.GetService<AppConfiguration>();
        _httpClient = new HttpClient();
        _translatorService = ServiceHelper.GetService<TranslatorService>();
        _alertService = ServiceHelper.GetService<IAlertService>();
    }

    private async Task SetAuthenticationHeaderAsync()
    {
        try
        {
            var tokenResult = await _authenticationServices.AcquireTokenSilentAsync();
            var accessToken = tokenResult.AccessToken;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to acquire token: {ex.Message}");
            //await _alertService.ShowAlertAsync("Failed to authenticate! Try to sign in again!", "OK");
            throw new ApplicationException("Authentication failed.", ex);
        }

    }

    private static async Task<T?> DeserializeResponseContent<T>(HttpResponseMessage response)
    {
        try
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(jsonResponse);
        }
        catch (JsonException jsonEx)
        {
            Console.WriteLine($"JSON parsing error: {jsonEx.Message}");
            throw new ApplicationException("Error parsing response content.", jsonEx);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            throw;
        }
    }

    public async Task<List<Languages>> GetAvailableLanguagesAsync()
    {
        try
        {
            await SetAuthenticationHeaderAsync();
            var requestUrl = _config.BaseFunctionUrlGetLanguages;//$"{_baseFunctionUrlGetLanguages}";
            var response = await _httpClient.GetAsync(requestUrl);
            if (response.IsSuccessStatusCode)
            {
                return await DeserializeResponseContent<List<Languages>>(response);
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
            await SetAuthenticationHeaderAsync();
            string encodedQRCode = Uri.EscapeDataString(qrCode);
            var requestUrl = $"{_config.BaseFunctionUrlGet}{encodedQRCode}";
            var response = await _httpClient.GetAsync(requestUrl);
            if (response.IsSuccessStatusCode)
            {
                return await DeserializeResponseContent<ObjectInfo?>(response);
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
            await SetAuthenticationHeaderAsync();
            var requestUrl = _config.BaseFunctionUrlGetDefectList; //$"{_baseFunctionUrlGetDefectList}";
            var response = await _httpClient.GetAsync(requestUrl);
            if (response.IsSuccessStatusCode)
            {
                return await DeserializeResponseContent<List<DefectList>>(response);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new List<DefectList>();
            }
            else
            {
                return new List<DefectList>();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return new List<DefectList>();
        }
    }

    public async Task<List<ReportType>?> GetReportTypesAsync()
    {
        try
        {
            await SetAuthenticationHeaderAsync();
            var requestUrl = _config.BaseFunctionUrlGetReportTypes;//$"{_baseFunctionUrlGetReportTypes}";
            var response = await _httpClient.GetAsync(requestUrl);
            if (response.IsSuccessStatusCode)
            {
                return await DeserializeResponseContent<List<ReportType>>(response);
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
        try
        {
            await SetAuthenticationHeaderAsync();

            reportData.TranslatedFaultReport = selectedLanguage.Language == "sv-SE" ?
                reportData.OriginalFaultReport :
                await _translatorService.TranslateTextAsync(reportData.OriginalFaultReport, "sv-SE", selectedLanguage.Language);
            reportData.QRCode = Guid.Parse(reportData.QRCodeString);

            var content = JsonContent.Create(reportData);
            var response = await _httpClient.PostAsync(_config.BaseFunctionUrlPost, content);

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred in {nameof(InsertReportAsync)}: {ex.Message}");
            return false;
        }
    }

}