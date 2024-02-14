﻿using Newtonsoft.Json;
using RepRepair.Extensions;
using RepRepair.Models.DatabaseModels;
using RepRepair.Services.AlertService;
using RepRepair.Services.Cognitive;
using System.Text;
using System.Text.Json.Serialization;

namespace RepRepair.Services.DB;

public class DatabaseService : IDatabaseService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseFunctionUrlGet = "http://localhost:7247/api/getobjectinfo/qrcode/";
    private readonly string _baseFunctionUrlPost = "http://localhost:7247/api/InsertReportInfo";
    private readonly TranslatorService _translatorService;

    public DatabaseService()
    {
        _httpClient = new HttpClient();
        _translatorService = ServiceHelper.GetService<TranslatorService>();
     
    }

    public async Task<ObjectInfo?> GetObjectInfoByQRCodeAsync(string qrCode)
    {
        try
        {
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
    public async Task<bool> InsertReportAsync(ReportInfo reportData)
    {
        if (reportData.SelectedLanguage == "sv-SE")
        {
            reportData.TranslatedFaultReport = null;
        }
        else
        {
            reportData.TranslatedFaultReport = await _translatorService.TranslateTextAsync(reportData.OriginalFaultReport, "sv", reportData.SelectedLanguage);
        }
            reportData.QRCode = Guid.Parse(reportData.QRCodeString);
            var json = JsonConvert.SerializeObject(reportData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_baseFunctionUrlPost, content);

            return response.IsSuccessStatusCode;

    }
}
