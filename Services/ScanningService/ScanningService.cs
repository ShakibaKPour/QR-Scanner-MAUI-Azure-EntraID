using RepRepair.Models.DatabaseModels;
using RepRepair.Services.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepRepair.Services.ScanningService
{
        public class ScanningService : IScanningService
        {
            private readonly IDatabaseService _databaseService;

            public ScanningService(IDatabaseService databaseService)
            {
                _databaseService = databaseService;
                
            }

            public event Action<ObjectInfo> ScannedObjectChanged;
            public event Action<bool> ScanStateChanged;

            public ObjectInfo CurrentScannedObject { get; private set; }
            public bool IsScanned { get; private set; }

            public async Task<ObjectInfo> ScanAsync(string qrCode)
            {
                var scannedObject = await _databaseService.GetObjectInfoByQRCodeAsync(qrCode);
                if (scannedObject != null)
                {
                    CurrentScannedObject = scannedObject;
                    IsScanned = true;
                    ScannedObjectChanged?.Invoke(CurrentScannedObject);
                    ScanStateChanged?.Invoke(IsScanned);
                }
                return CurrentScannedObject;
            }

            public void ResetScan()
            {
                if(CurrentScannedObject != null)
                {
                    CurrentScannedObject = null;
                    IsScanned = false;
                    ScannedObjectChanged?.Invoke(CurrentScannedObject);
                    ScanStateChanged?.Invoke(IsScanned);
            }
            else
            {
                Console.WriteLine("currentscannedobject is null");
            }
            
        }
        
    }
 }
