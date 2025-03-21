using Library.eCommerce.Services;
using Maui.eCommerce.ViewModels;

namespace Maui.eCommerce.Views;

public partial class ProductDetails : ContentPage
{
	public ProductDetails()
	{
		InitializeComponent();
		BindingContext = new ProductViewModel();
    }

    private void GoBackClicked(object sender, EventArgs e)
    {
		Shell.Current.GoToAsync("//InventoryManagement");
    }

    private void OkClicked(object sender, EventArgs e)
    {
        var name = (BindingContext as ProductViewModel)?.Name;
        decimal price = ((ProductViewModel)BindingContext).Price;
        int quantity = (int)((ProductViewModel)BindingContext).Quantity;
        InventoryServiceProxy.Current.AddOrUpdate(new Assignment1.Models.Product { Name = name, Price = price, Quantity = quantity });
        Shell.Current.GoToAsync("//InventoryManagement");
    }
}