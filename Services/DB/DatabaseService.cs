using RepRepair.Models;

namespace RepRepair.Services.DB;

public class DatabaseService : IDatabaseService
{
    public Task<ObjectInfo> GetObjectInfoByQRCodeAsync(string qrCode)
    {
        throw new NotImplementedException();
    }
}
