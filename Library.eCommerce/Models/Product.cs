using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public decimal Price { get; set; }

        //public int Quantity { get; set; }

        public string? Display
        {
            get
            {
                return $"{Id}. {Name}\n (${Price}) \n";
                //return $"{Id}. {Name} ${Price} \n Quantity: {Quantity}";
            }
        }

        public Product()
        {
            Name = string.Empty;
        }

        public Product(Product p)
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