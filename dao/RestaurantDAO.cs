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
            Console.WriteLine("Успішно видалено замовлення.");
        } // Видалення певного замовлення 
        
        
    }
}
