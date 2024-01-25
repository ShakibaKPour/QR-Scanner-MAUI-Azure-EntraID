using RepRepair.Models;
using RepRepair.Services.Navigation;
using RepRepair.ViewModels;

namespace RepRepair.Pages;

public partial class MainReportPage : ContentPage
{
    //TODO Implement the language selection 
    public MainReportPage()
    {
        InitializeComponent();
        BindingContext = new ReportViewModel();
    }

}
