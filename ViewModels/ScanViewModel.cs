using RepRepair.Extensions;
using System.Windows.Input;
using RepRepair.Services.AlertService;
using RepRepair.Models.DatabaseModels;
using RepRepair.Services.ScanningService;
using RepRepair.Pages;

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
        SimulateScan = new Command(async () => await ScanAsync("6F9619FF-8B86-D011-B42D-00C04FC964FF"));
        OnReport = new Command(async () => await OnReportAsync());
    }

    private async Task OnReportAsync()
    {
        if (_scanningService.IsScanned && _scanningService.CurrentScannedObject != null)
        {
            await Shell.Current.GoToAsync("//MainReportPage");
        }
        //else if(_scanningService.CurrentScannedObject == null)
        //{
        //    await _alertService.ShowAlertAsync("Alert", "Scanned object does not exist in the database", "OK");
        //}
        else if (!_scanningService.IsScanned)
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
