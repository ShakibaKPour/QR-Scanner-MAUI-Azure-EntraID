using Camera.MAUI;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Plugin.Maui.Audio;
using RepRepair.Services.DB;
using RepRepair.Services.VoiceRecording;
using RepRepair.ViewModels;
using ZXing.Net.Maui.Controls;
using RepRepair.Pages;
using RepRepair.Extensions;
using RepRepair.Services.Cognitive;
using RepRepair.Services.Language;
using RepRepair.Services.AlertService;
using RepRepair.Services.ScanningService;
using RepRepair.Services.ReportTypesService;
using RepRepair.Services.Configuration;
using RepRepair.Services.Auth;
using Microsoft.Maui.LifecycleEvents;
using Microsoft.Identity.Client;

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
                .ConfigureLifecycleEvents(events =>
                {
#if ANDROID
                    events.AddAndroid(platform =>
                    {
                        platform.OnActivityResult((activity, rc, result, data) =>
                        {
                            AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(rc, result, data);
                        });
                    });
#endif
                })
        .ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
        });

            builder.Services.AddSingleton<IDatabaseService, DatabaseService>();
            builder.Services.AddSingleton<IVoiceRecordingService,VoiceRedordingService>();
            builder.Services.AddSingleton<IAudioManager, AudioManager>();
            builder.Services.AddSingleton<IAzureCognitiveService, AzureCognitiveService>();
            builder.Services.AddSingleton<IAlertService, AlertService>();
            builder.Services.AddSingleton<IScanningService, ScanningService>();
            builder.Services.AddSingleton<ReportServiceType>();
            builder.Services.AddSingleton<TranslatorService>();
            builder.Services.AddSingleton<LanguageSettingsService>();
            builder.Services.AddSingleton<ConfigurationService>();
            builder.Services.AddSingleton<AuthenticationService>();
            builder.Services.AddTransient<HomeViewModel>();
            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<ScanViewModel>();
            builder.Services.AddTransient<ScanPage>();
            builder.Services.AddTransient<ReportViewModel>();
            builder.Services.AddTransient<MainReportPage>();
            builder.Services.AddTransient<VoiceReportViewModel>();
            builder.Services.AddTransient<VoiceReportPage>();
            builder.Services.AddTransient<WriteReportPage>();
            builder.Services.AddTransient<WriteReportViewModel>();
            builder.Services.AddTransient<DefectListPage>();
            builder.Services.AddTransient<DefectListViewModel>();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            var app= builder.Build();
            ServiceHelper.Initialize(app.Services);
            return app;
        }
    }
}
