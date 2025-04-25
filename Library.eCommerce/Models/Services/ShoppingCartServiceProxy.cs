using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using Assignment1.Models;
using Library.eCommerce.Models;
using Library.eCommerce.Util;
using Library.eCommerce.Utilities;
using Newtonsoft.Json;

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
            var productPayload = new WebRequestHandler().Get("/ShoppingCart").Result;
            items = JsonConvert.DeserializeObject<List<Item>>(productPayload) ?? new List<Item?>();
        }

        public async Task<IEnumerable<Item?>> Search(string? query)
        {
            if (query == null)
            {
                return new List<Item>();
            }
            var response = await new WebRequestHandler().Post("/ShoppingCart/Search", new QueryRequest { Query = query });
            // Fixes a bug that stops full inventory from being displayed in shopping cart view
            var SearchResults = JsonConvert.DeserializeObject<List<Item?>>(response) ?? new List<Item?>();
            return SearchResults;
        }

        public Item? AddOrUpdate(Item item)
        {
            var response = new WebRequestHandler().Post("/ShoppingCart", item).Result;
            var responseItem = JsonConvert.DeserializeObject<Item>(response);

            if (responseItem == null)
            {
                return item;
            }

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
            var result = new WebRequestHandler().Delete($"/ShoppingCart/{item.Id}").Result;

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

            return JsonConvert.DeserializeObject<Item>(result);
        }

        public decimal calTotal(decimal tax)
        {
            var response = new WebRequestHandler().Post("/ShoppingCart/Checkout", tax).Result;
            return JsonConvert.DeserializeObject<decimal>(response);
        }
    }
}