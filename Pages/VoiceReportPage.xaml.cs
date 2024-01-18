using Plugin.Maui.Audio;
using RepRepair.ViewModels;

namespace RepRepair;

public partial class VoiceReportPage : ContentPage
{
	public VoiceReportPage()
	{
		InitializeComponent();
		var audioManager = AudioManager.Current;
		BindingContext = new VoiceReportViewModel(audioManager);
	}

}