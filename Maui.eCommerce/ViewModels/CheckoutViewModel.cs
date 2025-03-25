using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.eCommerce.Models;
using Library.eCommerce.Services;

namespace Maui.eCommerce.ViewModels
{
    internal class CheckoutViewModel
    {
        private ShoppingCartServiceProxy cart = ShoppingCartServiceProxy.Current;
        public decimal Total
        {
            get
            {
                return cart.calTotal();
            }
        }
        //private decimal WithTax = 0.0m;

        public ObservableCollection<Item?> ShoppingCart
        {
            get
            {
                return new ObservableCollection<Item?>(cart.cartItems
                    .Where(i => i?.Quantity > 0));
            }
        }
    }
}
