using RepRepair.Extensions;
using System.Windows.Input;
using RepRepair.Services.AlertService;
using RepRepair.Models.DatabaseModels;
using RepRepair.Services.ScanningService;

namespace RepRepair.ViewModels;

    public class ScanViewModel : BaseViewModel
    {
        private readonly IScanningService _scanningService;
        private readonly IAlertService _alertService;

        public ICommand SimulateScan { get; private set; }
        public ICommand OnReport { get; private set; }

        public ObjectInfo ObjectInfo => _scanningService.CurrentScannedObject;

        private bool _isScanned;
        public bool IsScanned => _isScanned;

        public ScanViewModel()
        {
            _scanningService = ServiceHelper.GetService<IScanningService>();
            _alertService = ServiceHelper.GetService<IAlertService>();

            SubscribeToServiceEvents();
            InitializeCommands();
        }

        private void SubscribeToServiceEvents()
        {
            _scanningService.ScannedObjectChanged += (objectInfo) =>
            {
                _isScanned = objectInfo != null;
                OnPropertyChanged(nameof(ObjectInfo));
                OnPropertyChanged(nameof(IsScanned));
            };
        }

        private void InitializeCommands()
        {
            SimulateScan = new Command(async () => await ScanAsync("6F9619FF-8B86-D011-B42D-00C04FC964FF"));
            OnReport = new Command(async () => await OnReportAsync());
        }

        private async Task OnReportAsync()
        {
            if (_isScanned && ObjectInfo != null)
            {
                await Shell.Current.GoToAsync("//MainReportPage");
            }
            else
            {
                await _alertService.ShowAlertAsync("Alert", "You should scan the machine first!", "OK");
            }
        }

        public async Task<ObjectInfo?> ScanAsync(string qrCode)
        {
            var scannedObject = await _scanningService.ScanAsync(qrCode);
            _isScanned = scannedObject != null;
            OnPropertyChanged(nameof(IsScanned));
            return scannedObject;
        }

        public void ResetScan() => _scanningService?.ResetScan();
}