using Microsoft.Identity.Client;
using RepRepair.Extensions;
using RepRepair.Models;
using RepRepair.Pages;
using RepRepair.Services.AlertService;
using RepRepair.Services.Auth;
using RepRepair.Services.DB;
using RepRepair.Services.ReportTypesService;

namespace RepRepair
{
    //public partial class App : Application
    //{
    //    public static AuthenticationService _authenticationService { get; private set; }
    //    public static IAlertService AlertService;
    //    private readonly LanguageSettingsService _languageSettingsService;
    //    private readonly ReportServiceType _reportServiceType;

    //// Static property to store the AuthenticationResult globally
    ////public static AuthenticationResult AuthenticationResult { get; private set; }


    //public App()
    //    {
    //        InitializeComponent();
    //        _authenticationService = new AuthenticationService();
    //        AlertService = ServiceHelper.GetService<IAlertService>();
    //        _languageSettingsService = ServiceHelper.GetService<LanguageSettingsService>();
    //        _reportServiceType = ServiceHelper.GetService<ReportServiceType>();

    //        InitializeGlobalLanguagesAsync();
    //        InitializeGlobalReportTypesAsync();

    //    // Subscribe to the SignInSuccessful message
    //    MessagingCenter.Subscribe<App>(this, "SignInSuccessful", (sender) =>
    //    {
    //        NavigateToHome();
    //    });

    //    MainPage = new NavigationPage(new SignInPage()); // Start with SignInPage
    //        CheckSignInStatus();
    //    }

    //    private async void CheckSignInStatus()
    //    {
    //        try
    //        {
    //            var authResult = await _authenticationService.AcquireTokenSilentAsync();
    //            if (authResult != null)
    //            {
    //               // AuthenticationResult = authResult;
    //                NavigateToHome(); // Navigate home if silent sign-in succeeds
    //            }
    //        }
    //        catch (MsalUiRequiredException)
    //        {
    //            // If silent sign-in fails, user remains on the SignInPage to sign in manually
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine($"An error occurred during sign-in: {ex.Message}");
    //            // Optionally, navigate to an error page or show an alert
    //        }
    //    }

    //    private async void InitializeGlobalLanguagesAsync()
    //    {
    //        try
    //        {
    //            await _languageSettingsService.FetchAndUpdateAvailableLanguages(ServiceHelper.GetService<IDatabaseService>());
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine($"Failed to initialize languages: {ex.Message}");
    //        }
    //    }

    //    private async void InitializeGlobalReportTypesAsync()
    //    {
    //        try
    //        {
    //            await _reportServiceType.GetReportTypesAsync();
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine($"Failed to initialize report types: {ex.Message}");
    //        }
    //    }

    //    private void NavigateToHome()
    //    {
    //        Device.BeginInvokeOnMainThread(() =>
    //        {
    //            MainPage = new AppShell(); // Ensure Shell is fully initialized
    //            Shell.Current.GoToAsync("//HomePage").ConfigureAwait(false);
    //        });
    //    }

    // Method to store AuthenticationResult and navigate to home
    //public static void StoreAuthenticationResult(AuthenticationResult authResult)
    //{
    //    AuthenticationResult = authResult;
    //    Device.BeginInvokeOnMainThread(() =>
    //    {
    //        Current.MainPage = new AppShell();
    //        Shell.Current.GoToAsync("//HomePage").ConfigureAwait(false);
    //    });
    //}
    // }


    public partial class App : Application
    {
        public static IAuthenticationService _authenticationService { get; private set; }
        private readonly LanguageSettingsService _languageSettingsService;
        private readonly ReportServiceType _reportServiceType;
        //private readonly AppConfiguration _appConfig;
        //private readonly EventAggregator _eventAggregator;
        public static IAlertService AlertSvc;
        public App()
        {
            InitializeComponent();
            _authenticationService = ServiceHelper.GetService<IAuthenticationService>();
            CheckSignInStatus();
            //_appConfig = ServiceHelper.GetService<AppConfiguration>();
           // _eventAggregator = ServiceHelper.GetService<EventAggregator>();
            AlertSvc = ServiceHelper.GetService<IAlertService>();
            _languageSettingsService = ServiceHelper.GetService<LanguageSettingsService>();
            _reportServiceType = ServiceHelper.GetService<ReportServiceType>();
            InitializeGlobalLanguagesAsync();
            InitializeGlobalReportTypesAsync();
            MessagingCenter.Subscribe<App>(this, "SignInSuccessful", (sender) =>
            {
                NavigateToHome();
            });
            MainPage = new AppShell();
        }
        private async void CheckSignInStatus()
        {
            try
            {
                var authResult = await _authenticationService.AcquireTokenSilentAsync();
            }
            catch (MsalUiRequiredException)
            {
                Current.Dispatcher.Dispatch(() =>
                {
                    Current.MainPage = new NavigationPage(new SignInPage());
                });
                //Device.BeginInvokeOnMainThread(() =>
                //{
                //    //Shell.Current.GoToAsync(nameof(SignInPage));
                //    MainPage = new NavigationPage(new SignInPage());
                //});

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                MainPage = new NavigationPage(new HomePage());
            }
        }

        private async void InitializeGlobalLanguagesAsync()
        {
            await _languageSettingsService.FetchAndUpdateAvailableLanguages();
        }

        private async void InitializeGlobalReportTypesAsync()
        {
            await _reportServiceType.GetReportTypesAsync();
        }

        private void NavigateToHome()
        {
            this.Dispatcher.Dispatch(() =>
            {
                MainPage = new AppShell(); 
                Shell.Current.GoToAsync("//HomePage").ConfigureAwait(false);
            });
        }
    }
}

