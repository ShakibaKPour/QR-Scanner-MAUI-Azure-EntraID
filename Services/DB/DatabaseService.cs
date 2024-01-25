using RepRepair.Models;

namespace RepRepair.Services.DB;

public class DatabaseService : IDatabaseService
{
    public Task<ObjectInfo> GetObjectInfoByQRCodeAsync(string qrCode)
    {
        if (qrCode== "MockObjectQRCode")
        {
            return Task.FromResult(new ObjectInfo
            {
                Name = "Mock Object",
                Location = "ObjectLocation",
                ObjectId = 1
            });
        }
        return Task.FromResult<ObjectInfo>(null);
    }
}
