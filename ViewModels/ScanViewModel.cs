using RepRepair.Extensions;
using RepRepair.Services.DB;
using RepRepair.Services.Navigation;
using System.ComponentModel;
using System.Windows.Input;

namespace RepRepair.ViewModels;

public class ScanViewModel : BaseViewModel
{
    private readonly INavigationService _navigationService; 
    private readonly IDatabaseService _databaseService;
    private string qrCode;
    private string objectName;
    private string objectLocation;
    private int objectId;
    public ICommand OnBarcodeDetected { get; }
    public string QRCode
    {
        get => qrCode;
        set
        {
            qrCode = value;
            OnPropertyChanged(nameof(QRCode));
        }

    }
    public string ObjectName 
    {
        get => objectName;
        set 
        {
            objectName = value;
            OnPropertyChanged(nameof(objectName));
        }
    }
    public string ObjectLocation
    {
        get => objectLocation;
        set
        {
            objectLocation = value;
            OnPropertyChanged(nameof(ObjectLocation));
        }
    }
    public int ObjectId
    {
        get => objectId;
        set
        {
            objectId = value;
            OnPropertyChanged(nameof(ObjectId));
        }
    }
    public ScanViewModel()
    {
        _databaseService = ServiceHelper.GetService<IDatabaseService>();
        _navigationService = ServiceHelper.GetService<INavigationService>();
        OnBarcodeDetected = new Command<string>(LoadInfo);
    }


    public async void LoadInfo(string qrCode)
    {
            QRCode = qrCode;
            var objectInfo = await _databaseService.GetObjectInfoByQRCodeAsync(qrCode);
            if (objectInfo != null)
            {
                ObjectName = objectInfo.Name;
                ObjectId = objectInfo.ObjectId;
                ObjectLocation = objectInfo.Location;

                await _navigationService.NavigateToAsync<ReportViewModel>(objectInfo);
            }
    }

}
