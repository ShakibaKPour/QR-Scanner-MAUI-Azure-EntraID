using Microsoft.Maui.Controls;
using RepRepair.Services.Navigation;
using System.ComponentModel;
using System.Windows.Input;

namespace RepRepair.ViewModels;

public partial class HomeViewModel : BaseViewModel
{
    private readonly INavigationService _navigationService;
    public ICommand ScanCommand { get; set; }

    public HomeViewModel(INavigationService  navigationService)
    {
        _navigationService = navigationService;
        ScanCommand = new Command(async () => await OnScanAsync());

    }

    private async Task OnScanAsync()
    {
        //Navigate to the scanpage
        await _navigationService.NavigateToAsync<ScanViewModel>();
    }

   // public event PropertyChangedEventHandler? PropertyChanged;
}
