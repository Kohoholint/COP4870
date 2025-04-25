
using Library.eCommerce.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Api.eCommerce.Database
{
    public class CartFilebase
    {
        private string _root;
        private string _cartRoot;
        private string _productRoot;
        private static CartFilebase _instance;

        public static CartFilebase Current
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new CartFilebase();
                }

                return _instance;
            }
        }


        private CartFilebase()
        {
            _root = @"C:\\temp";
            _cartRoot = $"{_root}\\ShoppingCart";
            _productRoot = $"{_root}\\Products";
        }

     
        public Item AddOrUpdate(Item item)
        {

            //go to the right place
            string cartPath = $"{_cartRoot}\\{item.Id}.json";
            string path = $"{_productRoot}\\{item.Id}.json";
            Item cartItem;


            if (!File.Exists(path))
            {
                return null;
            }
            var invItem = JsonConvert.DeserializeObject<Item>(File.ReadAllText(path));
            if (invItem.Quantity == 0)
            {
                return null;
            }

            //if the item has been previously persisted
            if (File.Exists(cartPath))
            {
                cartItem = JsonConvert.DeserializeObject<Item>(File.ReadAllText(cartPath));
                cartItem.Quantity++;
            }
            else
            {
                cartItem = new Item(item);
                cartItem.Quantity = 1;
            }
            
            invItem.Quantity--;

            //blow them up
            File.Delete(cartPath);
            File.Delete(path);


            //write the file
            File.WriteAllText(cartPath, JsonConvert.SerializeObject(cartItem));
            File.WriteAllText(path, JsonConvert.SerializeObject(invItem));

            //return the item, which now has an id
            return cartItem;
        }

        

        public List<Item?> Cart
        {
            get
            {
                var root = new DirectoryInfo(_cartRoot);
                var _patients = new List<Item>();
                foreach(var patientFile in root.GetFiles())
                {
                    var patient = JsonConvert
                        .DeserializeObject<Item>
                        (File.ReadAllText(patientFile.FullName));
                    if(patient != null)
                    {
                        _patients.Add(patient);
                    }

                }
                return _patients;
            }
        }

        public bool ReturnItem(string id)
        {
            //TODO: refer to AddOrUpdate for an idea of how you can implement this.
            //go to the right place
            string cartPath = $"{_cartRoot}\\{id}.json";
            string path = $"{_productRoot}\\{id}.json";
            Item? itemToReturn;
            Item? cartItem;
            Item? invItem = null;

            if (File.Exists(cartPath))
            {
                cartItem = JsonConvert.DeserializeObject<Item>(File.ReadAllText(cartPath));
                cartItem.Quantity--;
                invItem = JsonConvert.DeserializeObject<Item>(File.ReadAllText(path));
                invItem.Quantity++;

                //blow them up
                File.Delete(path);
                File.Delete(cartPath);

                // If there's no more of that item in the cart, just delete it
                if (cartItem.Quantity > 0)
                {
                    File.WriteAllText(cartPath, JsonConvert.SerializeObject(cartItem));
                }
                
                File.WriteAllText(path, JsonConvert.SerializeObject(invItem));
                return true;
            }   
            return false;
        }

        public static IEnumerable<Item?> Search(string? query)
        {
            return Current.Cart.Where(p => p?.Product?.Name?.ToLower()
                .Contains(query?.ToLower() ?? string.Empty) ?? false);
        }

        public decimal Checkout(decimal tax)
        {
            string cartPath; 
            decimal Total = 0;
            //Calculate the total price of all products in the cart
            foreach (var item in Cart)
            {
                if (item?.Product != null && item.Quantity > 0)
                {
                    Total += item.Product.Price * (item.Quantity ?? 0);
                }
                if (tax > 1.0m)
                {
                    cartPath = $"{_cartRoot}\\{item.Id}.json";
                    File.Delete(cartPath);  //Purchasing an item clears it out of your shopping cart
                }                            //Next time you boot it up, shopping cart should be empty after checkout
 
            }

            Total *= tax; 
            //Print out the total price
            return Total;
        }

    }
  
}