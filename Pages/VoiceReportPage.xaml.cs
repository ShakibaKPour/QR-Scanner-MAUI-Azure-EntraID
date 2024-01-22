using Plugin.Maui.Audio;
using RepRepair.Services.Cognitive;
using RepRepair.Services.VoiceRecording;
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