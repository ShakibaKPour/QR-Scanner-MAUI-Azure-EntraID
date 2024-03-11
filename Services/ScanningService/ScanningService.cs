using RepRepair.Extensions;
using RepRepair.Models.DatabaseModels;
using RepRepair.Services.AlertService;
using RepRepair.Services.DB;

namespace RepRepair.Services.ScanningService
{
    public class ScanningService : IScanningService
        {
            private readonly IDatabaseService _databaseService;
            private readonly IAlertService _alertService;

            public ScanningService()
            {
                _databaseService = ServiceHelper.GetService<IDatabaseService>();
                _alertService = ServiceHelper.GetService<IAlertService>();
                
            }

            public event Action<ObjectInfo> ScannedObjectChanged;
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
                    return CurrentScannedObject;
                }
                else
                {
                    _alertService.ShowAlert("Alert", "Scanned object does not exist in the database");
                    return null;
                    // Decide what should be done here and what is actually returned from here
                    //_alertService.ShowConfirmation("Alert", "Scanned object does not exist in the database: Do you want to try again?", 
                    //    RetryScanning,"Yes", "No");
                }

            }catch (Exception ex)
            {
                _alertService.ShowAlert("Alert", $"Something went wrong {ex.Message}");
                return null;
            }
            }

        private async void RetryScanning(bool answer)
        {
            if (answer) ResetScan();
            else await Shell.Current.GoToAsync("///HomePage");
        }

        public void ResetScan()
            {
                //if(CurrentScannedObject != null)
                //{
                    CurrentScannedObject = null;
                    IsScanned = false;
                    ScannedObjectChanged?.Invoke(CurrentScannedObject);
                    ScanStateChanged?.Invoke(IsScanned);
            //}
            //else
            //{
            //    Console.WriteLine("currentscannedobject is null");
            //}
            
        }
        
    }
 }
