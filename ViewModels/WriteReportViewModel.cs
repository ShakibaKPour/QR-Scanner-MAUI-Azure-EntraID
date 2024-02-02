using RepRepair.Extensions;
using RepRepair.Models.DatabaseModels;
using RepRepair.Services.AlertService;
using RepRepair.Services.DB;
using RepRepair.Services.Language;
using RepRepair.Services.ScanningService;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace RepRepair.ViewModels
{
    public class WriteReportViewModel : BaseViewModel
    {
        public ObservableCollection<string> AvailableLanguages { get; } = new ObservableCollection<string>
        {
            "en-US", "es-ES", "it-IT", "sv-SE", "fr-FR", "fa-IR", "de-DE", "da-DK"
        };
        public ICommand OnSubmit { get; set; }
        public ObjectInfo ObjectInfo => _scanningService.CurrentScannedObject;
        private readonly IScanningService _scanningService;
        private readonly IAlertService _alertService;
        private readonly IDatabaseService _databaseService;
        private readonly LanguageSettingsService _languageSettingsService;
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
        public string SelectedLanguage
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
            OnSubmit = new Command(async () => await SubmitTextReport());
        }

        private async Task SubmitTextReport()
        {
            if (string.IsNullOrEmpty(ReportText))
            {
                await _alertService.ShowAlertAsync("Alert", "No input", "OK");
                return;
            }

            var success = await _databaseService.AddTextReport(ReportText);
            if (success)
            {
                await Shell.Current.GoToAsync("Thank You!");
                ClearFields();
               _scanningService.ResetScan();
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