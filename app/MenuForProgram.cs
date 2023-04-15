using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ConsoleTables;
using RestaurantAppOOP.db;
using RestaurantAppOOP.models;
using RestaurantAppOOP.dao;
using RestaurantAppOOP.control;
using ConsoleTableExt;
using RestaurantAppOOP.methods;

namespace RestaurantAppOOP.app
{
    public class MenuForProgram
    {
        public static void orderProcessing()
        {
            RestaurantControl control = RestaurantControl.getInstance();
            Console.OutputEncoding = Encoding.UTF8;
            int choice = 0;
            while (choice != 6)
            {
                PrintOrderTable();
                Console.Write("Make a choice:");
                choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        OrderMethods.CreateOrd();
                        break;
                    case 2:
                        Console.Clear();
                        OrderMethods.PrintOrder();
                        break;
                    case 3:
                        Console.Clear();
                        OrderMethods.PrintAllOrders();
                        break;
                    case 4:
                        Console.Write("Enter the order number you want to delete:");
                        int n = int.Parse(Console.ReadLine());
                        control.DeleteTheOrder(n);
                        break;
                    case 5:
                        OrderMethods.UpdateOrder();
                        break;
                    case 6:
                        break;
                    default:
                        Console.WriteLine("Wrong choice. Try again.");
                        break;
                }

                Console.WriteLine();
            }
        }

        public static void menuRestaurantControl()
        {
            Console.OutputEncoding = Encoding.UTF8;
            int choice = 0;
            while (choice != 3)
            {
                PrintMenuTable();
                choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        using (var context = new RestaurantContext())
                        {
                            var menuItems = context.Menus
                                .Select(m => new { m.Id, m.Name, m.Description, m.Cost })
                                .ToList();
                            Console.WriteLine("Current menu:");
                            ConsoleTableBuilder
                                .From(menuItems)
                                .ExportAndWriteLine();
                        }

                        break;
                    case 2:
                        string nameDish = Console.ReadLine();
                        Console.Write("Enter price to change:");
                        int recost = int.Parse(Console.ReadLine());
                        using (var context = new RestaurantContext())
                        {
                            var item = context.Menus.Single(i => i.Name == nameDish).Id;
                            var dish = context.Menus.Find(item);
                            if (item != null)
                            {
                                dish.Cost = recost;
                                context.SaveChanges();
                            }
                        }

                        Console.WriteLine("The price has been successfully changed.");
                        break;
                    case 3:
                        Console.Clear();
                        break;
                    default:
                        Console.WriteLine("Wrong choice. Try again.");
                        break;
                }

                Console.WriteLine();
            }
        }

        public static void waitersControl()
        {
            int choice = 0;
            while (choice != 3)
            {
                Console.WriteLine("Select a function:");
                Console.WriteLine("1. Display the list of available waiters");
                Console.WriteLine("2. Add a new waiter");
                Console.WriteLine("3. Return to the main menu");
                choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        WaiterMethods.PrintAllWaiters();
                        break;
                    case 2:
                        WaiterMethods.CreateNewWaiter();
                        break;
                    case 3:
                        Console.Clear();
                        break;
                    default:
                        Console.WriteLine("Wrong choice. Try again.");
                        break;
                }

                Console.WriteLine();
            }
        }

        private static void PrintOrderTable()
        {
            List<string> orderItems = new List<string>
            {
                "1. Create a new order",
                "2. Display information about a certain order",
                "3. Display a list of available orders",
                "4. Deleting an order",
                "5. Update the order",
                "6.Return to the main menu"
            };
            ConsoleTableBuilder.From(orderItems)
                .WithCharMapDefinition(
                    CharMapDefinition.FramePipDefinition,
                    new Dictionary<HeaderCharMapPositions, char>
                    {
                        { HeaderCharMapPositions.TopLeft, '?' },
                        { HeaderCharMapPositions.TopCenter, '?' },
                        { HeaderCharMapPositions.TopRight, '?' },
                        { HeaderCharMapPositions.BottomLeft, '?' },
                        { HeaderCharMapPositions.BottomCenter, '?' },
                        { HeaderCharMapPositions.BottomRight, '?' },
                        { HeaderCharMapPositions.BorderTop, '?' },
                        { HeaderCharMapPositions.BorderRight, '?' },
                        { HeaderCharMapPositions.BorderBottom, '?' },
                        { HeaderCharMapPositions.BorderLeft, '?' },
                        { HeaderCharMapPositions.Divider, '?' },
                    })
                .WithTitle("Order menu")
                .ExportAndWriteLine(TableAligntment.Left);
        }

        private static void PrintMenuTable()
        {
            List<string> menuItems = new List<string>
            {
                "1. Display the restaurant menu",
                "2. Change the price of a dish",
                "3. Return to the main menu"
            };
            ConsoleTableBuilder.From(menuItems)
                .WithCharMapDefinition(
                    CharMapDefinition.FramePipDefinition,
                    new Dictionary<HeaderCharMapPositions, char>
                    {
                        { HeaderCharMapPositions.TopLeft, '?' },
                        { HeaderCharMapPositions.TopCenter, '?' },
                        { HeaderCharMapPositions.TopRight, '?' },
                        { HeaderCharMapPositions.BottomLeft, '?' },
                        { HeaderCharMapPositions.BottomCenter, '?' },
                        { HeaderCharMapPositions.BottomRight, '?' },
                        { HeaderCharMapPositions.BorderTop, '?' },
                        { HeaderCharMapPositions.BorderRight, '?' },
                        { HeaderCharMapPositions.BorderBottom, '?' },
                        { HeaderCharMapPositions.BorderLeft, '?' },
                        { HeaderCharMapPositions.Divider, '?' },
                    })
                .WithTitle("Menu control")
                .ExportAndWriteLine(TableAligntment.Left);
        }
    }
}