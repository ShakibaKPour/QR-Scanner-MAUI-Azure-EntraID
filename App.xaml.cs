using RepRepair.Extensions;
using RepRepair.Services.AlertService;

namespace RepRepair
{
    public partial class App : Application
    {
       
        public static IAlertService AlertSvc;
        public App()
        {
            InitializeComponent();
            AlertSvc = ServiceHelper.GetService<IAlertService>();
            MainPage = new AppShell();
        }
    }
}
