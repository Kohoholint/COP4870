using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using Assignment1.Models;

namespace Library.eCommerce.Services
{
    public class CartServiceProxy
    {
        private CartServiceProxy()
        {
            Products = new List<Product?>();    //List of products in the cart
        }


        private static CartServiceProxy? instance;
        private static object instanceLock = new object();

        public static CartServiceProxy Current
        {
            get
            {
                lock(instanceLock)
                {
                    if(instance == null)
                    {
                        instance = new CartServiceProxy();
                    }
                }
                return instance;
            }
        }

        public List<Product?> Products {get; private set;}


        public Product AddOrUpdate(Product product)
        {
            Products.Add(product);


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

       public decimal calTotal()
        {
            decimal Total = 0;
            //Calculate the total price of all products in the cart
            foreach (Product? product in Products)
            {

                Total += (product?.Price ?? 0) * product.Quantity;
            }

            //Print out the total price
            return Total;
        }
    }
}