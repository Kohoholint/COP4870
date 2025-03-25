namespace Maui.eCommerce.Views;

using System.Net.WebSockets;
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
        (BindingContext as ShoppingViewModel).PurchaseItem();
    }

    private void RemoveFromCartClicked(object sender, EventArgs e)
    {
        (BindingContext as ShoppingViewModel).ReturnItem();
    }

    private void CheckoutClicked(object sender, EventArgs e)
    {
        var cart = (BindingContext as ShoppingViewModel)?.ShoppingCart;
        Shell.Current.GoToAsync("//Checkout");

    }

}