using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using Assignment1.Models;
using Library.eCommerce.DTO;
using Library.eCommerce.Models;
using Library.eCommerce.Util;
using Library.eCommerce.Utilities;
using Newtonsoft.Json;

namespace Library.eCommerce.Services
{
    public class ProductServiceProxy
    {
        private ProductServiceProxy()
        {
            var productPayload = new WebRequestHandler().Get("/Inventory").Result;
            Products = JsonConvert.DeserializeObject<List<Item>>(productPayload) ?? new List<Item?>();
            //Products = new List<Item?>
            //{
            //    new Item { Product = new ProductDTO{Id = 1, Name = "Bphone", Price = 1000.00m}, Id = 1, Quantity = 1 },
            //    new Item { Product = new ProductDTO{Id = 2, Name = "BacBook", Price = 3000.00m}, Id = 2, Quantity = 2 },
            //    new Item { Product = new ProductDTO{Id = 3, Name = "Bapple Batch", Price = 300.00m}, Id = 3, Quantity = 3 }
            //};
        }
        //TODO: Get rid of Quantity value from Products.cs

        private static ProductServiceProxy? instance;
        private static object instanceLock = new object();

        public static ProductServiceProxy Current
        {
            get
            {
                lock(instanceLock)
                {
                    if(instance == null)
                    {
                        instance = new ProductServiceProxy();
                    }
                }
                return instance;
            }
        }

        public List<Item?> Products {get; private set;} 

        public async Task<IEnumerable<Item?>> Search(string? query)
        {
            if (query == null)
            {
                return new List<Item>();
            }
            var response = await new WebRequestHandler().Post("/Inventory/Search", new QueryRequest { Query = query });
            Products = JsonConvert.DeserializeObject<List<Item?>>(response) ?? new List<Item?>();
            return Products;
        }
        

        public Item AddOrUpdate(Item item)
        {
            var response = new WebRequestHandler().Post("/Inventory", item).Result;
            var newItem = JsonConvert.DeserializeObject<Item>(response);
            if (newItem == null)
            {
                return item;
            }
            if (item.Id == 0)
            {
                Products.Add(newItem);
            }
            else
            {
                var existingItem = Products.FirstOrDefault(p => p.Id == item.Id);
                var index = Products.IndexOf(existingItem);
                Products.RemoveAt(index);
                Products.Insert(index, new Item(newItem));
            }

            return item;
        }

        public Item? Delete(int id)
        {
            if (id == 0)
            {
                return null;
            }

            var result = new WebRequestHandler().Delete($"/Inventory/{id}").Result;

            Item? product = Products.FirstOrDefault(p => p.Id == id);
            Products.Remove(product);

            return JsonConvert.DeserializeObject<Item>(result);
        }

        public Item? GetById(int id)
        {
            return Products.FirstOrDefault(p => p.Id == id);
        }

    }
}