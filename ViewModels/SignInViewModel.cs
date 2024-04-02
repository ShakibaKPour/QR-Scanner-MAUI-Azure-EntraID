using Microsoft.Identity.Client;
using RepRepair.Extensions;
using RepRepair.Models;
using RepRepair.Pages;
using System.Windows.Input;

namespace RepRepair.ViewModels
{
    public class SignInViewModel : BaseViewModel // Implement INotifyPropertyChanged in BaseViewModel
    {
        public ICommand SignInCommand { get; }
        //public static _authenticationService _authenticationService { get; private set; }
        //private readonly EventAggregator _eventAggregator;

        public SignInViewModel()
        {
            //_authenticationService = ServiceHelper.GetService<_authenticationService>();
            SignInCommand = new Command(async () => await SignInAsync());
           // _eventAggregator= ServiceHelper.GetService<EventAggregator>();
        }

        private async Task SignInAsync()
        {
            try
            {
                var authResult =  await App._authenticationService.SignInAsync();
                if (authResult != null)
                {
                    //_eventAggregator.Publish<App>(new App());
                    //Notify the app that sign -in was successful
                    MessagingCenter.Send<App>(Application.Current as App, "SignInSuccessful");
                    // App.StoreAuthenticationResult(authResult);
                }
            }
            catch (MsalClientException ex)
            {
                Console.WriteLine(ex.Message);
                // Handle specific sign-in exceptions here (e.g., show an error message)
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred during sign-in: {ex.Message}");
                // Handle general exceptions here (e.g., show an error message)
            }
        }


    }
}
