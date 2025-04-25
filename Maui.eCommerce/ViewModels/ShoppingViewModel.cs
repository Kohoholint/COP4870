using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Assignment1.Models;
using Library.eCommerce.Models;
using Library.eCommerce.Services;


namespace Maui.eCommerce.ViewModels
{
    class ShoppingViewModel : INotifyPropertyChanged
    {
        private ProductServiceProxy _invSvc = ProductServiceProxy.Current;
        private ShoppingCartServiceProxy _cartSvc = ShoppingCartServiceProxy.Current;
        public string? Query { get; set; }
        public Item? SelectedItem { get; set; }
        public Item? SelectedCartItem { get; set; }

        public ObservableCollection<Item?> Inventory
        {
            get
            {
                var filteredList = _invSvc.Products
                    .Where(p => p?.Product?.Name?.ToLower()
                    .Contains(Query?.ToLower() ?? string.Empty) ?? false);

                return new ObservableCollection<Item?>(filteredList.Where(i => i?.Quantity > 0));
            }
        }

        public ObservableCollection<Item?> ShoppingCart
        {
            get
            {
                var filteredList = _cartSvc.cartItems
                    .Where(p => p?.Product?.Name?.ToLower()
                    .Contains(Query?.ToLower() ?? string.Empty) ?? false);

                return new ObservableCollection<Item?>(filteredList.Where(i => i?.Quantity > 0));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (propertyName is null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        public void RefreshUX()
        {
            NotifyPropertyChanged(nameof(Inventory));
            NotifyPropertyChanged(nameof(ShoppingCart));
        }

        public async Task<bool> Search()
        {
            await _invSvc.Search(Query);
            await _cartSvc.Search(Query);
            NotifyPropertyChanged(nameof(Inventory));
            NotifyPropertyChanged(nameof(ShoppingCart));
            return true;
        }

        public void PurchaseItem()
        {
            if (SelectedItem != null)
            {
                var shouldRefresh = SelectedItem.Quantity >= 1;
                var updatedItem = _cartSvc.AddOrUpdate(SelectedItem);

                if (updatedItem != null && shouldRefresh)
                {
                    NotifyPropertyChanged(nameof(Inventory));
                    NotifyPropertyChanged(nameof(ShoppingCart));

                }
            }

        }
        
        public void ReturnItem()
        {
            if (SelectedCartItem != null)
            {
                var shouldRefresh = SelectedCartItem.Quantity >= 1;
                var updatedItem = _cartSvc.ReturnItem(SelectedCartItem);

                if (updatedItem != null && shouldRefresh)
                {
                    NotifyPropertyChanged(nameof(Inventory));
                    NotifyPropertyChanged(nameof(ShoppingCart));

                }
            }
        } 
    }
}
