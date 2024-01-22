using RepRepair.Extensions;
using RepRepair.Models;
using RepRepair.Services.Navigation;
using System.ComponentModel;
using System.Windows.Input;

namespace RepRepair.ViewModels;

public class ReportViewModel : BaseViewModel
{
    private readonly INavigationService _navigationService;
    public ReportInfo Report { get; set; }
    public ICommand NavigateToVoiceRecordCommand { get; set; }

    public ReportViewModel()
    {
        _navigationService = ServiceHelper.GetService<INavigationService>();
        Report = new ReportInfo();
        NavigateToVoiceRecordCommand = new Command(async () => await NavigateToVoiceRecordCommandAsync());
    }

    public override Task InitializeAsync(object parameter)
    {
        if(parameter is ObjectInfo objectInfo) 
        {  
            return Task.FromResult(objectInfo); 
        }
        return base.InitializeAsync(parameter);
    }

    private async Task NavigateToVoiceRecordCommandAsync()
    {
        await _navigationService.NavigateToAsync<VoiceReportViewModel>();
    }

    //public event PropertyChangedEventHandler? PropertyChanged;

    //protected virtual void OnPropertyChanged(string propertyName)
    //{
    //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    //}
}
