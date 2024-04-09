using RepRepair.Extensions;
using RepRepair.Models.DatabaseModels;
using RepRepair.Services.AlertService;
using RepRepair.Services.DB;

namespace RepRepair.Services.ScanningService;

public class ScanningService : IScanningService
{
    private readonly IDatabaseService _databaseService;
    private readonly IAlertService _alertService;

    public ScanningService()
    {
        _databaseService = ServiceHelper.GetService<IDatabaseService>();
        _alertService = ServiceHelper.GetService<IAlertService>();
    }
    public event Action<ObjectInfo?> ScannedObjectChanged;
    public event Action<bool> ScanStateChanged;

    public ObjectInfo? CurrentScannedObject { get; private set; }
    public bool IsScanned { get; private set; }

    public async Task<ObjectInfo?> ScanAsync(string qrCode)
    {
        try
        {
            var scannedObject = await _databaseService.GetObjectInfoByQRCodeAsync(qrCode);
            if (scannedObject != null)
            {
                CurrentScannedObject = scannedObject;
                IsScanned = true;
                ScannedObjectChanged?.Invoke(CurrentScannedObject);
                ScanStateChanged?.Invoke(IsScanned);
            }
            else
            {
                await _alertService.ShowAlertAsync("Alert", "Scanned object does not exist in the database.");
                IsScanned = false;
            }
            return CurrentScannedObject;
        }
        catch (Exception ex)
        {
            await _alertService.ShowAlertAsync("Error", $"Something went wrong: {ex.Message}");
            IsScanned = false;
            return null;
        }
    }

    public void ResetScan()
    {
        CurrentScannedObject = null;
        IsScanned = false;
        ScannedObjectChanged?.Invoke(CurrentScannedObject);
        ScanStateChanged?.Invoke(IsScanned);
    }
}