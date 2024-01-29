using RepRepair.ViewModels;

namespace RepRepair.Pages;

public partial class MainReportPage : ContentPage
{
    public MainReportPage()
    {
        InitializeComponent();
        BindingContext = new ReportViewModel();
    }
}
