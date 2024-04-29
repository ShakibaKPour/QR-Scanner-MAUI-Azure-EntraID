using RepRepair.Pages;

namespace RepRepair
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
            Routing.RegisterRoute(nameof(SignInPage), typeof(SignInPage));
            Routing.RegisterRoute(nameof(VoiceReportPage), typeof(VoiceReportPage));
            Routing.RegisterRoute(nameof(ThankYouPage), typeof(ThankYouPage));
            Routing.RegisterRoute(nameof(WriteReportPage), typeof(WriteReportPage));
            Routing.RegisterRoute(nameof(DefectListPage), typeof(DefectListPage));
            BindingContext = this;
        }

    }
}
