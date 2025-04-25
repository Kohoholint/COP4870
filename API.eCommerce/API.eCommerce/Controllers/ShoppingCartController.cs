using Api.eCommerce.Database;
using API.eCommerce.EC;
using Assignment1.Models;
using Library.eCommerce.DTO;
using Library.eCommerce.Models;
using Library.eCommerce.Util;
using Microsoft.AspNetCore.Mvc;

namespace API.eCommerce.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShoppingCartController : ControllerBase
    {


        private readonly ILogger<ShoppingCartController> _logger;

        public ShoppingCartController(ILogger<ShoppingCartController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Item?> Get()
        {
            return new ShoppingCartEC().Get();
        }

        [HttpGet("{id}")]
        public Item? GetById(int id)
        {
            return new ShoppingCartEC().Get().FirstOrDefault(i => i?.Id == id);
        }


        [HttpDelete("{id}")]
        public Item? ReturnItem(int id)
        {
            var itemToDelete = CartFilebase.Current.Cart.FirstOrDefault(i => i?.Id == id);
            if (itemToDelete != null)
            {
                CartFilebase.Current.ReturnItem(itemToDelete.Id.ToString());
            }
            return itemToDelete;
        }

        [HttpPost]
        public Item? AddOrUpdate([FromBody] Item item)
        {
            var newItem = new ShoppingCartEC().AddOrUpdate(item);
            return item;
        }

        [HttpPost("Search")]
        public IEnumerable<Item> Search([FromBody] QueryRequest query)
        {
            return new ShoppingCartEC().Get(query.Query);
        }

        [HttpPost("Checkout")]
        public decimal Checkout([FromBody] decimal tax)
        {
            return new ShoppingCartEC().Checkout(tax);
        }
    }
}
