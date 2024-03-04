using Microsoft.Identity.Client;
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
                    // Navigate to the main page after successful sign-in
                    // Adjust navigation based on your app structure
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
