using RepRepair.Pages;
using RepRepair.ViewModels;

namespace RepRepair
{
    public partial class AppShell : Shell
    {
        public bool IsReportVisible
        {
            get => (bool)GetValue(IsReportVisibleProperty);
            set => SetValue(IsReportVisibleProperty, value);
        }

        public static readonly BindableProperty IsReportVisibleProperty= BindableProperty.Create(nameof(IsReportVisible), typeof(bool), typeof(AppShell), false);
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("VoiceReportPage", typeof(VoiceReportPage));
            Routing.RegisterRoute("Thank You!", typeof(ThankYouPage));
            Routing.RegisterRoute("Write to Us!", typeof(WriteReportPage));
            BindingContext = this;

            MessagingCenter.Subscribe<ScanViewModel, bool>(this, "UpdateReportTabVisibility", (sender, args) =>
            {
                IsReportVisible = args;
            });
        }
    }
}
