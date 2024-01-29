using RepRepair.Extensions;
using RepRepair.Services.DB;
using RepRepair.Services.Navigation;
using System.Windows.Input;
using RepRepair.Models;

namespace RepRepair.ViewModels;

public class ScanViewModel : BaseViewModel
{
    private readonly IDatabaseService _databaseService;
    private ObjectInfo objectInfo;
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

    public ScanViewModel()
    {
        _databaseService = ServiceHelper.GetService<IDatabaseService>();
        OnBarcodeDetected = new Command<string>(LoadInfo);
        SimulateScan = new Command(SimulateLoadInfoAsync);
        OnReport = new Command(async () => await OnReportAsync());
    }

    private async Task OnReportAsync()
    {
        await Shell.Current.GoToAsync("//MainReportPage");
        MessagingCenter.Send(this, "ObjectInfoMessage", objectInfo);
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
        UpdateObjectProperties(objectInfo);

    }


    public async void LoadInfo(string qrCode)
    {
            objectInfo = await _databaseService.GetObjectInfoByQRCodeAsync(qrCode);
        if (objectInfo != null)
        {
            ObjectInfo = objectInfo;
        }

        UpdateObjectProperties(objectInfo);       
    }

}
