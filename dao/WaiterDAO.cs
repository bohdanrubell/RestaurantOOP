using Microsoft.EntityFrameworkCore;
using RestaurantAppOOP.db;
using RestaurantAppOOP.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAppOOP.dao;

public class WaiterDAO
{
    public List<Waiter> FindAll()
    {
        using var _context = new RestaurantContext();
        return _context.Waiters.ToList();
    }

    public void IsWaiter(string name)
    {
        using var _context = new RestaurantContext();
        var check = _context.Waiters.SingleOrDefault(w => w.NameWaiter == name);
        if (check != null)
        {
            throw new InvalidOperationException("Waiter with this name already exists");
        }
    }
    public void IsNotWaiter(string name)
    {
        using var _context = new RestaurantContext();
        var check = _context.Waiters.SingleOrDefault(w => w.NameWaiter == name);
        if (check == null)
        {
            throw new InvalidOperationException("Waiter not found!");
        }
    }

    public void Create(string name)
    {
        using var _context = new RestaurantContext();
        var newWaiter = new Waiter { NameWaiter = name };
        _context.Waiters.Add(newWaiter);
        _context.SaveChanges();
    }

    public void Delete(string name)
    {
        using var _context = new RestaurantContext();
        var nameWaiter = _context.Waiters.SingleOrDefault(wd => wd.NameWaiter == name);
        var delWaiter = _context.Waiters.SingleOrDefault(w => w.NameWaiter == name).Id;
        var ordrTheWaiter = _context.Orders.Where(o => o.IdWaiterNavigation.Id == delWaiter);
        _context.Orders.RemoveRange(ordrTheWaiter);
        if (delWaiter != null)
        {
            _context.Waiters.Remove(nameWaiter);
            _context.SaveChanges();
        }

        _context.SaveChanges();
        Console.WriteLine("Delete was completed.");
    }
}