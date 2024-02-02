using RepRepair.Extensions;
using System.Windows.Input;
using RepRepair.Services.AlertService;
using RepRepair.Models.DatabaseModels;
using RepRepair.Services.ScanningService;

namespace RepRepair.ViewModels;

public class ScanViewModel : BaseViewModel
{
    public ICommand SimulateScan { get; }
    public ICommand OnReport { get; }
    public ObjectInfo ObjectInfo => _scanningService.CurrentScannedObject;
    private readonly IScanningService _scanningService;
    private readonly IAlertService _alertService;
    private bool _isScanned;
    public bool IsScanned { get => _isScanned; }

    public ScanViewModel()
    {
        _alertService = ServiceHelper.GetService<IAlertService>();
        _scanningService = ServiceHelper.GetService<IScanningService>();
        _scanningService.ScannedObjectChanged += (objectInfo) =>
        {
            OnPropertyChanged(nameof(ObjectInfo));
        };
        _scanningService.ScanStateChanged += (isScanned) =>
        {
            _isScanned = isScanned;
            OnPropertyChanged(nameof(IsScanned));
        };
        SimulateScan = new Command(async () => await ScanAsync("MockObjectQRCode"));
        OnReport = new Command(async () => await OnReportAsync());
    }

    private async Task OnReportAsync()
    {
        if (_scanningService.IsScanned && _scanningService.CurrentScannedObject != null)
        {
            await Shell.Current.GoToAsync("//MainReportPage");
        }
        else
        {
            await _alertService.ShowAlertAsync("Alert", "You should scan the machine first!", "OK");
        }
        return;

    }

    public async Task<ObjectInfo> ScanAsync(string qrCode)
    {
        return await _scanningService.ScanAsync(qrCode);
    }

    public void ResetScan() => _scanningService?.ResetScan();
}
