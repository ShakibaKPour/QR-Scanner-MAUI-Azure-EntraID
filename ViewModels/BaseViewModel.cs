using System.ComponentModel;

namespace RepRepair.ViewModels;

public abstract class BaseViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    //initializes the ViewModel with data or performs any setup when the ViewModel is navigated to.
    //particularly useful when navigating to a new page and we need to pass data to the ViewModel.
    public virtual Task InitializeAsync(object parameter)
    {
        return Task.CompletedTask;
    }

    //Consider to put the Navigationservice in the BaseViewModel class
    //so that it stores the navigationService instance in a NavigationService property, of type INavigationService.
    //Therefore, all view-model classes, which derive from the BaseViewModel class,
    //can use the NavigationService property to access the methods specified by the INavigationService interface.
}
