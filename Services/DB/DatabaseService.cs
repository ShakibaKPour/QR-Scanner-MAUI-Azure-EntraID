using Newtonsoft.Json;
using RepRepair.Models.DatabaseModels;
using System.Text;
using System.Text.Json.Serialization;

namespace RepRepair.Services.DB;

public class DatabaseService : IDatabaseService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseFunctionUrl = "http://localhost:7247/api/getobjectinfo/qrcode/"; // Azure function url

    public DatabaseService()
    {
        _httpClient = new HttpClient();
     
    }

    public async Task<ObjectInfo> GetObjectInfoByQRCodeAsync(string qrCode)
    {
        try
        {
            string encodedQRCode = Uri.EscapeDataString(qrCode);
            var requestUrl = $"{_baseFunctionUrl}{encodedQRCode}";

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
    private List<VoiceMessageInfo> _voiceMessages = new List<VoiceMessageInfo>();
    private ReportInfo _textReport = new ReportInfo();
    private List<ReportInfo> _textReports = new List<ReportInfo>();
    public Task<bool> AddVoiceMessageInfoAsync(VoiceMessageInfo voiceMessageInfo)
    {
        _voiceMessages.Add(voiceMessageInfo);
        return Task.FromResult(true);
    }

    public Task<List<VoiceMessageInfo>> GetAllVoiceMessagesAsync()
    {
        return Task.FromResult(_voiceMessages);
    }

    //public Task<ObjectInfo> GetObjectInfoByQRCodeAsync(string qrCode)
    //{
    //    if (qrCode == "MockObjectQRCode")
    //    {
    //        return Task.FromResult(new ObjectInfo
    //        {
    //            Name = "Mock Object",
    //            Location = "ObjectLocation",
    //            QRCode = "MockObjectQRCode"
    //        });
    //    }
    //    return Task.FromResult<ObjectInfo>(null);
    //}

    public async Task<bool> InsertReportAsync(ReportInfo reportData)
    {
        var json = JsonConvert.SerializeObject(reportData);
        var content= new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(_baseFunctionUrl, content); //adjust the _baseFunctionUrl

        return response.IsSuccessStatusCode;

    }
}
