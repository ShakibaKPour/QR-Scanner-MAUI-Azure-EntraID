using RepRepair.ViewModels;
using Microsoft.Maui.Controls;
using System;
using RepRepair.Services.Navigation;

namespace RepRepair;

public partial class HomePage : ContentPage
{
    //TODO Implement the language selection 
    public HomePage(INavigationService navigationService)
	{
        InitializeComponent();
        BindingContext = new HomeViewModel(navigationService);
	}
}