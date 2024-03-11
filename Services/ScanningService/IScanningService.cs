using RepRepair.Models.DatabaseModels;

namespace RepRepair.Services.ScanningService
{
    public interface IScanningService
    {
        event Action<ObjectInfo> ScannedObjectChanged;
        event Action<bool> ScanStateChanged;
        Task<ObjectInfo?> ScanAsync(string qrCode);
        void ResetScan();
        ObjectInfo CurrentScannedObject { get; }
        bool IsScanned { get; }
    }
}
