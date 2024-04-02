using RepRepair.Extensions;
using RepRepair.Models.DatabaseModels;
using RepRepair.Pages;
using RepRepair.Services.AlertService;
using RepRepair.Services.DB;
using RepRepair.Services.Language;
using RepRepair.Services.ReportTypesService;
using RepRepair.Services.ScanningService;
using System.Windows.Input;

namespace RepRepair.ViewModels
{

    public class DefectListViewModel : BaseViewModel
    {
        public List<ReportType> ReportTypes { get => _reportServiceType.CachedReportTypes; }
        public ObjectInfo ObjectInfo => _scanningService.CurrentScannedObject;
        public ICommand SubmitDefectCommand { get; }
        public ICommand OnRefresh { get; set; }
        private readonly IScanningService _scanningService;
        private readonly IAlertService _alertService;
        private readonly IDatabaseService _databaseService;
        private readonly LanguageSettingsService _languageSettingsService;
        private readonly ReportServiceType _reportServiceType;
        private List<DefectList> _defects;
        public List<DefectList> Defects
        {
            get { return _defects; }
            set 
            {
                _defects = value;
                OnPropertyChanged(nameof(Defects));
            }
        }

        private DefectList? _selectedDefect;
        public DefectList SelectedDefect
        {
            get => _selectedDefect;
            set
            {
                _selectedDefect = value;
                OnPropertyChanged(nameof(SelectedDefect));
            }
        }
        public Languages SelectedLanguage
        {
            get => _languageSettingsService.CurrentLanguage;
            set
            {
                if (_languageSettingsService.CurrentLanguage != value)
                {
                    _languageSettingsService.CurrentLanguage = value;
                    _languageSettingsService.CurrentLanguage = value;
                    OnPropertyChanged(nameof(SelectedLanguage));
                }
            }
        }

        public DefectListViewModel() 
        {
            _languageSettingsService = ServiceHelper.GetService<LanguageSettingsService>();
            _scanningService= ServiceHelper.GetService<IScanningService>();
            _scanningService.ScannedObjectChanged += (objectInfo) =>
            {
                OnPropertyChanged(nameof(ObjectInfo));
            };
            _alertService = ServiceHelper.GetService<IAlertService>();
            _reportServiceType = ServiceHelper.GetService<ReportServiceType>();
            _databaseService = ServiceHelper.GetService<IDatabaseService>();
            LoadDefectList();
            OnRefresh = new Command(async () => await RefreshCommandExecuted());
            SubmitDefectCommand = new Command(SubmitDefect);
            ValidateIsScanned();
        }
        public async Task RefreshCommandExecuted()
        {
            await _languageSettingsService.RefreshAvailableLanguages(ServiceHelper.GetService<IDatabaseService>());
            await _reportServiceType.RefreshReportTypes();
            await LoadDefectList();
        }

        private async Task LoadDefectList()
        {
            var list = await _databaseService.GetDefectListAsync();
            Defects = list;


           
        }

        private async void ValidateIsScanned()
        {
            if (ObjectInfo == null)
            {
                await _alertService.ShowAlertAsync("Alert", "Start by scanning the QR code", "OK");
                await Shell.Current.GoToAsync("///ScanPage");
            }
        }

        private async void SubmitDefect(object obj)
        {
            Console.WriteLine($"Selected defect description: {SelectedDefect?.Description}");
            if (SelectedDefect == null)
            {
                 await _alertService.ShowAlertAsync("Alert", "Choose an alternative", "OK");
                return;
            }
            else
            {
                var reportType = ReportTypes.Where(r => r.TypeOfReport == "Defect List").FirstOrDefault();

                if (reportType == null)
                {
                    await _reportServiceType.RefreshReportTypes();
                    reportType = ReportTypes.Where(r => r.TypeOfReport == "Defect List").FirstOrDefault();
                }

                var newReportData = new ReportInfo
                {
                    SelectedLanguage = _languageSettingsService.CurrentLanguage.ID,
                    OriginalFaultReport = SelectedDefect.Description,
                    TranslatedFaultReport = null,
                    TypeOfReport = reportType?.ID,
                    QRCodeString = ObjectInfo.QRCode,
                };
                Console.WriteLine($"Original fault : {newReportData.OriginalFaultReport}");
                var success = await _databaseService.InsertReportAsync(newReportData, SelectedLanguage);
                if (success)
                { 
                    await Shell.Current.GoToAsync(nameof(ThankYouPage));
                    _scanningService.ResetScan();
                }

            }
        }
    }
}
