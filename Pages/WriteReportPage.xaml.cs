using RepRepair.ViewModels;

namespace RepRepair.Pages;

public partial class WriteReportPage : ContentPage
{
	public WriteReportPage()
	{
		InitializeComponent();
		BindingContext = new WriteReportViewModel();
	}
}