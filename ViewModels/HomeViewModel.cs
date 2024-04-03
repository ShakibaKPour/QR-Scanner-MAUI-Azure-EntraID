using Microsoft.Identity.Client;
using RepRepair.Extensions;
using RepRepair.Services.AlertService;
using System.Windows.Input;

namespace RepRepair.ViewModels;

public class HomeViewModel : BaseViewModel
{
    private readonly IAlertService _alertService;
        public ICommand ScanCommand { get; private set; }
        public ICommand SignOutCommand { get; private set; }

        public HomeViewModel()
        {
            _alertService = ServiceHelper.GetService<IAlertService>();
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            ScanCommand = new Command(async () => await OnScanAsync());
            SignOutCommand = new Command(async () => await SignOutAsync());
        }

        private async Task OnScanAsync()
        {
            try
            {
                await Shell.Current.GoToAsync("//ScanPage");
            }
            catch (Exception ex)
            {
                _alertService.ShowAlert("Error","An error occurred while navigating to ScanPage");
            }
        }

        private async Task SignOutAsync()
        {
        try
        {
                await App._authenticationService.SignOutAsync();
            }
            catch (MsalClientException)
        {
                _alertService.ShowAlert("Error","An error occurred during sign out");
            }
        }
    }