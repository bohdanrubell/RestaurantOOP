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

    public void Create(string name)
    {
        using var _context = new RestaurantContext();
        var newWaiter = new Waiter { NameWaiter = name };
        _context.Waiters.Add(newWaiter);
        _context.SaveChanges();
    }
}