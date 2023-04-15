using System.Globalization;
using Microsoft.EntityFrameworkCore;
using RestaurantAppOOP.control;
using RestaurantAppOOP.db;
using RestaurantAppOOP.models;

namespace RestaurantAppOOP.methods;

public class OrderMethods
{
    public static void CreateOrd()
    {
        using (var context = new RestaurantContext())
        {
            Console.Write("Enter the waiter's name:");
            string waiterName = Console.ReadLine();
            Console.Write("Enter the table number:");
            int tableNumber = int.Parse(Console.ReadLine());

            int waiterId = context.Waiters.Single(w => w.NameWaiter == waiterName).Id;

            var orderN = new Order
            {
                IdWaiter = waiterId,
                NumberOfTable = tableNumber,
                DateOrder = DateTime.Now
            };
            var dishes = new Dictionary<string, int>();
            Console.WriteLine(
                "Enter the name of the dish and the quantity (for example: Borscht 1), or enter 'end' to complete the entry: ");
            while (true)
            {
                string inputMenu = Console.ReadLine();

                if (inputMenu.ToLower() == "end")
                {
                    break;
                }

                string[] menuParts = inputMenu.Split(' ');

                if (menuParts.Length != 2 || !int.TryParse(menuParts[1], out int quantity))
                {
                    Console.WriteLine("Invalid input format. Enter the dish name and quantity separated by a space.");
                    continue;
                }

                if (dishes.ContainsKey(menuParts[0]))
                {
                    dishes[menuParts[0]] += quantity;
                }
                else
                {
                    dishes.Add(menuParts[0], quantity);
                }
            }


            foreach (var dis in dishes)
            {
                var dishID = context.Menus.Single(d => d.Name == dis.Key).Id;

                var orderedDish = new OrderedDish
                {
                    IdMenu = dishID,
                    Number = dis.Value
                };
                orderN.OrderedDishes.Add(orderedDish);
            }

            context.Orders.Add(orderN);
            context.SaveChanges();
            var idford = orderN.Id;
            Console.WriteLine($"Order #{idford} was added.");
        }
    }

    public static void PrintOrder()
    {
        Console.Write("ID замовлення:");
        int num = Convert.ToInt32(Console.ReadLine());
        using (RestaurantContext _context = new RestaurantContext())
        {
            var orderW = _context.Orders
                .Include(o => o.IdWaiterNavigation)
                .Include(o => o.OrderedDishes)
                .ThenInclude(od => od.IdMenuNavigation)
                .FirstOrDefault(o => o.Id == num);
            if (orderW == null)
            {
                Console.WriteLine("Замовлення не знайдено");
                return;
            }

            string dt = orderW.DateOrder.ToString("yy-MM-dd");
            Console.WriteLine($"ID замовлення: {orderW.Id}");
            Console.WriteLine($"Офіціант: {orderW.IdWaiterNavigation.NameWaiter}");
            Console.WriteLine($"Дата замовлення {dt}");

            Console.WriteLine("┌──────────────────┬───────────┬──────────┐");
            Console.WriteLine("|      Name        | Quantity  |   Cost   |");
            Console.WriteLine("├──────────────────┼───────────┼──────────|");
            foreach (var orderedDish in orderW.OrderedDishes)
            {
                Console.WriteLine("│{0,18}│{1,11}│{2,10:F}│", orderedDish.IdMenuNavigation.Name, orderedDish.Number,
                    (orderedDish.Number * orderedDish.IdMenuNavigation.Cost));
            }

            var total = orderW.OrderedDishes.Sum(od => od.Number * od.IdMenuNavigation.Cost);
            Console.WriteLine("├──────────────────┴───────────┴──────────|");
            Console.WriteLine("|Total amount                  {0,10:F} |", total);
            Console.WriteLine("└─────────────────────────────────────────┘");
            // Виведення загальної суми замовлення
        }
    }

    public static void PrintAllOrders()
    {
        RestaurantControl control = RestaurantControl.getInstance();
        var order = control.FindAllOrders();
        foreach (Order or in order)
        {
            Console.WriteLine($"ID замовлення : {or.Id}");
            Console.WriteLine($"{or.IdWaiterNavigation.NameWaiter}");
            Console.WriteLine($"Дата замовлення: {or.DateOrder}");
            Console.WriteLine($"Столик, де було замовлено: {or.NumberOfTable}");
            Console.WriteLine("----------------------------------------------");
        }
    }

    public static void UpdateOrder()
    {
        RestaurantControl dao = RestaurantControl.getInstance();
        Console.WriteLine("What do you want to update?");
        Console.WriteLine("1. Waiter`s name");
        Console.WriteLine("2.Add new dishes in order");
        Console.WriteLine("3.Update dishes in order");
        Console.WriteLine("4.Delete dishes in order");

        int ch = int.Parse(Console.ReadLine());
        switch (ch)
        {
            case 1:
                Console.Write("Enter the order id:  ");
                int id = int.Parse(Console.ReadLine());
                Console.Write("Enter the new waiter name:  ");
                string newWaiter = Console.ReadLine();
                dao.UpdateWOrder(id, newWaiter);
                break;
            case 2:
                List<OrderedDish> dd = dao.GetOrderedDishes(8);
                Dictionary<string, int> newItems = new Dictionary<string, int>();
                foreach (var d in dd)
                {
                    newItems.Add(d.IdMenuNavigation.Name,d.Number);
                }
                foreach (var d in dd)
                {
                    Console.WriteLine($"{d.IdMenuNavigation.Name } X  {d.Number}");
                }
                Console.WriteLine(
                    "Enter the name of the dish and the quantity (for example: Borscht 1), or enter 'end' to complete the entry: ");
                while (true)
                {
                    string inputMenu = Console.ReadLine();

                    if (inputMenu.ToLower() == "end")
                    {
                        break;
                    }

                    string[] menuParts = inputMenu.Split(' ');

                    if (menuParts.Length != 2 || !int.TryParse(menuParts[1], out int quantity))
                    {
                        Console.WriteLine(
                            "Invalid input format. Enter the dish name and quantity separated by a space.");
                        continue;
                    }

                    if (newItems.ContainsKey(menuParts[0]))
                    {
                        newItems[menuParts[0]] += quantity;
                    }
                    else
                    {
                        newItems.Add(menuParts[0], quantity);
                    }
                    dao.UpdateList(8,newItems);
                }

                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            default:
                Console.WriteLine("Error.Try again.");
                break;
        }
    } 
        
    }
    
