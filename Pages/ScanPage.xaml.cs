using RepRepair.Services.DB;
using RepRepair.Services.Navigation;
using RepRepair.ViewModels;
using ZXing.Net.Maui;

namespace RepRepair.Pages;

public partial class ScanPage : ContentPage
{
	public ScanPage()
	{
		InitializeComponent();
		BindingContext = new ScanViewModel();
    }

    //private void cameraView_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    //{
    //    //Dispatcher.Dispatch(() =>
    //    //{
            
    //    //});
    //}
}