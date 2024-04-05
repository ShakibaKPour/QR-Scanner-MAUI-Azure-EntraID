using Camera.MAUI;
using RepRepair.Extensions;
using RepRepair.Services.AlertService;
using RepRepair.ViewModels;

namespace RepRepair.Pages;

public partial class ScanPage : ContentPage
{
    private ScanViewModel _viewModel;
    private readonly IAlertService _alertService;

    public ScanPage()
    {
        InitializeComponent();

        _viewModel = new ScanViewModel();
        _alertService = ServiceHelper.GetService<IAlertService>();

        BindingContext = _viewModel;

        //if (cameraView != null)
        //{
            cameraView.CamerasLoaded += OnCameraLoaded;
            ConfigureCameraForScanning();
            cameraView.BarcodeDetected += OnBarcodeDetected;
        //}
        //else
        //{
        //    AlertCameraNotSupported();
        //}
    }

    private async void AlertCameraNotSupported()
    {
        await _alertService.ShowAlertAsync("Error", "Camera view is not available. Press on start the camera", "OK");
    }

    private void OnCameraLoaded(object sender, EventArgs e)
    {
        try
        {
            if (cameraView?.NumCamerasDetected > 0)
            {
                cameraView.Camera = cameraView.Cameras.FirstOrDefault();
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await cameraView.StartCameraAsync();
                });
            }
            else
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    _alertService.ShowAlertAsync("Error", "No cameras detected on this device.", "OK");
                });
            }
        }
        catch (Exception ex)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                _alertService.ShowAlertAsync("Error", $"Failed to load cameras: {ex.Message}", "OK");
            });
        }
    }

    private void ConfigureCameraForScanning()
    {
        if (cameraView != null)
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
    }

    private async void OnBarcodeDetected(object sender, Camera.MAUI.ZXingHelper.BarcodeEventArgs args)
    {
        if (cameraView == null) return;

        ConfigureCameraForScanning();
        string? qrCode = args.Result.FirstOrDefault()?.ToString();

        if (!string.IsNullOrEmpty(qrCode))
        {
            await _viewModel.ScanAsync(qrCode);
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await cameraView.StopCameraAsync();
            });
        }
    }

    private void ScanAgain(object sender, EventArgs e)
    {
        _viewModel.ResetScan();
        RestartCamera();
    }

    public void RestartCamera()
    {
        if (cameraView == null) return;

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await cameraView.StopCameraAsync();
            ConfigureCameraForScanning();
            await cameraView.StartCameraAsync();
        });
    }

    private async void StopCamera(object sender, EventArgs e)
    {
        if (cameraView == null) return;

        try
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await cameraView.StopCameraAsync();
            });
        }
        catch (Exception ex)
        {
            await _alertService.ShowAlertAsync("Error", $"Error stopping the camera: {ex.Message}", "OK");
        }
    }

    private void StartCamera(object sender, EventArgs e)
    {
        if (cameraView == null) return;

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            ConfigureCameraForScanning();
            await cameraView.StartCameraAsync();
        });
    }
}