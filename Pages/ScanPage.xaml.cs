using RepRepair.Extensions;
using RepRepair.Services.AlertService;
using RepRepair.ViewModels;

namespace RepRepair.Pages;

public partial class ScanPage : ContentPage
{
    // TODO : handle all possible null exceptions, including cameraview  
    public ScanViewModel _viewModel;
    private readonly IAlertService _alertService;
	public ScanPage()
	{
		InitializeComponent();
		_viewModel = new ScanViewModel();
        _alertService = ServiceHelper.GetService<IAlertService>();
        BindingContext = _viewModel;
        cameraView.CamerasLoaded += OnCameraLoaded;
        ConfigureCameraForScanning();
        cameraView.BarcodeDetected += OnBarcodeDetected;
    }

    private void OnCameraLoaded(object sender, EventArgs e)
    {
        if (cameraView.NumCamerasDetected > 0)
        {
            cameraView.Camera = cameraView.Cameras.FirstOrDefault();
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await cameraView.StartCameraAsync();
            });
        }
        else
        {
            throw new Exception("Camera not found");
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
        ConfigureCameraForScanning();
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
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
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
            ConfigureCameraForScanning();
            await cameraView.StartCameraAsync();
        });
    }
}