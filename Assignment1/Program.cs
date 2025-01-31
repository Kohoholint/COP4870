﻿using System;
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

            Console.WriteLine("C. Create new inventory item");
            Console.WriteLine("R. Read all inventory items");
            Console.WriteLine("u. Update an inventory item");
            Console.WriteLine("D. Delete an inventory item");

            List<Product?> list = ProductServiceProxy.Current.Products;

            char choice = 'Z';
            do 
            {
                string? input = Console.ReadLine();
                choice = input[0];
                switch(choice) 
                {
                    case 'C':
                    case 'c':
                        ProductServiceProxy.Current.AddOrUpdate(new Product
                        {
                            Name = Console.ReadLine()
                        });
                        break;

                    case 'R':
                    case 'r':
                        list.ForEach(Console.WriteLine);
                        break;

                    case 'U':
                    case 'u':
                        //Select one of the products
                        Console.WriteLine("Which product would you like to update?");
                        int selection = int.Parse(Console.ReadLine() ?? "-1");
                        var selectedProd = list.FirstOrDefault(p => p.Id == selection);

                        if (selectedProd != null)
                        {
                            selectedProd.Name = Console.ReadLine() ?? "ERROR";
                            ProductServiceProxy.Current.AddOrUpdate(selectedProd);
                        }
                        break;

                    case 'D':
                    case 'd':
                        //select one of the products
                        //throw it away
                        Console.WriteLine("Which product would you like to delete");
                        selection = int.Parse(Console.ReadLine() ?? "-1");
                        ProductServiceProxy.Current.Delete(selection);
                        break;

                    default:
                        Console.WriteLine("Error: Unknown Command");
                        break;
                    
                }

            } while (choice != 'Q' && choice != 'q');
            
            Console.ReadLine();
        }
    }
}