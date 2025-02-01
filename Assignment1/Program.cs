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
            bool Done = false;
            Console.WriteLine("Welcome to Definetly not Amazon!");
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("I. Inventory");
            Console.WriteLine("C. Cart");

            List<Product?> list = InventoryServiceProxy.Current.Products;
            List<Product?> cart = CartServiceProxy.Current.Products;

            char choice = 'Z';
            while (!Done)
            {
                string? input = Console.ReadLine();
                choice = input[0];
                switch(choice) 
                {
                    case 'I':
                    case 'i':
                        InventoryMenu(list, cart, ref Done);
                        break;

                    case 'C':
                    case 'c':
                        CartMenu(cart, list, ref Done);
                        break;

                    default:
                        Console.WriteLine("Error: Unknown Command");
                        break;
                    
                }

            }

        }

        static void InventoryMenu(List<Product?> list, List<Product?> cart, ref bool Done)
        {
            Console.WriteLine("\nInventory");
            Console.WriteLine("C. Create new inventory item");
            Console.WriteLine("R. Read all inventory items");
            Console.WriteLine("u. Update an inventory item");
            Console.WriteLine("D. Delete an inventory item");
            Console.WriteLine("B. Go to cart");

            char choice = 'Z';
            do 
            {
                Console.WriteLine("\nWhat would you like to do?");
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

                    case 'B':
                    case 'b':
                        CartMenu(cart, list, ref Done);
                        break;  
                    default:
                        Console.WriteLine("Error: Unknown Command");
                        break;
                    
                }

            } while (!Done);
        }

        static void CartMenu(List<Product?> cart, List<Product?> list, ref bool Done)
        {
            Console.WriteLine("\n");
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
                Console.WriteLine("\nWhat would you like to do?");
                string? input = Console.ReadLine();
                choice = input[0];

                switch(choice) 
                {
                    case 'C':
                    case 'c':
                      //Checkout
                      //Makes an itemized receipt and prints out total including sales tax
                        Checkout(cart, ref Done);
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
                                selectedProd.Quantity -= quantity;
                                InventoryServiceProxy.Current.AddOrUpdate(selectedProd);
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
                            Console.WriteLine("Enter in the new quantity:");
                            int newQuantity = int.Parse(Console.ReadLine() ?? "0");
                            int diff = newQuantity - prodInCart.Quantity;
                            if (diff <= selectedProd.Quantity)
                            {   //Updates the cart when a user wants to add more of an item
                                prodInCart.Quantity = newQuantity;
                                CartServiceProxy.Current.AddOrUpdate(prodInCart);
                                selectedProd.Quantity -= diff;
                                InventoryServiceProxy.Current.AddOrUpdate(selectedProd);
                                Console.WriteLine("Item updated.");
                            }
                            else if (diff < 0)
                            {   //Updates the cart when a user wants to remove an item
                                prodInCart.Quantity = newQuantity;
                                CartServiceProxy.Current.AddOrUpdate(prodInCart);
                                selectedProd.Quantity += Math.Abs(diff);
                                InventoryServiceProxy.Current.AddOrUpdate(selectedProd);
                                Console.WriteLine("Item updated.");
                            }
                            else if (diff > selectedProd.Quantity)
                            {   //Should occur when someone tries to add more of an item than is in stock
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
                        int backToInventory = cart.FirstOrDefault(p => p.Id == selection).Quantity;
                        selectedProd = list.FirstOrDefault(p => p.Id == selection);
                        selectedProd.Quantity += backToInventory;
                        CartServiceProxy.Current.Delete(selection);
                        InventoryServiceProxy.Current.AddOrUpdate(selectedProd);
                        break;

                    case 'B':
                    case 'b':
                        InventoryMenu(list, cart, ref Done);
                        break;
                    default:
                        Console.WriteLine("Error: Unknown Command");
                        break;
                    
                }

            } while (!Done);
            
        }

        static void Checkout(List<Product?> cart, ref bool Done)
        {   //This is the only way to end the program
            //Prints out the receipt
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

            Done = true;
        }
    }
}