using RepRepair.Models;

namespace RepRepair.Services.DB;

public class DatabaseService : IDatabaseService
{
    private List<VoiceMessageInfo> _voiceMessages = new List<VoiceMessageInfo>();
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
}
