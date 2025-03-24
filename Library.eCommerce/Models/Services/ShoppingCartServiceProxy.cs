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
        private ProductServiceProxy _prodSvc = ProductServiceProxy.Current;
        private List<Item?> items;
        public List<Item?> cartItems
        {
            get
            {
                return items;
            }
        }

        public static ShoppingCartServiceProxy Current
        {
            get
            {
                if (instance == null)
                {
                    instance = new ShoppingCartServiceProxy();
                }
                return instance;
            }
        }



        public static ShoppingCartServiceProxy? instance;
        private ShoppingCartServiceProxy()
        {
            items = new List<Item?>();
        }

        public Item? AddOrUpdate(Item item)
        {
            var existingInvItem = _prodSvc.GetById(item.Id);
            if (existingInvItem == null || existingInvItem.Quantity == 0)
            {
                return null;
            }
            
            if (existingInvItem != null)
            {
                existingInvItem.Quantity--;
            }

            var existingItem = cartItems.FirstOrDefault(i => i.Id == item.Id);
            if (existingItem == null)
            {
                var newItem = new Item(item);
                newItem.Quantity = 1;
                cartItems.Add(newItem);
            }
            else
            {
                existingItem.Quantity++;
            }

            return existingInvItem;
        }

        public Item? ReturnItem(Item? item)
        {
            if (item?.Id <= 0 || item == null)
            {
                return null;
            }

            var itemToReturn = cartItems.FirstOrDefault(c => c.Id == item.Id);
            if (itemToReturn != null)
            {
                itemToReturn.Quantity--;
                var inventoryItem = _prodSvc.Products.FirstOrDefault(p => p.Id == item.Id);
                if (inventoryItem == null)
                {
                    _prodSvc.AddOrUpdate(new Item(itemToReturn));
                }
                else
                {
                    inventoryItem.Quantity++;
                }
            }


            return itemToReturn;
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