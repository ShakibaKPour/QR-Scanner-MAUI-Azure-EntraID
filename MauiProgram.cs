using Camera.MAUI;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Plugin.Maui.Audio;
using RepRepair.Services.DB;
using RepRepair.Services.Navigation;
using RepRepair.Services.VoiceRecording;
using RepRepair.ViewModels;
using ZXing.Net.Maui.Controls;
using RepRepair.Pages;
using RepRepair.Extensions;
using RepRepair.Services.Cognitive;
using RepRepair.Services.Language;

namespace RepRepair
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCameraView()
                .UseMauiCommunityToolkit()
                .UseBarcodeReader()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<INavigationService, NavigationService>();
            builder.Services.AddSingleton<IDatabaseService, DatabaseService>();
            builder.Services.AddSingleton<IVoiceRecordingService,VoiceRedordingService>();
            builder.Services.AddSingleton<IAudioManager, AudioManager>();
            builder.Services.AddSingleton<IAzureCognitiveService, AzureCognitiveService>();
            builder.Services.AddSingleton<TranslatorService>();
            builder.Services.AddSingleton<LanguageSettingsService>();
            builder.Services.AddSingleton<HomeViewModel>();
            builder.Services.AddSingleton<HomePage>();
            builder.Services.AddSingleton<ScanViewModel>();
            builder.Services.AddSingleton<ScanPage>();
            builder.Services.AddSingleton<ReportViewModel>();
            builder.Services.AddSingleton<MainReportPage>();
            builder.Services.AddSingleton<VoiceReportViewModel>();
            builder.Services.AddSingleton<VoiceReportPage>();
            

#if DEBUG
            builder.Logging.AddDebug();
#endif

            var app= builder.Build();
            ServiceHelper.Initialize(app.Services);
            return app;
        }
    }
}
