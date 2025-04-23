using API.eCommerce.EC;
using Assignment1.Models;
using Library.eCommerce.DTO;
using Library.eCommerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.eCommerce.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryController : ControllerBase
    {
  

        private readonly ILogger<InventoryController> _logger;

        public InventoryController(ILogger<InventoryController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Item?> Get()
        {
            return new InventoryEC().Get();
        }

        [HttpGet("{id}")]
        public Item? GetById(int id)
        {
            return new InventoryEC().Get().FirstOrDefault(i => i?.Id == id);
        }


        [HttpDelete("{id}")]
        public Item? Delete(int id)
        {
            return new InventoryEC().Delete(id);
        }
    }
}
