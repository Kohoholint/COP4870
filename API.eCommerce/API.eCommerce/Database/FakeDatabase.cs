using Assignment1.Models;
using Library.eCommerce.DTO;
using Library.eCommerce.Models;

namespace API.eCommerce.Database
{
    public static class FakeDatabase
    {
        private static List<Item?> inventory = new List<Item?>
            {
                new Item { Product = new ProductDTO{Id = 1, Name = "Bphone WEB", Price = 1000.00m}, Id = 1, Quantity = 1 },
                new Item { Product = new ProductDTO { Id = 2, Name = "BacBook WEB", Price = 3000.00m }, Id = 2, Quantity = 2 },
                new Item { Product = new ProductDTO { Id = 3, Name = "Bapple Batch WEB", Price = 300.00m }, Id = 3, Quantity = 3 }
        };

        public static int Lastkey_Item
        {
            get
            {
                if (!inventory.Any())
                {
                    return 0;
                }
                return inventory.Select(p => p?.Id ?? 0).Max();
            }
        }

        public static List<Item?> Inventory
        {
            get
            {
                return inventory;
            }
        }

        public static IEnumerable<Item> Search(string? query)
        {
            return Inventory.Where(p => p?.Product?.Name?.ToLower()
                .Contains(query?.ToLower() ?? string.Empty) ?? false);
        }
        
    }
}
