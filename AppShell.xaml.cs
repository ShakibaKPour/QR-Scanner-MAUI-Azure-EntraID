using RepRepair.Pages;
using RepRepair.Services.Navigation;
using RepRepair.ViewModels;

namespace RepRepair
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("VoiceReport", typeof(VoiceReportPage));
        }
    }
}
