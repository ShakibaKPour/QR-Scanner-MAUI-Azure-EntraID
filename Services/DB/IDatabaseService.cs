using RepRepair.Models;

namespace RepRepair.Services.DB;

public interface IDatabaseService
{
    Task<ObjectInfo> GetObjectInfoByQRCodeAsync(string qrCode);
}
