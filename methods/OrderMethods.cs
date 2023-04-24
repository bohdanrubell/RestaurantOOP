using System.Globalization;
using ConsoleTableExt;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestaurantAppOOP.control;
using RestaurantAppOOP.db;
using RestaurantAppOOP.models;

namespace RestaurantAppOOP.methods;

public class OrderMethods
{
    private static RestaurantControl control = RestaurantControl.getInstance();
    private static WaiterControl wcontrol = WaiterControl.getInstance();

    public static void CreateOrd()
    {
        
        int waiterId = WaiterOrder(); // Вибираємо офіціанта за допомогоб методу
        int tableNumber = TableOrder(); // Вибираємо столик за допомгою методу
        var orderN = new Order
        {
            IdWaiter = waiterId,
            NumberOfTable = tableNumber,
            DateOrder = DateTime.Now
        }; // Створюємо екземпляр класу Order для ноовго замовлення

        ItemsOrder(orderN); // Додаємо елементи меню до замовлення за допомоги методу
        control.CreateNewOrderToDB(orderN); // Відправка замовлення на базу даних
        var idford = orderN.Id;
        Console.WriteLine($"Order #{idford} was added.");
    } // Метод для створення нового замовлення +

    private static int WaiterOrder()
    {
        int waiterId = 0;
        while (true)
        {
            Console.Write("Enter the waiter's name: ");
            string waiterName = Console.ReadLine();
            try
            {
                //Перевірка введеного офіцанта на наявнсть, якщо існує, передаємо його ідентифікатор
                waiterId = control.CheckingTheWaiterInOrder(waiterName);
                break;
            }
            catch (ArgumentException e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
                continue;
            }
            catch (FormatException)
            {
                Console.Clear();
                Console.WriteLine("Invalid format! Please,try again.");
            }
        } // Вибір офіціанта та перевірка на його наявність в базі даних

        return waiterId;
    }//+

    private static int TableOrder()
    {
        int tableNumber = 0;
        while (true)
        {
            Console.Write("Enter the table number: ");
            try
            {
                tableNumber = int.Parse(Console.ReadLine());
                if (tableNumber > 100)
                { Console.WriteLine("Error! The restaurant has no more than 100 tables!"); continue; }
                else { break; }
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid format. Please,try again");
                continue;
            }

        }
        return tableNumber;
    }//+

    private static void ItemsOrder(Order orderN)
    {
        string inputMenu = null;
        int dishID, number = 0;
        while (true)
        {
            MenuMethods.PrintAllItemMenu();
            Console.Write("Enter the name of the dish or enter 'end' to complete the entry: ");
            try
            {
                inputMenu = Console.ReadLine();
                if (inputMenu.ToLower() == "end")
                {
                    break;
                }
                else
                {
                    //Перевірка на наявність елемента в меню, якщо так повертаємо його ідентифікатор
                    dishID = control.ChekingItemMenuInDB(inputMenu);
                }
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
                continue;
            }

            var dishes = new Dictionary<string, int>(); // Тимчасове зберігання замовлених страв
            Console.Write("Enter the quantity of dish: ");
            try
            {
                // Введеня кількості поточного елемента меню 
                number = int.Parse(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid format. Please,try again.");
                continue;
            }

            /*Перевірка на наявність блюда в тим.сховищі, якщо так, до елементу(ключ)
             добавляємо певну кількість
            до поточної*/
            if (dishes.ContainsKey(inputMenu))
            {
                dishes[inputMenu] += number;
            }
            else
            {
                dishes.Add(inputMenu, number);
            }

            // Збереження елементу в тимчасове сховище
            foreach (var dis in dishes)
            {
                //Створюємо екземпяр класу, який відповідає за замовлені страви
                var orderedDish = new OrderedDish
                {
                    IdMenu = dishID,
                    Number = dis.Value
                };
                orderN.OrderedDishes.Add(orderedDish);
                Console.Clear();// Замовлений елемент меню присвоюємо до певного замовлення
            }
        } // Створення замовлених страв до замовлення
    }//+

    public static void PrintOrder()
    {
        int num = 0;
        bool loop = true;
        while (loop)
        {
            Console.Write("Order ID:"); // Вводимо ідентифікатор замовлення
            try
            {
                num = int.Parse(Console.ReadLine());
                loop = false;
            }
            catch (Exception)
            {
                Console.Clear();
                Console.WriteLine("Invalid format. Please, try again.");
                continue;
            }
            List<Order> orderP = new List<Order>();
             try
                {
                    orderP.AddRange(control.GetTheOrderForPrint(num));
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine($"Order #{num} didn't find.");
                   continue;
                }
             // Створюємо тимчасове сховище для виведення інформаціі замовлвення
                Console.WriteLine("┌─────────────────────────────────────────┐");
                Console.WriteLine($"│{"ID Order:",-10}  {orderP[0].Id,29}│"); 
                Console.WriteLine($"│{"Waiter:",-10} {orderP[0].IdWaiterNavigation.NameWaiter.Trim(),30}│");
                Console.WriteLine($"│{"Date order:",-10} {orderP[0].DateOrder.ToString("dd-MM-yyyy"),29}│");

                orderP.Clear();
                Console.WriteLine("├──────────────────┬───────────┬──────────|");
                Console.WriteLine("│      Name        | Quantity  |   Cost   │");
                Console.WriteLine("┣──────────────────┼───────────┼──────────┨");

                List<OrderedDish> dishesForPrint = new List<OrderedDish>();
                dishesForPrint.AddRange(control.GetOrderedDishes(num));
                foreach (var orderedDish in dishesForPrint)
                {
                    Console.WriteLine("│{0,18}│{1,11}│{2,10:F}│", orderedDish.IdMenuNavigation.Name, orderedDish.Number,
                        (orderedDish.Number * orderedDish.IdMenuNavigation.Cost));
                }
                var total = dishesForPrint.Sum(od => od.Number * od.IdMenuNavigation.Cost);
                Console.WriteLine("├──────────────────┴───────────┴──────────|");
                Console.WriteLine("│Total amount                  {0,10:F} │", total);
                Console.WriteLine("└─────────────────────────────────────────┘");
                dishesForPrint.Clear();
                loop = false;
        }
        
        
        //Виводимо інформацію в табличному вигляді
        
        // Виведення загальної суми замовлення
    } // Метод для виведення всієї інформації із замовлення + 

    public static void PrintAllOrders()
    {
        
        var order = control.FindAllOrders();
        if (order != null && order.Any())
        {
            foreach (Order or in order)
            {
                Console.WriteLine(" ───────────────────────────────");
                Console.WriteLine($"│{"ID Order:",0}  {or.Id,20}│");
                Console.WriteLine($"│{"Waiter's name: ",0} {or.IdWaiterNavigation.NameWaiter.Trim(),15}│");
                Console.WriteLine($"│{"Date Order:",0} {or.DateOrder.ToString("dd-MM-yyyy"),19}│");
                Console.WriteLine($"│{"Table:",0} {or.NumberOfTable,24}│");
                Console.WriteLine(" ───────────────────────────────");
                Console.WriteLine();
            }
            
            Console.WriteLine("For more information, go to function 2.");
        }
        else
        {
            Console.WriteLine("ERROR: No order found!");
        }
    } // Метод для виведення всіх замовлень які є в БД (скорочена) +

    public static void DeleteOrder()
    {
        
        Console.Write("Enter the order number you want to delete: ");
        try
        {
            int n = int.Parse(Console.ReadLine());
            control.DeleteTheOrder(n);
        }
        catch (FormatException)
        {
            Console.Clear();
            Console.WriteLine("Invalid format. Please,try again!");
        }
        catch (ArgumentNullException)
        {
            Console.Clear();
            Console.WriteLine("The order does not exist.");
        }
    } // Метод для видалення замовлення + 

    public static void UpdateOrderMenu()
    {
        int orderID = 0;
        while (true)
        {
            Console.Write("Enter the ID order: ");
            try
            {
                orderID = int.Parse(Console.ReadLine());
                control.CheckOrder(orderID);
                break;
            }
            catch (FormatException)
            {
                Console.Clear();
                Console.WriteLine("Invalid format. Please, try again.");
                continue;
            }
            catch (InvalidOperationException e )
            {
                Console.Clear();
                Console.WriteLine(e.Message);
                continue;
            }
        }
        
        int ch = 0;
        while (true)
        {
            PrintUpdateTable();
            try
            {
                Console.Write("Select a function: ");
                ch = int.Parse(Console.ReadLine());
                if (ch > 5)
                {
                    throw new ArgumentOutOfRangeException();
                }
                else
                {
                   break;
                }
            }
            catch (FormatException)
            {
                ch = 0;
                Console.Clear();
                Console.WriteLine("Invalid format. Please, try again.");
                continue;
            }
            catch (ArgumentOutOfRangeException)
            {
                ch = 0;
                Console.Clear();
                Console.WriteLine("Invalid selection. Please, try again. Must be [1-5]");
                continue;
            }
        }
       
       
        switch (ch)
        {
            case 1:
                updateWaiterInOrder(orderID);
                break;
            case 2:
                addTheItemsInOrder(orderID);
                break;
            case 3:
                updateQuantityDishesInOrder(orderID);
                break;
            case 4:
                deleleDishesInOrder(orderID);
                break;
            case 5:
                break;
            default:
                Console.WriteLine("Error.Try again.");
                break;
        }
    }// +

    private static void updateWaiterInOrder(int orderID)
    {
        string newWaiter = null;
        while (true)
        {
            Console.Write("Enter the new waiter name: ");
            try
            {
                newWaiter = Console.ReadLine();
                wcontrol.NotWaiter(newWaiter);
                break;
            }
            catch (FormatException)
            {
                Console.Clear();
                Console.WriteLine("Invalid format. Please, try again!");
                continue;
            }
            catch (InvalidOperationException e )
            {
                Console.Clear();
                Console.WriteLine(e);
                continue;
            }

        }
        control.UpdateWOrder(orderID, newWaiter);
    }// +

    private static void addTheItemsInOrder(int orderID)
    {
        List<OrderedDish> dd = control.GetOrderedDishes(orderID);
        Dictionary<string, int> newItems = new Dictionary<string, int>();
        foreach (var d in dd)
        {
            newItems.Add(d.IdMenuNavigation.Name, d.Number);
        }
        Console.WriteLine("Order menu now:");
        Console.WriteLine("┌─────────────────────────┐");
        foreach (var d in dd)
        {
            Console.WriteLine("│ {0,-10} │ {1,10} │", d.IdMenuNavigation.Name,d.Number);
        }

        Console.WriteLine("└─────────────────────────┘");
        string inputMenu = null;
        int quant = 0;
        while (true)
        {
           
                    
            Console.Write("Enter the name of the dish or enter 'end' to complete the entry: ");
            try
            {
                inputMenu = Console.ReadLine();
                if (inputMenu.ToLower() == "end")
                {
                    break;
                }
                else
                {
                    control.ChekingItemMenuInDB(inputMenu);
                }
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
                continue;
            }

            Console.WriteLine("Enter the quantity of dish: ");
            try
            {
                 quant = int.Parse(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid format. Please, try again.");
            }

            if (newItems.ContainsKey(inputMenu))
            {
                newItems[inputMenu] += quant;
            }
            else
            {
                newItems.Add(inputMenu, quant);
            }

            
        }
        control.UpdateList(orderID, newItems);

    }// +
    
    private static void updateQuantityDishesInOrder(int orderID)
    {
        List<OrderedDish> dd = control.GetOrderedDishes(orderID);
        Dictionary<string, int> newItems = new Dictionary<string, int>();
        foreach (var d in dd)
        {
            newItems.Add(d.IdMenuNavigation.Name, d.Number);
        }
        Console.WriteLine("Order menu now:");
        Console.WriteLine("┌─────────────────────────┐");
        foreach (var d in dd)
        {
            Console.WriteLine("│ {0,-10} │ {1,10} │", d.IdMenuNavigation.Name,d.Number);
        }

        Console.WriteLine("└─────────────────────────┘");
        dd.Clear();
        string upDish = null;
        int nquantity = 0;
        while (true)
        {


            Console.Write("Enter the name of the dish or enter 'end' to complete the entry: ");
            try
            {
                upDish = Console.ReadLine();
                if (upDish.ToLower() == "end")
                {
                    break;
                }
                else
                {
                    control.ChekingItemMenuInDB(upDish);
                }
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
                continue;
            }

            Console.WriteLine("Enter the quantity of dish: ");
            try
            {
                nquantity = int.Parse(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid format. Please, try again.");
            }
            if (newItems.ContainsKey(upDish))
            {
                newItems[upDish] = nquantity;
            }
            else
            {
                Console.WriteLine("Dish wasn`t found.");
            }

        }
        control.UpdateList(orderID, newItems);
    }// +

    private static void deleleDishesInOrder(int orderID)
    {
        RestaurantControl dao = RestaurantControl.getInstance();
        List<OrderedDish> dd = dao.GetOrderedDishes(orderID);
        Dictionary<string, int> dishesInOrder = new Dictionary<string, int>();
        foreach (var d in dd)
        {
            dishesInOrder.Add(d.IdMenuNavigation.Name, d.Number);
        }

        dd.Clear();
        Console.WriteLine("Order menu now:");
        Console.WriteLine("┌─────────────────────────┐");
        foreach (var d in dishesInOrder)
        {
            Console.WriteLine("│ {0,-10} │ {1,10} │", d.Key,d.Value);
        }

        Console.WriteLine("└─────────────────────────┘");
        string delDish = null;
        while (true)
        {
            Console.Write("Enter the name dish or enter end for exit: ");
            try
            {
                delDish = Console.ReadLine();
                if (delDish.ToLower() == "end")
                {
                    break;
                }
            }
            catch (FormatException)
            {
                Console.Clear();
                Console.WriteLine("Invalid format. Please, try again");
                continue;
            }
            
            if (dishesInOrder.ContainsKey(delDish))
            {
                dishesInOrder.Remove(delDish);
                dao.UpdateList(orderID, dishesInOrder);
            }
            else
            {
                Console.WriteLine("Dish was not found.");
            }
        }
    } // +-
    
    private static void PrintUpdateTable()
    {
        List<string> menuItems = new List<string>
        {
            "1. Waiter`s name",
            "2.Add new dishes in order",
            "3.Update dishes in order",
            "4.Delete dishes in order",
            "5. Return to menu"
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
            .WithTitle("What update?")
            .ExportAndWriteLine(TableAligntment.Left);
    }
    
}