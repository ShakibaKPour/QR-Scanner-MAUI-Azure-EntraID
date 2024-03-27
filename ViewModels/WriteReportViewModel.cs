using RepRepair.Extensions;
using RepRepair.Models.DatabaseModels;
using RepRepair.Pages;
using RepRepair.Services.AlertService;
using RepRepair.Services.DB;
using RepRepair.Services.Language;
using RepRepair.Services.ReportTypesService;
using RepRepair.Services.ScanningService;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace RepRepair.ViewModels
{
    public class WriteReportViewModel : BaseViewModel
    {
        public ICommand OnSubmit { get; set; }
        public ICommand OnRefresh { get; set; }
        public ICommand OnGoBack { get; set; }
        public ObjectInfo ObjectInfo => _scanningService.CurrentScannedObject;
        private readonly IScanningService _scanningService;
        private readonly IAlertService _alertService;
        private readonly IDatabaseService _databaseService;
        private readonly LanguageSettingsService _languageSettingsService;
        private readonly ReportServiceType _reportServiceType;
        //private readonly TranslatorService _translatorService;
        private string _reportText;
        public string ReportText
        {
            get => _reportText;
            set
            {
                _reportText = value;
                OnPropertyChanged(nameof(ReportText));
            }
        }
        public List<ReportType> ReportTypes { get => _reportServiceType.CachedReportTypes; }
        public ObservableCollection<Languages> AvailableLanguages 
        {
            get => _languageSettingsService.AvailableLanguages;
        }

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
            //_translatorService = ServiceHelper.GetService<TranslatorService>();
            OnSubmit = new Command(async () => await SubmitTextReport());
            OnRefresh = new Command(async ()=>await RefreshLanguagesCommandExecuted());
            OnGoBack = new Command(async () => await NavigateBackAsync());
            ValidateIsScanned();
        }

        private async void ValidateIsScanned()
        {
            if(ObjectInfo == null)
            {
                await _alertService.ShowAlertAsync("Alert", "Start by scanning the QR code", "OK");
                await Shell.Current.GoToAsync("///ScanPage");
            }
        }

        private async Task NavigateBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }

        public async Task RefreshLanguagesCommandExecuted()
        {
            await _languageSettingsService.RefreshAvailableLanguages(ServiceHelper.GetService<IDatabaseService>());
        }


        private async Task<List<Languages>> GetAvilableLanguages()
        {
            var languages = await _databaseService.GetAvailableLanguagesAsync();
            return languages;
        }

        private async Task SubmitTextReport()
        {
            if (string.IsNullOrEmpty(ReportText))
            {
                await _alertService.ShowAlertAsync("Alert", "No input", "OK");
                return;
            }
            //var textTobeTranslated = ReportText;
            //var translation = await _translatorService.TranslateTextAsync(textTobeTranslated, "sv", SelectedLanguage);

            var reportType = ReportTypes.Where(r => r.TypeOfReport == "Write Message").FirstOrDefault();

            var newReportData = new ReportInfo
            {
                SelectedLanguage = _languageSettingsService.CurrentLanguage.ID,
                OriginalFaultReport = ReportText,
                //TranslatedFaultReport = translation,
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
                await _alertService.ShowAlertAsync("Error", "Could not submit", "OK");
            }
        }

        private void ClearFields()
        {
            ReportText = string.Empty;
            OnPropertyChanged(nameof(ReportText));
        }

    }
}