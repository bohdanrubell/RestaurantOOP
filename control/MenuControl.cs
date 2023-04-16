using System.ComponentModel;
using RestaurantAppOOP.dao;
using RestaurantAppOOP.models;

namespace RestaurantAppOOP.control;

public class MenuControl
{
    private static MenuControl instance;
    private static MenuDAO dao = new MenuDAO();

    private MenuControl() { }

    public static MenuControl getInstance()
    {
        if (instance == null)
        {
            instance = new MenuControl();
        }
        return instance;
    }

    public void AddNewItem(string name, string discr, decimal cost)
    {
        dao.AddNewItemMenu(name,discr,cost);
    }
    public void AddNewItem(string name, decimal cost)
    {
        dao.AddNewItemMenu(name,cost);
    }
}