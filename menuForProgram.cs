using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleTables;

namespace RestaurantAppOOP
{
    public class menuForProgram
    {
        public static void orderProcessing()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            int choice = 0;
            while (choice != 3)
            {
                Console.WriteLine("Виберіть функцію:");
                Console.WriteLine("1.Створити нове замовлення");
                Console.WriteLine("2.Вивести інформацію про певне замовлення");
                Console.WriteLine("3.Повернутись в головне меню");
                choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Console.Clear();

                        using(var context = new RestaurantContext())
                        {
                            string waiterName = Console.ReadLine();
                            int tableNumber = int.Parse(Console.ReadLine());

                            int waiterId = context.Waiters.Single(w => w.NameWaiter == waiterName).Id;

                            var order = new Order
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
                                order.OrderedDishes.Add(orderedDish);
                            }
                            context.Orders.Add(order);
                            context.SaveChanges();
                            var idford = order.Id;
                            Console.WriteLine($"Замовлення {idford} додано");
                        }
                        
                        break;
                    case 2:
                        Console.Clear();
                        Console.Write("ID замовлення:");
                        int num = Convert.ToInt32(Console.ReadLine());
                        using (RestaurantContext _context = new RestaurantContext())
                        {
                            /*
                            var data = from o in db.Orders
                                       join w in db.Waiters on o.IdWaiter equals w.Id
                                       join d in db.OrderedDishes on o.Id equals d.IdOrder
                                       join m in db.Menus on d.IdMenu equals m.Id
                                       where o.Id == num
                                       group new { m.Name, d.Number, w.NameWaiter } by o into g
                                       select new
                                       {
                                           OrderId = g.Key.Id,
                                           OrderDate = g.Key.DateOrder,
                                           Waiter = g.Select(x => x.NameWaiter).FirstOrDefault(),
                                           Dishes = g.Select(x => new { Name = x.Name, Number = x.Number })
                                       };
                            foreach (var item in data)
                            {
                                Console.WriteLine(item.OrderId);
                                Console.WriteLine(item.OrderDate);
                                Console.WriteLine(item.Waiter);
                                foreach (var dish in item.Dishes)
                                {
                                    Console.WriteLine($"\t{dish.Name} x {dish.Number}");
                                }
                            }*/
                            var order = _context.Orders
                                .Include(o =>  o.IdWaiterNavigation)
                                .Include(o => o.OrderedDishes)
                                    .ThenInclude(od => od.IdMenuNavigation)
                                .FirstOrDefault(o => o.Id == num);
                            if(order == null)
                            {
                                Console.WriteLine("Замовлення не знайдено");
                                return;
                            }

                            Console.WriteLine($"ID замовлення: {order.Id}");
                            Console.WriteLine($"Офіціант: {order.IdWaiterNavigation.NameWaiter}");
                            Console.WriteLine($"Дата замовлення {order.DateOrder}");

                            Console.WriteLine("\nПерелік страв:");
                            foreach (var orderedDish in order.OrderedDishes)
                            {
                                Console.WriteLine($"{orderedDish.IdMenuNavigation.Name} - {orderedDish.Number} порцій x {orderedDish.IdMenuNavigation.Cost} грн = {orderedDish.Number * orderedDish.IdMenuNavigation.Cost} грн");
                            }

                            // Виведення загальної суми замовлення
                            var total = order.OrderedDishes.Sum(od => od.Number * od.IdMenuNavigation.Cost);
                            Console.WriteLine($"\nЗагальна сума замовлення: {total} грн");

                        }
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

        public static void menuRestaurantControl()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            int choice = 0;
            while (choice != 3)
            {
                Console.WriteLine("Виберіть функцію:");
                Console.WriteLine("1.Вивести меню ресторану");
                Console.WriteLine("2.Змінити ціну страви");
                Console.WriteLine("3.Повернутись в головне меню");
                choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                       Console.Clear();
                        using(var context = new RestaurantContext())
                        {
                            var menuItems = context.Menus.ToList();
                            Console.WriteLine("Актуальне меню:");
                            ConsoleTable.From(menuItems).Write();
                        }
                        break;
                    case 2:
                        string nameDish = Console.ReadLine();
                        Console.Write("Введіть ціну для змінення:");
                        int recost = int.Parse(Console.ReadLine());
                        using(var context = new RestaurantContext())
                        {
                            var item = context.Menus.Single(i => i.Name == nameDish).Id;
                            var dish = context.Menus.Find(item);
                            if(item != null)
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
    }
}
