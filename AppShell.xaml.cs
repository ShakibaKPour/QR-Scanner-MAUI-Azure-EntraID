using RepRepair.Pages;

namespace RepRepair
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("VoiceReportPage", typeof(VoiceReportPage));
        }
    }
}
