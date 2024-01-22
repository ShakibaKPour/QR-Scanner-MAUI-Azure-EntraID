using RepRepair.ViewModels;

namespace RepRepair.Services.Navigation;

public interface INavigationService
{
    Task NavigateToAsync<TViewModel>(object parameter = null)
        where TViewModel : BaseViewModel;
    //Task NavigateToAsync<TViewModel>();
}
