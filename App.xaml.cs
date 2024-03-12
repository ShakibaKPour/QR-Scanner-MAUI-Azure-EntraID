using Microsoft.Identity.Client;
using RepRepair.Extensions;
using RepRepair.Pages;
using RepRepair.Services.AlertService;
using RepRepair.Services.Auth;
using RepRepair.Services.DB;
using RepRepair.Services.Language;
using RepRepair.Services.ReportTypesService;

namespace RepRepair
{
    public partial class App : Application
    {
        public static AuthenticationService _authenticationService {  get; private set; }
        private readonly LanguageSettingsService _languageSettingsService;
        private readonly ReportServiceType _reportServiceType;
        public static IAlertService AlertSvc;
        public App()
        {
            InitializeComponent();
            _authenticationService = new AuthenticationService();
            CheckSignInStatus();
            AlertSvc = ServiceHelper.GetService<IAlertService>();
            _languageSettingsService = ServiceHelper.GetService<LanguageSettingsService>();
            _reportServiceType = ServiceHelper.GetService<ReportServiceType>();
            InitializeGlobalLanguagesAsync();
            InitializeGlobalReportTypesAsync();
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
                Device.BeginInvokeOnMainThread(() =>
                {
                    //Shell.Current.GoToAsync(nameof(SignInPage));
                    MainPage = new NavigationPage(new SignInPage());
                });

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private async void InitializeGlobalLanguagesAsync()
        {
            await _languageSettingsService.FetchAndUpdateAvailableLanguages(ServiceHelper.GetService<IDatabaseService>());
        }

        private async void InitializeGlobalReportTypesAsync()
        {
            await _reportServiceType.GetReportTypesAsync();
        }
    }
}
