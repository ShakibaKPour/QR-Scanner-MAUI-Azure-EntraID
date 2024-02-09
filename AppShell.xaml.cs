using RepRepair.Pages;
using RepRepair.ViewModels;

namespace RepRepair
{
    public partial class AppShell : Shell
    {
        //public bool IsReportVisible
        //{
        //    get => (bool)GetValue(IsReportVisibleProperty);
        //    set => SetValue(IsReportVisibleProperty, value);
        //}

        //public static readonly BindableProperty IsReportVisibleProperty= BindableProperty.Create(nameof(IsReportVisible), typeof(bool), typeof(AppShell), false);
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(MainReportPage), typeof(MainReportPage));
            Routing.RegisterRoute(nameof(VoiceReportPage), typeof(VoiceReportPage));
            Routing.RegisterRoute(nameof(ThankYouPage), typeof(ThankYouPage));
            Routing.RegisterRoute(nameof(WriteReportPage), typeof(WriteReportPage));
            Routing.RegisterRoute(nameof(DefectListPage), typeof(DefectListPage));
            BindingContext = this;
        }

        //    MessagingCenter.Subscribe<ScanViewModel, bool>(this, "UpdateReportTabVisibility", (sender, args) =>
        //    {
        //        IsReportVisible = args;
        //    });
        //}

        //~AppShell()
        //{
        //    MessagingCenter.Unsubscribe<ScanViewModel, bool>(this, "UpdateReportTabVisibility");
        //}
    }
}
