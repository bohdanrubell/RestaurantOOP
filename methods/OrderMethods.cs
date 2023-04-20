using System.Globalization;
using ConsoleTableExt;
using Microsoft.EntityFrameworkCore;
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
        } // Вибір офіціанта та перевірка на його наявність в базі даних

        int tableNumber = 0;
        while (true)
        {
            Console.Write("Enter the table number: ");
            try
            {
                tableNumber = int.Parse(Console.ReadLine());
                if (tableNumber > 100)
                {
                    throw new ArgumentOutOfRangeException("Error! The restaurant has no more than 100 tables!");
                }
                else
                {
                    break;
                }
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
                continue;
            } // Виняток обробки для діапазону столиків
            catch (FormatException)
            {
                Console.WriteLine("Invalid format. Please,try again");
                continue;
            } // Виняток обробки формату даниих
        } // Вибір столика та перевірка діапазону

        var orderN = new Order
        {
            IdWaiter = waiterId,
            NumberOfTable = tableNumber,
            DateOrder = DateTime.Now
        }; // Створюємо екземпляр класу Order для ноовго замовлення

        string inputMenu = null;
        int dishID, number = 0;
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
                orderN.OrderedDishes.Add(orderedDish); // Замовлений елемент меню присвоюємо до певного замовлення
            }
        } // Створення замовлених страв до замовлення

        control.CreateNewOrderToDB(orderN); // Відправка замовлення на базу даних
        var idford = orderN.Id;
        Console.WriteLine($"Order #{idford} was added.");
    } // Метод для створення нового замовлення +

    public static void PrintOrder()
    {
        Console.Write("Order ID:"); // Вводимо ідентифікатор замовлення
        int num = Convert.ToInt32(Console.ReadLine());
        List<Order> orderP = new List<Order>();
        orderP.AddRange(control.GetTheOrderForPrint(num)); // Створюємо тимчасове сховище для виведення інформаціі замовлвення
        if (orderP == null)
        {
            Console.WriteLine("Замовлення не знайдено");
            return;
        }
        //Виводимо інформацію в табличному вигляді
        string formattedDate = orderP[0].DateOrder.ToString("dd-MM-yyyy");
        Console.WriteLine("┌─────────────────────────────────────────┐");
        Console.WriteLine($"│{"ID Order:",-10}  {orderP[0].Id,29}│"); 
        Console.WriteLine($"│{"Waiter:",-10} {orderP[0].IdWaiterNavigation.NameWaiter.Trim(),30}│");
        Console.WriteLine($"│{"Date order:",-10} {formattedDate,29}│");

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
                Console.WriteLine($"│{"Date Order:",0} {or.DateOrder,6} │");
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
                Console.WriteLine(e);
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
        Console.WriteLine("======================");
        foreach (var d in dd)
        {
            Console.WriteLine($"{d.IdMenuNavigation.Name} X  {d.Number}");
        }

        Console.WriteLine("======================");
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
        Console.WriteLine("======================");
        foreach (var d in dd)
        {
            Console.WriteLine($"{d.IdMenuNavigation.Name} X  {d.Number}");
        }
        Console.WriteLine("======================");
        Console.WriteLine();
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
        Console.WriteLine("======================");
        foreach (var d in dishesInOrder)
        {
            Console.WriteLine($"{d.Key} X  {d.Value}");
        }

        Console.WriteLine("======================");
        while (true)
        {
            Console.Write("Enter the name dish or enter end for exit: ");
            string delDish = Console.ReadLine();
            if (delDish.ToLower() == "end")
            {
                break;
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