using Microsoft.Identity.Client;
using Microsoft.Maui.Controls;
using RepRepair.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RepRepair.ViewModels
{
    public class SignInViewModel : BaseViewModel // Implement INotifyPropertyChanged in BaseViewModel
    {
        public ICommand SignInCommand { get; }
        

        public SignInViewModel()
        {
            SignInCommand = new Command(async () => await SignInAsync());
        }

        private async Task SignInAsync()
        {
            try
            {
                var authResult = await App._authenticationService.SignInAsync();
                if (authResult != null)
                {
                    // I get nullexception here because the Shell.Current is null, and that is because the 
                    //Shell is not yet set in the constructor
                    await Shell.Current.GoToAsync("//HomePage");
                }
            }
            catch (MsalClientException ex)
            {
                // Handle exceptions from MSAL
                Console.WriteLine(ex.Message);
                // Optionally display an error message to the user
            }
        }


    }
}
