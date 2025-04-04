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
            int selectOrder = 0;
            while (selectOrder != 6)
            {
                PrintOrderTable();
                Console.Write("Select a function:");
                try
                {
                    selectOrder = Convert.ToInt32(Console.ReadLine());
                    if (selectOrder > 6)
                    {
                        
                        throw new ArgumentOutOfRangeException();
                    }
                }
                catch (FormatException)
                {
                    selectOrder = 0;
                    Console.Clear();
                    Console.WriteLine("Invalid format. Please, try again.");
                }
                catch (ArgumentOutOfRangeException)
                {
                    selectOrder = 0;
                    Console.Clear();
                    Console.WriteLine("Invalid selection. Please, try again. Must be [1-6]");
                }

                switch (selectOrder)
                {
                    case 1:
                        Console.Clear();
                        OrderMethods.CreateOrd(); // Створення нового замовлення
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
                        OrderMethods.DeleteOrder();
                        break;
                    case 5:
                        OrderMethods.UpdateOrderMenu();
                        break;
                    case 6:
                        Console.Clear();
                        break;
                    
                }
                Console.WriteLine();
            }
            
            
        }// +

        public static void menuRestaurantControl()
        {
            MenuControl dao = MenuControl.getInstance();
            Console.OutputEncoding = Encoding.UTF8;
            int selectMenu = 0;
            while (selectMenu != 5)
            {
                PrintMenuTable();
                try
                {
                    selectMenu = Convert.ToInt32(Console.ReadLine());
                    if (selectMenu > 5)
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                }
                catch (FormatException)
                {
                    selectMenu = 0;
                    Console.Clear();
                    Console.WriteLine("Invalid format. Please, try again.");
                }
                catch (ArgumentOutOfRangeException)
                {
                    selectMenu = 0;
                    Console.Clear();
                    Console.WriteLine("Invalid selection. Please,try again. Must be [1-4]");
                }
                switch (selectMenu)
                {
                    case 1:
                        Console.Clear();
                        MenuMethods.PrintAllItemMenu();
                        break;
                    case 2:
                        MenuMethods.ChangeTheCostOfItem();
                        break;
                    case 3:
                        MenuMethods.AddNewItemOfMenu();
                        break;
                    case 4:
                       MenuMethods.DeleteTheItemMenu();
                        break;
                    case 5:
                        Console.Clear();
                        break;
                }

                Console.WriteLine();
            }
        }

        public static void waitersControl()
        {
            int selectWaiter = 0;
            while (selectWaiter != 4)
            {
                PrintWaiterTable();
                Console.Write("Select a function:");
                try
                {
                    selectWaiter = Convert.ToInt32(Console.ReadLine());
                    if (selectWaiter > 4)
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                }
                catch (FormatException)
                {
                    selectWaiter = 0;
                    Console.Clear();
                    Console.WriteLine("Invalid format. Please, try again.");
                }
                catch (ArgumentOutOfRangeException)
                {
                    selectWaiter = 0;
                    Console.Clear();
                    Console.WriteLine("Invalid selection. Please, try again. Must be [1-4]");
                }
                
                switch (selectWaiter)
                {
                    case 1:
                        Console.Clear();
                        WaiterMethods.PrintAllWaiters();
                        break;
                    case 2:
                        Console.Clear();
                        WaiterMethods.CreateNewWaiter();
                        break;
                    case 3:
                        Console.Clear();
                        WaiterMethods.DeleteWaiter();
                        break;
                    case 4:
                        Console.Clear();
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
                "3. Add the new item",
                "4. Delete the item",
                "5. Return to the main menu"
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
        
        private static void PrintWaiterTable()
        {
            List<string> waiterItems = new List<string>
            {
                "1. Display the list of available waiters",
                "2. Add a new waiter",
                "3. Delete the waiter",
                "4. Return to the main menu"
            };
            ConsoleTableBuilder.From(waiterItems)
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
                .WithTitle("Waiter's control")
                .ExportAndWriteLine(TableAligntment.Left);
        }
    }
}