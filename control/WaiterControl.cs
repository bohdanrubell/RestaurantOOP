using RestaurantAppOOP.dao;
using RestaurantAppOOP.models;

namespace RestaurantAppOOP.control;

public class WaiterControl
{
    private static WaiterControl instance;
    private static WaiterDAO dao = new WaiterDAO();

    private WaiterControl() { }

    public static WaiterControl getInstance()
    {
        if (instance == null)
        {
            instance = new WaiterControl();
        }
        return instance;
    }


    public List<Waiter> FindAllWaiters()
    {
        return dao.FindAll();
    }

    public void CheckWaiter(string name)
    {
        dao.IsWaiter(name);
    }

    public void NotWaiter(string name)
    {
        dao.IsNotWaiter(name);
    }

    public void CreateWaiter(string name)
    {
        dao.Create(name);
    }

    public void DeleteWaiter(string name)
    {
        dao.Delete(name);
    }
}