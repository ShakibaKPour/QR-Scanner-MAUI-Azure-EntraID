using RepRepair.Pages;
using RepRepair.ViewModels;

namespace RepRepair.Services.Navigation;

public class NavigationService : INavigationService
{
    private readonly Dictionary<Type, Type> _mappings;

    public NavigationService()
    {
        _mappings = new Dictionary<Type, Type>();
        CreatePageViewModelMappings();
    }

    private void CreatePageViewModelMappings()
    {
        _mappings.Add(typeof(ScanViewModel), typeof(ScanPage));
        _mappings.Add(typeof(ReportViewModel), typeof(MainReportPage));
        _mappings.Add(typeof(HomeViewModel), typeof(HomePage));
        _mappings.Add(typeof(VoiceReportViewModel), typeof(VoiceReportPage));
    }

    public async Task NavigateToAsync<TViewModel>(object parameter = null) where TViewModel : BaseViewModel
    {
        var pageType = _mappings[typeof(TViewModel)];
        var page = Activator.CreateInstance(pageType) as Page;

        if (page != null)
        {
            if (page.BindingContext is BaseViewModel viewModel)
            {
                await viewModel.InitializeAsync(parameter);
            }
            await Application.Current.MainPage.Navigation.PushAsync(page);
        }
    }

    public async Task NavigateToAsync<TViewModel>()
    {
        await Application.Current.MainPage.Navigation.PopAsync();
    }
}
