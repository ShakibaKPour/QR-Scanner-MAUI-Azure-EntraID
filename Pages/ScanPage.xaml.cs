using RepRepair.ViewModels;

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
        cameraView.BarCodeOptions = new Camera.MAUI.ZXingHelper.BarcodeDecodeOptions
        {
            AutoRotate = true,
            PossibleFormats = { ZXing.BarcodeFormat.QR_CODE },
            ReadMultipleCodes = false,
            TryHarder = true,
            TryInverted = true
        };
        cameraView.BarCodeDetectionEnabled = true;
    }

    private void OnBarcodeDetected(object sender, Camera.MAUI.ZXingHelper.BarcodeEventArgs args)
    {
            string qr = args.Result[0].ToString();
            _viewModel.LoadInfo(qr);
               MainThread.BeginInvokeOnMainThread(async () =>
            {
                await cameraView.StopCameraAsync();
            });
        
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

    private void ScanAgain(object sender, EventArgs e)
    {
        // clear the objectinfo properties, startup the camera and barcode detected
    }
}