using System;
using System.ComponentModel.Design;
using System.Reflection.Metadata.Ecma335;
using System.Xml.Serialization;
using Assignment1.Models;
using Library.eCommerce.Services;

namespace MyApp 
{
    internal class Program 
    {
        static void Main(string[] args) 
        {
            Console.WriteLine("Welcome to Amazon!");
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("I. Inventory");
            Console.WriteLine("C. Cart");

            List<Product?> list = InventoryServiceProxy.Current.Products;
            List<Product?> cart = CartServiceProxy.Current.Products;

            char choice = 'Z';
            while (true)
            {
                string? input = Console.ReadLine();
                choice = input[0];
                switch(choice) 
                {
                    case 'I':
                    case 'i':
                        InventoryMenu(list, cart);
                        break;

                    case 'C':
                    case 'c':
                        CartMenu(cart, list);
                        break;

                    default:
                        Console.WriteLine("Error: Unknown Command");
                        break;
                    
                }

            }

        }

        static void InventoryMenu(List<Product?> list, List<Product?> cart)
        {
            Console.WriteLine("Inventory");
            Console.WriteLine("C. Create new inventory item");
            Console.WriteLine("R. Read all inventory items");
            Console.WriteLine("u. Update an inventory item");
            Console.WriteLine("D. Delete an inventory item");
            Console.WriteLine("B. Go to cart");

            char choice = 'Z';
            do 
            {
                string? input = Console.ReadLine();
                choice = input[0];
                switch(choice) 
                {
                    case 'C':
                    case 'c':
                        Console.WriteLine("Please enter in the name, price, and quantity of the item you would like to add");
                        InventoryServiceProxy.Current.AddOrUpdate(new Product
                        {
                            Name = Console.ReadLine(),
                            Price = decimal.Parse(Console.ReadLine() ?? "0"),
                            Quantity = int.Parse(Console.ReadLine() ?? "0")
                        });
                        break;

                    case 'R':
                    case 'r':
                        list.ForEach(Console.WriteLine);
                        break;

                    case 'U':
                    case 'u':
                        //Select one of the products to update
                        Console.WriteLine("Which product would you like to update?");
                        int selection = int.Parse(Console.ReadLine() ?? "-1");
                        var selectedProd = list.FirstOrDefault(p => p.Id == selection);

                        if (selectedProd != null)
                        {
                            selectedProd.Name = Console.ReadLine() ?? "ERROR";
                            selectedProd.Price = decimal.Parse(Console.ReadLine() ?? "0");
                            selectedProd.Quantity = int.Parse(Console.ReadLine() ?? "0");
                            InventoryServiceProxy.Current.AddOrUpdate(selectedProd);
                        }
                        break;

                    case 'D':
                    case 'd':
                        //select one of the products
                        //throw it away
                        Console.WriteLine("Which product would you like to delete");
                        selection = int.Parse(Console.ReadLine() ?? "-1");
                        InventoryServiceProxy.Current.Delete(selection);
                        break;

                    default:
                        Console.WriteLine("Error: Unknown Command");
                        break;
                    
                }

            } while (choice != 'B' && choice != 'b');
            CartMenu(cart, list);
        }

        static void CartMenu(List<Product?> cart, List<Product?> list)
        {
            Console.WriteLine("Shopping Cart");
            Console.WriteLine("C. Checkout");
            Console.WriteLine("R. Read all items in your shopping cart");
            Console.WriteLine("A. Add an item to your cart");
            Console.WriteLine("U. Update an item");
            Console.WriteLine("D. Delete an item");
            Console.WriteLine("B. Go to inventory");

            char choice = 'Z';
            do 
            {
                Console.WriteLine("What would you like to do?");
                string? input = Console.ReadLine();
                choice = input[0];

                switch(choice) 
                {
                    case 'C':
                    case 'c':
                      //Checkout
                      //Makes an itemized receipt and prints out total including sales tax
                        Checkout(cart);
                        break;

                    case 'R':
                    case 'r':
                        cart.ForEach(Console.WriteLine);
                        break;

                    case 'A':
                    case 'a':
                        //Add an item to the cart
                        Console.WriteLine("Which item would you like to add?");
                        int selection = int.Parse(Console.ReadLine() ?? "-1");
                        var selectedProd = list.FirstOrDefault(p => p.Id == selection);

                        if (selectedProd != null)
                        {
                            Console.WriteLine("How many would you like to add?");
                            int quantity = int.Parse(Console.ReadLine() ?? "0");
                            if (quantity <= selectedProd.Quantity)
                            {
                                Product addedToCart = new Product
                                {
                                    Id = selectedProd.Id,
                                    Name = selectedProd.Name,
                                    Price = selectedProd.Price,
                                    Quantity = quantity
                                };
                                CartServiceProxy.Current.AddOrUpdate(addedToCart);
                                Console.WriteLine("Item added to cart.");
                            }
                            else{
                                Console.WriteLine("Error: Not enough in stock");
                            }
                        }
                        break;

                    case 'U':
                    case 'u':
                        //Select one of the products
                        Console.WriteLine("Which item would you like to update?");
                        selection = int.Parse(Console.ReadLine() ?? "-1");
                        selectedProd = list.FirstOrDefault(p => p.Id == selection);
                        var prodInCart = cart.FirstOrDefault(p => p.Id == selection);

                        if (selectedProd != null)
                        {
                            int quantity = int.Parse(Console.ReadLine() ?? "0");
                            if (quantity <= selectedProd.Quantity)
                            {
                                prodInCart.Quantity = quantity;
                                CartServiceProxy.Current.AddOrUpdate(prodInCart);
                            }
                            else
                            {
                                Console.WriteLine("Error: Not enough in stock");
                            }
                        }
                        break;

                    case 'D':
                    case 'd':
                        //select one of the products
                        //throw it away
                        Console.WriteLine("Which product would you like to delete");
                        selection = int.Parse(Console.ReadLine() ?? "-1");
                        CartServiceProxy.Current.Delete(selection);
                        break;

                    default:
                        Console.WriteLine("Error: Unknown Command");
                        break;
                    
                }

            } while (choice != 'B' && choice != 'b');

        InventoryMenu(list, cart);
            
        }

        static void Checkout(List<Product?> cart)
        {
            decimal Total = 0;
            decimal SalesTax = 0.07m;
            Console.WriteLine("Receipt");
            foreach (Product? product in cart)
            {
                Console.WriteLine(product?.Display);
            }
            Total = CartServiceProxy.Current.calTotal();
            Console.WriteLine("Total: " + Total);
            Console.WriteLine("Sales Tax: " + Total * SalesTax);
            Console.WriteLine("Grand Total: " + Total * (1 + SalesTax));

            return;
        }
    }
}