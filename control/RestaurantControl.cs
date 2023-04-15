using RestaurantAppOOP.dao;
using RestaurantAppOOP.db;
using RestaurantAppOOP.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAppOOP.control;

    public class RestaurantControl
    {
        private static RestaurantControl instance;
        private static RestaurantDAO dao = new RestaurantDAO();

        private RestaurantControl() { }

    public static RestaurantControl getInstance()
    {
        if (instance == null)
        {
            instance = new RestaurantControl();
        }
        return instance;
    }

    public List<Order> FindAllOrders()
    {
        return dao.FindAllOrd();
    }

    public void DeleteTheOrder(int orderId)
    {
        dao.DeleteOrder(orderId);
    }

    public void UpdateWOrder(int orderID, string newName)
    {
        dao.UpdateWaiterNameOrder(orderID,newName);
    }

    public List<OrderedDish> GetOrderedDishes(int orderID)
    {
        return dao.GetOrderedDishesForOrder(orderID);
    }

    public void UpdateList(int ordId, Dictionary<string, int> nDis)
    {
        dao.UpdateListOfDishes(ordId,nDis);
    }
}
