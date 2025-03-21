using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using Assignment1.Models;

namespace Library.eCommerce.Services
{
    public class InventoryServiceProxy
    {
        private InventoryServiceProxy()
        {
            Products = new List<Product?>
            {
                new Product { Id = 1, Name = "Bbhone", Price = 1000.00m, Quantity = 100 },
                new Product { Id = 2, Name = "BacBook", Price = 300.00m, Quantity = 100 },
                new Product { Id = 3, Name = "Bapple Batch", Price = 300.00m, Quantity = 100 }
            };
        }

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

        private static InventoryServiceProxy? instance;
        private static object instanceLock = new object();

        public static InventoryServiceProxy Current
        {
            get
            {
                lock(instanceLock)
                {
                    if(instance == null)
                    {
                        instance = new InventoryServiceProxy();
                    }
                }
                return instance;
            }
        }

        public List<Product?> Products {get; private set;} //Maybe make this a dictionary? The key would be the product ID and the quantity of said product would be the values.


        public Product AddOrUpdate(Product product)
        {
           if (product.Id == 0)
           {
                product.Id = LastKey + 1;
                Products.Add(product);
           }
           

            return product;
        }

        public Product? Delete(int id)
        {
            if (id == 0)
            {
                return null;
            }

            Product? product = Products.FirstOrDefault(p => p.Id == id);
            Products.Remove(product);

            return product;
        }

    }
}