using RepRepair.Extensions;
using RepRepair.Services.DB;
using RepRepair.Services.Navigation;
using System.Windows.Input;
using RepRepair.Models;
using RepRepair.Services.AlertService;
using System.Diagnostics;

namespace RepRepair.ViewModels;

public class ScanViewModel : BaseViewModel
{
    private readonly IDatabaseService _databaseService;
    private readonly IAlertService _alertService;
    private ObjectInfo objectInfo;
    private bool _isScanned = false;
    public ICommand OnBarcodeDetected { get; }
    public ICommand SimulateScan { get; }
    public ICommand OnReport { get; }
    public ObjectInfo ObjectInfo
    {
        get => objectInfo;
        set
        {
            if (objectInfo != value)
            {
                objectInfo= value;
                OnPropertyChanged(nameof(ObjectInfo));
                UpdateObjectProperties(objectInfo);
            }
        }
    }
    public bool IsScanned
    {
        get => _isScanned;
        set
        {
            _isScanned= value;
            OnPropertyChanged(nameof(IsScanned));
        }
    }
    public ScanViewModel()
    {
        _databaseService = ServiceHelper.GetService<IDatabaseService>();
        _alertService = ServiceHelper.GetService<IAlertService>();
        OnBarcodeDetected = new Command<string>(LoadInfo);
        SimulateScan = new Command(SimulateLoadInfoAsync);
        OnReport = new Command(async () => await OnReportAsync());
    }

    private async Task OnReportAsync()
    {
        if(objectInfo != null)
        {
            await Shell.Current.GoToAsync("//MainReportPage");
            MessagingCenter.Send(this, "ObjectInfoMessage", objectInfo);
        }
        else
        {
           await _alertService.ShowAlertAsync("Alert", "You should scan the machine first!", "OK");
        }
        return;

    }

    private void UpdateObjectProperties(ObjectInfo obj)
    {
        if (obj != null)
        {
            OnPropertyChanged(nameof(ObjectInfo.Name));
            OnPropertyChanged(nameof(ObjectInfo.ObjectId));
            OnPropertyChanged(nameof(ObjectInfo.Location));
            OnPropertyChanged(nameof(ObjectInfo.QRCode));
        }

    }
    public async void SimulateLoadInfoAsync()
    {
        var objectInfo = await _databaseService.GetObjectInfoByQRCodeAsync("MockObjectQRCode");
        if (objectInfo != null)
        {
            ObjectInfo = objectInfo;
        }
        IsScanned = true;
        MessagingCenter.Send(this, "UpdateReportTabVisibility", IsScanned);
        UpdateObjectProperties(objectInfo);
       
    }


    public async void LoadInfo(string qrCode)
    {
         var  objectInfo = await _databaseService.GetObjectInfoByQRCodeAsync(qrCode);
        if (objectInfo != null)
        {
            ObjectInfo = objectInfo;
        }
        IsScanned = true;
        MessagingCenter.Send(this, "UpdateReportTabVisibility", IsScanned);
        UpdateObjectProperties(objectInfo);       
    }

    public void ResetScan()
    {
        ObjectInfo = null;
        IsScanned = false;
        OnPropertyChanged(nameof(IsScanned));
        MessagingCenter.Send(this, "UpdateReportTabVisibility", IsScanned);
    }
}
