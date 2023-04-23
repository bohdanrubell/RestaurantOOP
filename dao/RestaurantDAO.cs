using Microsoft.EntityFrameworkCore;
using RestaurantAppOOP.db;
using RestaurantAppOOP.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAppOOP.dao
{
    public class RestaurantDAO
    {
        public List<Order> GetTheOrder(int orderID)
        {
            using var _context = new RestaurantContext();
            var orderGet = _context.Orders
                .Include(o => o.IdWaiterNavigation)
                .Include(od => od.OrderedDishes)
                .FirstOrDefault(i => i.Id == orderID);
            return new List<Order> {orderGet};
        }

        public void isNotOrder(int orderID)
        {
            using var _context = new RestaurantContext();
            var check = _context.Orders.SingleOrDefault(i => i.Id == orderID);
            if (check == null)
            {
                throw new InvalidOperationException("Order not found!");
            }
        }
        
        public void CreateOrder(Order newOrder)
        {
            using var _context = new RestaurantContext();
            try
            {
                _context.Orders.Add(newOrder);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error!: {e.Message}");
            }
        }  // Створення нового замовлення
        
        public List<Order> FindAllOrd()
        {
            using var _context = new RestaurantContext();
            return _context.Orders
                .Include(o => o.IdWaiterNavigation)
                .ToList();
              
        } // Знаходження всіх замовлень

        public void DeleteOrder(int orderID)
        {
            using var _context = new RestaurantContext();
            var order = _context.Orders.Find(orderID);
            if (order != null)
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentNullException();
            }
            Console.WriteLine("Delete was completed.");
        } // Видалення певного замовлення 

        public void UpdateWaiterNameOrder(int orderID,string newWaiterName)
        {
            using var _context = new RestaurantContext();
            var _order = _context.Orders
                    .Include(o => o.IdWaiterNavigation)
                    .SingleOrDefault(o => o.Id == orderID)
                ;
            if (_order != null)
            {
                _order.IdWaiterNavigation.NameWaiter = newWaiterName;
                _context.SaveChanges();
            }
            else
            {
                Console.WriteLine($"Order with ID {orderID} not found.");
            }

        } // Змінити офіціанта в певному замовленні

        public List<OrderedDish> GetOrderedDishesForOrder(int orderID)
        {
            using var context = new RestaurantContext();
            return context.OrderedDishes
                .Include(od => od.IdMenuNavigation)
                .Where(od => od.IdOrder == orderID)
                .ToList();
            
        } // Отримати замовлені страви в певному замовленні

        public void UpdateListOfDishes(int orderID, Dictionary<string, int> newI)
        {
            using var context = new RestaurantContext();
            var order = context.Orders
                .Include(o => o.OrderedDishes)
                .SingleOrDefault(s => s.Id == orderID);
            order.OrderedDishes.Clear();

            foreach (var nDish in newI)
            {
                var dish = context.Menus.SingleOrDefault(d => d.Name == nDish.Key);

                var orderdDsh = new OrderedDish
                {
                    IdMenu = dish.Id,
                    Number = nDish.Value
                };
                order.OrderedDishes.Add(orderdDsh);

            }

            context.SaveChanges();
        } // Оновити список замовлених страв в певному замовлені
        
        public int CheckTheWaiter(string name)
        {
            using var _context = new RestaurantContext();
            var waiter = _context.Waiters.SingleOrDefault(w => w.NameWaiter == name);
            if (waiter != null)
            {
                return waiter.Id;
            }
            else
            {
                throw new ArgumentException($"Waiter with name {name} does not exist.");
            }
        } // Перевірка на наявність офіціанта в базі даних

        public int CheckItemM(string name)
        {
            using var _context = new RestaurantContext();
            var item = _context.Menus.SingleOrDefault(i => i.Name == name);
            if (item != null)
            {
                return item.Id;
            }
            else
            {
                throw new ArgumentException($"Item menu with name {name} does not exist.");
            }
        } // Перевірка на навяність елемента меню в базі даних
        
    }
}
