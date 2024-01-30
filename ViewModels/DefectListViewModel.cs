using RepRepair.Extensions;
using RepRepair.Models;
using RepRepair.Services.AlertService;
using RepRepair.Services.DB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RepRepair.ViewModels
{

    public class DefectListViewModel : BaseViewModel
    {
        private string _description;
        private readonly IAlertService _alertService;
        private readonly IDatabaseService _databaseService;
        public ObservableCollection<DefectItem> Defects { get; } = new ObservableCollection<DefectItem>();
        public DefectItem? SelectedDefect { get; set; }
        
        public ICommand SubmitDefectCommand { get; }
        public string Description
        {
            get => _description;
            set => _description = value;
        }

        public DefectListViewModel() 
        {
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
               var success = await _databaseService.AddTextReport(Description);
            if (success)
                {
                    await Shell.Current.GoToAsync("Thank You!");
                }

            }
        }
    }
}
