using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assignment1.Models;

namespace Library.eCommerce.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public decimal Price { get; set; }

        //public int Quantity { get; set; }

        public string? Display
        {
            get
            {
                return $"{Name}\n (${Price}) \n";
                //return $"{Id}. {Name} ${Price} \n Quantity: {Quantity}";
            }
        }

        public ProductDTO()
        {
            Name = string.Empty;
        }

        public ProductDTO(Product p)
        {
            Name = p.Name;
            Id = p.Id;
            Price = p.Price;
        }

        public ProductDTO(ProductDTO p)
        {
            Name = p.Name;
            Id = p.Id;
            Price = p.Price;
        }

        public override string ToString()
        {
            return Display ?? string.Empty;
        }
    }
}
