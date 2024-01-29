using RepRepair.ViewModels;

namespace RepRepair.Pages;

public partial class VoiceReportPage : ContentPage
{
	public VoiceReportPage()
	{
		InitializeComponent();
        BindingContext = new VoiceReportViewModel();
    }
}