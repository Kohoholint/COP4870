using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using Assignment1.Models;
using Library.eCommerce.Models;

namespace Library.eCommerce.Services
{
    public class ProductServiceProxy
    {
        private ProductServiceProxy()
        {
            Products = new List<Item?>
            {
                new Item { Product = new Product{Id = 1, Name = "Bbhone", Price = 1000.00m}, Id = 1, Quantity = 100 },
                new Item { Product = new Product{Id = 2, Name = "BacBook", Price = 3000.00m}, Id = 2, Quantity = 100 },
                new Item { Product = new Product{Id = 3, Name = "Bapple Batch", Price = 300.00m}, Id = 3, Quantity = 100 }
            };
        }
        //TODO: Get rid of Quantity value from Products.cs

        private int LastKey
        {
            get
            {
                if(!Products.Any())
                {
                    return 0;
                }
                return Products.Select(p => p?.Id ?? 0).Max();
            }
        }

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

        public Item AddOrUpdate(Item item)
        {
           if (item.Id == 0)
           {
                item.Id = LastKey + 1;
                item.Product.Id = item.Id;
                Products.Add(item);
           }
           

            return item;
        }

        public Item? Delete(int id)
        {
            if (id == 0)
            {
                return null;
            }

            Item? product = Products.FirstOrDefault(p => p.Id == id);
            Products.Remove(product);

            return product;
        }

        public Item? GetById(int id)
        {
            return Products.FirstOrDefault(p => p.Id == id);
        }

    }
}