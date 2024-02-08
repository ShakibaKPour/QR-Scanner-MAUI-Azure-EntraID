using Newtonsoft.Json;
using RepRepair.Extensions;
using RepRepair.Models.DatabaseModels;
using RepRepair.Services.AlertService;
using System.Text;
using System.Text.Json.Serialization;

namespace RepRepair.Services.DB;

public class DatabaseService : IDatabaseService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseFunctionUrlGet = "http://localhost:7247/api/getobjectinfo/qrcode/";
    private readonly string _baseFunctionUrlPost = "http://localhost:7247/api/InsertReportInfo";

    public DatabaseService()
    {
        _httpClient = new HttpClient();
     
    }

    public async Task<ObjectInfo> GetObjectInfoByQRCodeAsync(string qrCode)
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
        var json = JsonConvert.SerializeObject(reportData);
        var content= new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(_baseFunctionUrlPost, content); 

        return response.IsSuccessStatusCode;

    }
}
