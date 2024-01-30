using RepRepair.Models.DatabaseModels;

namespace RepRepair.Services.DB;

public class DatabaseService : IDatabaseService
{
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

    public Task<ObjectInfo> GetObjectInfoByQRCodeAsync(string qrCode)
    {
        if (qrCode== "MockObjectQRCode")
        {
            return Task.FromResult(new ObjectInfo
            {
                Name = "Mock Object",
                Location = "ObjectLocation",
                ObjectId = 1,
                QRCode = "MockObjectQRCode"
        });
        }
        return Task.FromResult<ObjectInfo>(null);
    }

    public Task<bool> AddTextReport(string textReport)
    {

        _textReport.TextContent = textReport;
        _textReport.ReportedDate = DateTime.Now;
        _textReport.VoiceMessageId = null;
        _textReports.Add(_textReport);

        return Task.FromResult(true);
    }
}
