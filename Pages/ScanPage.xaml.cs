using RepRepair.Services.DB;
using RepRepair.Services.Navigation;
using RepRepair.ViewModels;
using Camera.MAUI;
using ZXing.Net.Maui;

namespace RepRepair.Pages;

public partial class ScanPage : ContentPage
{
    private ScanViewModel _viewModel;
	public ScanPage()
	{
		InitializeComponent();
		_viewModel = new ScanViewModel();
        BindingContext = _viewModel;
        cameraView.CamerasLoaded += OnCameraLoaded;
        cameraView.BarcodeDetected += OnBarcodeDetected;

    }

    private void OnBarcodeDetected(object sender, Camera.MAUI.ZXingHelper.BarcodeEventArgs args)
    {
            string qr = args.Result[0].ToString();
            _viewModel.LoadInfo(qr);
            cameraView.StopCameraAsync();
        
    }

    private void OnCameraLoaded(object sender, EventArgs e)
    {
            if (cameraView.NumCamerasDetected > 0)
            {
                cameraView.Camera = cameraView.Cameras.First();
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await cameraView.StartCameraAsync();
                });
            }
    }
}