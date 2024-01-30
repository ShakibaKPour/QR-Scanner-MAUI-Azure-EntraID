using RepRepair.ViewModels;

namespace RepRepair.Pages;

public partial class DefectListPage : ContentPage
{
	public DefectListPage()
	{
		InitializeComponent();
		BindingContext= new DefectListViewModel();
	}
}