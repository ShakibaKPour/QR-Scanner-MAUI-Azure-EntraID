using RepRepair.Models.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
