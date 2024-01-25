using RepRepair.ViewModels;
using Microsoft.Maui.Controls;
using System;
using RepRepair.Services.Navigation;
using RepRepair.Extensions;

namespace RepRepair.Pages;

public partial class HomePage : ContentPage
{
    //TODO Implement the language selection 
    public HomePage()
    {
        InitializeComponent();
        BindingContext = new HomeViewModel();
    }

}