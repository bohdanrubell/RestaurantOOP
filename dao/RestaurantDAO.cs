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

        }

        public List<OrderedDish> GetOrderedDishesForOrder(int orderID)
        {
            using var context = new RestaurantContext();
            return context.OrderedDishes
                .Include(od => od.IdMenuNavigation)
                .Where(od => od.IdOrder == orderID)
                .ToList();
            
        }

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
        }
        
    }
}
