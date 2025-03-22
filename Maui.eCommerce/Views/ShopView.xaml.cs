namespace Maui.eCommerce.Views;
using Maui.eCommerce.ViewModels;

public partial class ShopView : ContentPage
{
	public ShopView()
	{
		InitializeComponent();
		BindingContext = new ShoppingViewModel();
	}

    private void CancelClicked(object sender, EventArgs e)
    {
		Shell.Current.GoToAsync("//MainPage");
    }

    private void AddToCartClicked(object sender, EventArgs e)
    {

    }
}