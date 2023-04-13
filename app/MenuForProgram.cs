using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleTables;
using RestaurantAppOOP.db;
using RestaurantAppOOP.models;
using RestaurantAppOOP.dao;
using RestaurantAppOOP.control;
using ConsoleTableExt;

namespace RestaurantAppOOP.app
{
    public class MenuForProgram
    {
        public static void orderProcessing()
        {
            RestaurantControl control = RestaurantControl.getInstance();
            Console.OutputEncoding = Encoding.UTF8;
            int choice = 0;
            while (choice != 5)
            {
                Console.WriteLine("Виберіть функцію:");
                Console.WriteLine("1.Створити нове замовлення");
                Console.WriteLine("2.Вивести інформацію про певне замовлення");
                Console.WriteLine("3.Вивести список наявних замовлень");
                Console.WriteLine("4.Видалення замовлення");
                Console.WriteLine("5.Повернутись в головне меню");
                choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Console.Clear();

                        using (var context = new RestaurantContext())
                        {
                            string waiterName = Console.ReadLine();
                            int tableNumber = int.Parse(Console.ReadLine());

                            int waiterId = context.Waiters.Single(w => w.NameWaiter == waiterName).Id;

                            var orderN = new Order
                            {
                                IdWaiter = waiterId,
                                NumberOfTable = tableNumber,
                                DateOrder = DateTime.Now
                            };

                            var dishes = new List<Tuple<string, int>>
                            {
                                Tuple.Create("Суп", 100)

                            };
                            foreach (var dis in dishes)
                            {
                                var dishID = context.Menus.Single(d => d.Name == dis.Item1).Id;

                                var orderedDish = new OrderedDish
                                {
                                    IdMenu = dishID,
                                    Number = dis.Item2
                                };
                                orderN.OrderedDishes.Add(orderedDish);
                            }
                            context.Orders.Add(orderN);
                            context.SaveChanges();
                            var idford = orderN.Id;
                            Console.WriteLine($"Замовлення {idford} додано");
                        }

                        break;
                    case 2:
                        Console.Clear();
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

                            Console.WriteLine($"ID замовлення: {orderW.Id}");
                            Console.WriteLine($"Офіціант: {orderW.IdWaiterNavigation.NameWaiter}");
                            Console.WriteLine($"Дата замовлення {orderW.DateOrder}");

                            Console.WriteLine("\nПерелік страв:");
                            foreach (var orderedDish in orderW.OrderedDishes)
                            {
                                Console.WriteLine($"{orderedDish.IdMenuNavigation.Name} - {orderedDish.Number} порцій x {orderedDish.IdMenuNavigation.Cost} грн = {orderedDish.Number * orderedDish.IdMenuNavigation.Cost} грн");
                            }

                            // Виведення загальної суми замовлення
                            var total = orderW.OrderedDishes.Sum(od => od.Number * od.IdMenuNavigation.Cost);
                            Console.WriteLine($"\nЗагальна сума замовлення: {total} грн");

                        }
                        break;
                    case 3:
                        var order = control.FindAllOrders();
                        foreach (Order or in order)
                        {
                         Console.WriteLine($"ID замовлення : {or.Id}");
                         Console.WriteLine($"{or.IdWaiterNavigation.NameWaiter}");
                         Console.WriteLine($"Дата замовлення: {or.DateOrder}");
                         Console.WriteLine($"Столик, де було замовлено: {or.NumberOfTable}");
                         Console.WriteLine("----------------------------------------------");
                        }
                        
                        
                        break;
                        case 4:
                        Console.Write("Введіть номер замовлення, який хочете видалити:");
                        int n = int.Parse( Console.ReadLine() );
                        control.DeleteTheOrder(n);

                        break;
                        case 5:
                        break;
                    default:
                        Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                        break;
                }
                Console.WriteLine();
            }

        }

        public static void menuRestaurantControl()
        {
            var table = new ConsoleTable("Управління меню")
                .AddRow("Виберіть функцію:")
                .AddRow("1.Вивести меню ресторану")
                .AddRow("2.Змінити ціну страви")
                .AddRow("3.Повернутись в головне меню");
            Console.OutputEncoding = Encoding.UTF8;
            int choice = 0;
            while (choice != 3)
            {
                
                table.Write();
                choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        using (var context = new RestaurantContext())
                        {
                            var menuItems = context.Menus
                                .Select(m => new {m.Id,m.Description,m.Cost})
                                .ToList();
                            Console.WriteLine("Актуальне меню:");
                            ConsoleTableBuilder
                                .From(menuItems)
                                .ExportAndWriteLine();
                        }
                        break;
                    case 2:
                        string nameDish = Console.ReadLine();
                        Console.Write("Введіть ціну для змінення:");
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
                        Console.WriteLine("Ціна успішно змінена.");
                        break;
                    case 3:
                        Console.Clear();
                        break;
                    default:
                        Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                        break;
                }
                Console.WriteLine();
            }
        }

        public static void waitersControl()
        {
            WaiterControl dao = WaiterControl.getInstance();
            Console.OutputEncoding = Encoding.UTF8;
            int choice = 0;
            while (choice != 3)
            {
                Console.WriteLine("Виберіть функцію:");
                Console.WriteLine("1.Вивести список наявних офіціантів");
                Console.WriteLine("2.Додати нового офіціанта");
                Console.WriteLine("3.Повернутись в головне меню");
                choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Console.Clear();
                       
                        List<Waiter> waiters = dao.FindAllWaiters();
                            
                        foreach (var w in waiters)
                        {
                            Console.WriteLine("|===================================|");
                            Console.WriteLine($"|ID - {w.Id} | Ім'я - {w.NameWaiter} ");
                            
                        }
                            Console.WriteLine("|===================================|");

                        break;
                    case 2:
                        
                        Console.Write("Введіть ім'я нового офіціанту: ");
                        string name = Console.ReadLine();
                        dao.CreateWaiter(name);
                        break;
                    case 3:
                        Console.Clear();
                        break;
                    default:
                        Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                        break;
                }
                Console.WriteLine();
            }
        }
    }
}
