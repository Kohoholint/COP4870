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
        public decimal SubTotal
        {
            get
            {
                return cart.calTotal(1m); //Just gets subtotal
            }
        }
        public decimal Tax
        {
            get
            {
                return cart.calTotal(0.07m); //Just gets tax
            }
        }
        public decimal Total
        {
            get
            {
                return cart.calTotal(1.07m);    //Gets total with tax
            }
        }


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
