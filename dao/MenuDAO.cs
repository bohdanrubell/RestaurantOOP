using RestaurantAppOOP.db;
using RestaurantAppOOP.models;

namespace RestaurantAppOOP.dao;

public class MenuDAO
{
    public List<Menu> FindAllDishes()
    {
        using var _context = new RestaurantContext();
        return _context.Menus.ToList();
    }

    public void AddNewItemMenu(string name,string discription,decimal cost)
    {
        using var _context = new RestaurantContext();
        var newItem = new Menu
        {
            Name = name,
            Description = discription,
            Cost = cost
        };
        _context.Menus.Add(newItem);
        _context.SaveChanges();
    }
    public void AddNewItemMenu(string name,decimal cost)
    {
        using var _context = new RestaurantContext();
        var newItem = new Menu
        {
            Name = name,
            Cost = cost
        };
        _context.Menus.Add(newItem);
        _context.SaveChanges();
    }

    public void DeleteItemMenu(string itemName)
    {
        using var _context = new RestaurantContext();
        var delMenu = _context.Menus.SingleOrDefault(i => i.Name == itemName);
        if (delMenu != null)
        {
            _context.Menus.Remove(delMenu);
            _context.SaveChanges();
        }
        Console.WriteLine("Delete item was completed.");
    }
}