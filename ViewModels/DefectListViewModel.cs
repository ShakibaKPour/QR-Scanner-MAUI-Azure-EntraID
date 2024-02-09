using RepRepair.Extensions;
using RepRepair.Models;
using RepRepair.Models.DatabaseModels;
using RepRepair.Pages;
using RepRepair.Services.AlertService;
using RepRepair.Services.DB;
using RepRepair.Services.ScanningService;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace RepRepair.ViewModels
{

    public class DefectListViewModel : BaseViewModel
    {
        public ObservableCollection<DefectItem> Defects { get; } = new ObservableCollection<DefectItem>();
        public DefectItem? SelectedDefect { get; set; }
        public ObjectInfo ObjectInfo => _scanningService.CurrentScannedObject;
        public ICommand SubmitDefectCommand { get; }
        private readonly IScanningService _scanningService;
        private readonly IAlertService _alertService;
        private readonly IDatabaseService _databaseService;
        private string _description;
        public string Description
        {
            get => _description;
            set => _description = value;
        }

        public DefectListViewModel() 
        {
            _scanningService= ServiceHelper.GetService<IScanningService>();
            _scanningService.ScannedObjectChanged += (objectInfo) =>
            {
                OnPropertyChanged(nameof(ObjectInfo));
            };
            _alertService = ServiceHelper.GetService<IAlertService>();
            _databaseService = ServiceHelper.GetService<IDatabaseService>();
            LoadDefects();
            SubmitDefectCommand = new Command(SubmitDefect);
        }

        private void LoadDefects()
        {
            Defects.Add(new DefectItem { Description = "problem1" });
            Defects.Add(new DefectItem { Description = "problem2" });
        }

        private async void SubmitDefect(object obj)
        {
            if (SelectedDefect == null)
            {
                 await _alertService.ShowAlertAsync("Alert", "Choose an alternative", "OK");
                return;
            }
            else
            {
                var newReportData = new ReportInfo
                {
                    SelectedLanguage = "English",
                    OriginalFaultReport = Description,
                    TranslatedFaultReport = null,
                    TypeOfReport = "Defect List",
                    //QRCode = ObjectInfo.QRCode,
                    ObjectId = ObjectInfo.ObjectId,
                   // ReportDate = DateTime.Now,
                };
                var success = await _databaseService.InsertReportAsync(newReportData);
                if (success)
                { 
                    await Shell.Current.GoToAsync(nameof(ThankYouPage));
                    _scanningService.ResetScan();
                }

            }
        }

        //private void ClearFields()
        //{
        //    SelectedDefect = null;
        //    OnPropertyChanged(nameof(SelectedDefect));

        //}
    }
}
