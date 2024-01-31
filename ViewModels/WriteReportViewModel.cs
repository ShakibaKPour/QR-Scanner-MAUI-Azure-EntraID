using RepRepair.Extensions;
using RepRepair.Models.DatabaseModels;
using RepRepair.Pages;
using RepRepair.Services.AlertService;
using RepRepair.Services.DB;
using RepRepair.Services.Language;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RepRepair.ViewModels
{
    public class WriteReportViewModel : BaseViewModel
    {
        private string _reportText;
        private ObjectInfo _objectInfo;
        private readonly IAlertService _alertService;
        private readonly IDatabaseService _databaseService;
        private readonly LanguageSettingsService _languageSettingsService;
        public ObservableCollection<string> AvailableLanguages { get; } = new ObservableCollection<string>
    {
        "en-US", "es-ES", "it-IT", "sv-SE", "fr-FR", "fa-IR", "de-DE", "da-DK"
    };
        public ICommand OnSubmit { get; set; }

        public string ReportText
        {
            get => _reportText;
            set => _reportText = value;
        }
        public ObjectInfo ObjectInfo
        {
            get => _objectInfo;
            set => _objectInfo = value;
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
            _alertService = ServiceHelper.GetService<IAlertService>();
            _databaseService = ServiceHelper.GetService<IDatabaseService>();
            _languageSettingsService = ServiceHelper.GetService<LanguageSettingsService>();
            SubscribeToMessages();
            OnSubmit = new Command(SubmitEmail);
        }

        private void SubscribeToMessages()
        {
            MessagingCenter.Subscribe<ReportViewModel, ObjectInfo>(this, "ObjectInfoMessage", (sender, arg) =>
            {
                _objectInfo = arg;
                OnPropertyChanged(nameof(ObjectInfo)); // Notify UI about the change
            });
        }
        private async void SubmitEmail(object obj)
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
               // UpdateJointTable();
            }
            else
            {
                await _alertService.ShowAlertAsync("Error", "Could not submit", "OK");
            }
        }

        //private async void UpdateJointTable()
        //{
        //    //implement a logic to get the addedReport, attach it to the objectID from th eobjectinfo and create an entry at the defect table

        //}
    }
}
