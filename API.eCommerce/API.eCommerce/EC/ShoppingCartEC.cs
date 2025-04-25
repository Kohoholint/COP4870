using Api.eCommerce.Database;
using API.eCommerce.Database;
using Library.eCommerce.Models;

namespace API.eCommerce.EC
{
    public class ShoppingCartEC
    {
        public List<Item?> Get()
        {
            return CartFilebase.Current.Cart;
        }

        //TODO: FIX MEEEE
        public IEnumerable<Item?> Get(string? query)
        {
            //TODO: FIX MEEEE
            return CartFilebase.Search(query).Take(100) ?? new List<Item>();
        }

        public Item? ReturnItem(int id)
        {
            var itemToDelete = CartFilebase.Current.Cart.FirstOrDefault(i => i?.Id == id);
            if (itemToDelete != null && itemToDelete.Id > 0)
            {
                CartFilebase.Current.ReturnItem(itemToDelete.Id.ToString());
            }
            return itemToDelete;
        }

        public Item? AddOrUpdate(Item item)
        {
            //    if (item.Id == 0)
            //    {
            //        item.Id = Filebase.Current.LastKey + 1;
            //        item.Product.Id = item.Id;
            //        Filebase.Current.Inventory.Add(item);
            //    }
            //    else
            //    {
            //        var existingItem = Filebase.Current.Inventory.FirstOrDefault(p => p.Id == item.Id);
            //        var index = Filebase.Current.Inventory.IndexOf(existingItem);
            //        Filebase.Current.Inventory.RemoveAt(index);
            //        Filebase.Current.Inventory.Insert(index, new Item(item));
            //    }


            return CartFilebase.Current.AddOrUpdate(item);
        }

        public decimal Checkout(decimal tax)
        {
            return CartFilebase.Current.Checkout(tax);
        }

    }
}
