using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using Assignment1.Models;
using Library.eCommerce.Models;

namespace Library.eCommerce.Services
{
    public class ShoppingCartServiceProxy
    {
        private ProductServiceProxy _prodSvc;
        private List<Item?> items;
        public List<Item?> cartItems
        {
            get
            {
                return items;
            }
        }

        public static ShoppingCartServiceProxy? instance;
        private ShoppingCartServiceProxy()
        {
            items = new List<Item?>();
        }
        /*
        public decimal calTotal()
        {
            decimal Total = 0;
            //Calculate the total price of all products in the cart
            foreach (Product? product in Products)
            {
                //TODO: FIX THIS
                //Total += (product?.Price ?? 0) * product.Quantity;
            }

            //Print out the total price
            return Total;
        } */
    }
}