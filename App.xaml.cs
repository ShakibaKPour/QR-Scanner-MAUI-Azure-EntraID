using RepRepair.Pages;
using RepRepair.ViewModels;

namespace RepRepair
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }
    }
}
