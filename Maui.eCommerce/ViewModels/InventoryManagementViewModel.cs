using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assignment1.Models;
using Library.eCommerce.Services;

namespace Maui.eCommerce.ViewModels
{
    public class InventoryManagementViewModel
    {
        public Product? SelectedProduct { get; set; }
        private InventoryServiceProxy _svc = InventoryServiceProxy.Current;
        public List<Product?> Products
        {
            get
            {
                return _svc.Products;
            }
        }

        public Product? Delete()
        {
            return _svc.Delete(SelectedProduct?.Id ?? 0);
        }
    }
}
