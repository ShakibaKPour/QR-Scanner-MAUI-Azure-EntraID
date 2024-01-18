using RepRepair.Services.DB;
using RepRepair.Services.Navigation;
using RepRepair.ViewModels;
using ZXing.Net.Maui;

namespace RepRepair;

public partial class ScanPage : ContentPage
{
	public ScanPage(IDatabaseService databaseService, INavigationService navigationService)
	{
		InitializeComponent();
		BindingContext = new ScanViewModel(databaseService, navigationService);
    }

    private void cameraView_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    {
        //Dispatcher.Dispatch(() =>
        //{
            
        //});
    }
}