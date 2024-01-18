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
}
