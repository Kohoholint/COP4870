using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assignment1.Models;
using Library.eCommerce.Services;

namespace Maui.eCommerce.ViewModels
{
    public class ProductViewModel
    {
        public string? Name { 
            get
            {
                return Model?.Name;
            }
            set
            {
                if (Model != null && Model.Name != value)
                {
                    Model.Name = value;
                }
            }
        }
        public decimal Price
        {
            get
            {
                return Model?.Price ?? 0;
            }
            set
            {
                if (Model != null)
                {
                    Model.Price = value;
                }
            }
        }
        public int Quantity {
            get
            {
                return Model?.Quantity ?? 0;
            }
            set
            {
                if (Model != null)
                {
                    Model.Quantity = value;
                }
            }
        }

        public Product? Model { get; set; }

        public void AddOrUpdate()
        {
            InventoryServiceProxy.Current.AddOrUpdate(Model);
        }

        public ProductViewModel ()
        {
            Model = new Product();
        }

        public ProductViewModel(Product? model)
        {
            Model = model;
        }
    }
}
