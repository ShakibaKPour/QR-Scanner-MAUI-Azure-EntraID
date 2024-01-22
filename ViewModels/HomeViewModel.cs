using Microsoft.Maui.Controls;
using RepRepair.Extensions;
using RepRepair.Services.Navigation;
using System.ComponentModel;
using System.Windows.Input;

namespace RepRepair.ViewModels;

public partial class HomeViewModel : BaseViewModel
{
    private readonly INavigationService _navigationService;
    public ICommand ScanCommand { get; set; }

    public HomeViewModel()
    {
        _navigationService = ServiceHelper.GetService<INavigationService>();
        ScanCommand = new Command(async () => await OnScanAsync());

    }

    private async Task OnScanAsync()
    {
        //Navigate to the scanpage
        await _navigationService.NavigateToAsync<ScanViewModel>();
    }

    // public event PropertyChangedEventHandler? PropertyChanged;

    //protected virtual void OnPropertyChanged(string propertyName)
    //{
    //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    //}
}
