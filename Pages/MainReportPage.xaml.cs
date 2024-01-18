using RepRepair.Services.Navigation;
using RepRepair.ViewModels;

namespace RepRepair;

public partial class MainReportPage : ContentPage
{
    //TODO Implement the language selection 
    public MainReportPage(INavigationService navigationService)
	{
		InitializeComponent();
		BindingContext = new ReportViewModel(navigationService);
		
	}

}