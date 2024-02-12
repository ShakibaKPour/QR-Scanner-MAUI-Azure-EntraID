using RepRepair.Services.DB;
using RepRepair.Services.ScanningService;
using RepRepair.Services.VoiceRecording;
using RepRepair.ViewModels;

namespace RepRepair.Pages;

public partial class ScanPage : ContentPage
{
    public ScanViewModel _viewModel;
   // private IDatabaseService _databaseService;
	public ScanPage()
	{
		InitializeComponent();
       // var scanningService = new ScanningService(_databaseService);
		_viewModel = new ScanViewModel();
        BindingContext = _viewModel;
        cameraView.CamerasLoaded += OnCameraLoaded;
        cameraView.BarcodeDetected += OnBarcodeDetected;
        ConfigureCameraForScanning();
    }

    private void OnCameraLoaded(object sender, EventArgs e)
    {
            if (cameraView.NumCamerasDetected > 0)
            {
                cameraView.Camera = cameraView.Cameras.First();
                ConfigureCameraForScanning();
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await cameraView.StartCameraAsync();
                });
            }
    }

    private void ConfigureCameraForScanning()
    {
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

    private async void OnBarcodeDetected(object sender, Camera.MAUI.ZXingHelper.BarcodeEventArgs args)
    {
        string qr = args.Result[0].ToString();
        await _viewModel.ScanAsync(qr);
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await cameraView.StopCameraAsync();
        });

    }

    private void ScanAgain(object sender, EventArgs e)
    {
        _viewModel.ResetScan();
        RestartCamera();
    }

    public void RestartCamera()
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await cameraView.StopCameraAsync();
            ConfigureCameraForScanning();
            await cameraView.StartCameraAsync();
        });
    }

    private async void StopCamera(object sender, EventArgs e)
    {
        try
        {
            // _viewModel.SimulateLoadInfoAsync();
          // await _viewModel.ScanAsync("TestQR");
            await _viewModel.ScanAsync("6F9619FF-8B86-D011-B42D-00C04FC964FF");
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await cameraView.StartCameraAsync();
                await cameraView.StopCameraAsync();
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error with stopping camera: {ex.Message}");
        }

    }

    private void StartCamera(object sender, EventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await cameraView.StopCameraAsync();
            ConfigureCameraForScanning();
            await cameraView.StartCameraAsync();
        });
    }
}