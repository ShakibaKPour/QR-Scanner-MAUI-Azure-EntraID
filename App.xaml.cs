using RepRepair.Extensions;
using RepRepair.Services.AlertService;
using RepRepair.Services.Configuration;
using RepRepair.Services.DB;
using RepRepair.Services.Language;
using RepRepair.Services.ReportTypesService;

namespace RepRepair
{
    public partial class App : Application
    {
        private readonly LanguageSettingsService _languageSettingsService;
        private readonly ReportServiceType _reportServiceType;
        private readonly ConfigurationService _configurationService;
        public static IAlertService AlertSvc;
        public App()
        {
            InitializeComponent();
            AlertSvc = ServiceHelper.GetService<IAlertService>();
            _configurationService = ServiceHelper.GetService<ConfigurationService>();
            //InitializeAppConfig();
            _languageSettingsService = ServiceHelper.GetService<LanguageSettingsService>();
            _reportServiceType = ServiceHelper.GetService<ReportServiceType>();
            InitializeGlobalLanguagesAsync();
            InitializeGlobalReportTypesAsync();
            MainPage = new AppShell();
        }

        //private async void InitializeAppConfig()
        //{
        //    await _configurationService.GetAppConfiguration();
        //}

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
