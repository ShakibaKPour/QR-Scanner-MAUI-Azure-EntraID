using RepRepair.Extensions;
using RepRepair.Models.DatabaseModels;
using RepRepair.Pages;
using RepRepair.Services.AlertService;
using RepRepair.Services.DB;
using RepRepair.Services.ReportTypesService;
using RepRepair.Services.ScanningService;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace RepRepair.ViewModels
{
    public class WriteReportViewModel : BaseViewModel
    {
        private readonly IScanningService _scanningService;
        private readonly IAlertService _alertService;
        private readonly IDatabaseService _databaseService;
        private readonly LanguageSettingsService _languageSettingsService;
        private readonly ReportServiceType _reportServiceType;

        public ICommand OnSubmit { get; private set; }
        public ICommand OnRefresh { get; private set; }
        public ICommand OnGoBack { get; private set; }

        private string _reportText = string.Empty;
        public string ReportText
        {
            get => _reportText;
            set
            {
                if (_reportText != value)
                {
                    _reportText = value;
                    OnPropertyChanged(nameof(ReportText));
                }
            }
        }

        public ObjectInfo ObjectInfo => _scanningService.CurrentScannedObject;

        public List<ReportType> ReportTypes => _reportServiceType.CachedReportTypes;

        public ObservableCollection<Languages> AvailableLanguages => _languageSettingsService.AvailableLanguages;

        public Languages SelectedLanguage
        {
            get => _languageSettingsService.CurrentLanguage;
            set
            {
                if (_languageSettingsService.CurrentLanguage != value)
                {
                    _languageSettingsService.CurrentLanguage = value;
                    OnPropertyChanged(nameof(SelectedLanguage));
                }
            }
        }

        public WriteReportViewModel()
        {
            _scanningService = ServiceHelper.GetService<IScanningService>();
            _alertService = ServiceHelper.GetService<IAlertService>();
            _databaseService = ServiceHelper.GetService<IDatabaseService>();
            _languageSettingsService = ServiceHelper.GetService<LanguageSettingsService>();
            _reportServiceType = ServiceHelper.GetService<ReportServiceType>();

            InitializeCommands();
            ValidateIsScanned();
        }

        private void InitializeCommands()
        {
            OnSubmit = new Command(async () => await ExecuteWithTryCatch(SubmitTextReport));
            OnRefresh = new Command(async () => await ExecuteWithTryCatch(RefreshLanguagesCommandExecuted));
            OnGoBack = new Command(async () => await ExecuteWithTryCatch(NavigateBackAsync));
        }

        private async Task ExecuteWithTryCatch(Func<Task> action)
        {
            try
            {
                await action();
            }
            catch (Exception ex)
            {
                await _alertService.ShowAlertAsync("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        private async Task NavigateBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }

        private async Task RefreshLanguagesCommandExecuted()
        {
            await _languageSettingsService.RefreshAvailableLanguages();
            await _reportServiceType.RefreshReportTypes();
        }

        private async Task SubmitTextReport()
        {
            if (string.IsNullOrEmpty(ReportText))
            {
                await _alertService.ShowAlertAsync("Alert", "Please enter a report text.", "OK");
                return;
            }

            var reportType = ReportTypes.FirstOrDefault(r => r.TypeOfReport == "Write Message");
            if (reportType == null)
            {
                await _reportServiceType.RefreshReportTypes();
                reportType = ReportTypes.Where(r => r.TypeOfReport == "Write Message").FirstOrDefault();
            }

            var newReportData = new ReportInfo
            {
                SelectedLanguage = _languageSettingsService.CurrentLanguage.ID,
                OriginalFaultReport = ReportText,
                TypeOfReport = reportType?.ID,
                QRCodeString = ObjectInfo.QRCode,
            };

            var success = await _databaseService.InsertReportAsync(newReportData, SelectedLanguage);
            if (success)
            {
                await Shell.Current.GoToAsync(nameof(ThankYouPage));
                _scanningService.ResetScan();
                ClearFields();
            }
            else
            {
                await _alertService.ShowAlertAsync("Error", "Submission failed. Please try again.", "OK");
            }
        }

        private void ClearFields()
        {
            ReportText = string.Empty;
        }

        private async void ValidateIsScanned()
        {
            if (ObjectInfo == null)
            {
                await _alertService.ShowAlertAsync("Alert", "Start by scanning the QR code", "OK");
                await Shell.Current.GoToAsync("///ScanPage");
            }
        }
    }
}