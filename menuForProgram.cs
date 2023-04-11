using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                        
                        break;
                    case 2:
                        Console.Clear();
                        Console.Write("ID замовлення:");
                        int num = Convert.ToInt32(Console.ReadLine());
                        using (RestaurantContext db = new RestaurantContext())
                        {
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
                            }
                            break;
                        }
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
