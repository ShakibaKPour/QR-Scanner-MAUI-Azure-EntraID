using RepRepair.ViewModels;

namespace RepRepair.Pages;

public partial class SignInPage : ContentPage
{
	public SignInPage()
	{
		InitializeComponent();
		BindingContext= new SignInViewModel();
	}
}