using RestaurantAppOOP.control;
using RestaurantAppOOP.models;

namespace RestaurantAppOOP.methods;

public class MenuMethods
{
    private static MenuControl mcontrol = MenuControl.getInstance();
    private static RestaurantControl rcontrol = RestaurantControl.getInstance();

    public static void PrintAllItemMenu()
    {
        var menu = mcontrol.AllItemsPrint();
        Console.WriteLine("Available menu: ");
        foreach (var itm in menu)
        {
            Console.WriteLine($" {itm.Name} - {itm.Description} - {itm.Cost} ");
        }
    }

    public static void ChangeTheCostOfItem()
    {
        string nameDish = null;
        int recost,dishID = 0;
        while (true)
        {
            Console.Write("Enter the item name: ");
            try
            {
                nameDish = Console.ReadLine();
                dishID = rcontrol.ChekingItemMenuInDB(nameDish);
            }
            catch (FormatException)
            {
                Console.Clear();
                Console.WriteLine("Invalid format. Please, try again.");
                continue;
            }
            catch (ArgumentException e)
            {
                Console.Clear();
                Console.WriteLine(e);
                continue;
            }
            
            Console.Write("Enter price to change: ");
            try
            {
                recost = int.Parse(Console.ReadLine());
                break;
            }
            catch (FormatException)
            {
                Console.Clear();
                Console.WriteLine("Invalid format. Please, try again.");
                continue;
            }
            catch
            {
                Console.Clear();
                Console.WriteLine("Error!");
                continue;
            }
            
        }
        mcontrol.ChangeCostOfItem(dishID,recost);
        Console.WriteLine("The price has been successfully changed.");
        
    }

    public static void AddNewItemOfMenu()
    {
        string name,discription = null;
        decimal costItem = 0;
        while (true)
        {
            try
            {
                Console.Write("Enter the new item name: ");
                name = Console.ReadLine();
                if (mcontrol.AllItemsPrint().Any(itm => itm.Name == name))
                {
                    Console.Clear();
                    Console.WriteLine("This item already exists. Please enter a different name.");
                    continue;
                }
                Console.Write("Enter the description (if you want it): ");
                discription = Console.ReadLine();
                Console.Write("Enter the cost new dish: ");
                costItem = decimal.Parse(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.Clear();
                Console.WriteLine("Invalid format. Please, try again.");
                continue;
            }
            
            if (discription == null)
            {
                mcontrol.AddNewItem(name,costItem);
                Console.WriteLine("The item has been successfully added.");
                
            }
            else
            {
                mcontrol.AddNewItem(name,discription,costItem);
                Console.WriteLine("The item has been successfully added.");
                
            }
            Console.Write("Do you want to add another item? (y/n) ");
            string answer2 = Console.ReadLine().ToLower();

            if (answer2 == "n")
            {
                break;
            }
            
        }
    }

    public static void DeleteTheItemMenu()
    {
        bool loop = true;
        string name = null;
        while (loop)
        {
            Console.Write("Enter the name item: ");
            name = Console.ReadLine();
            if (mcontrol.AllItemsPrint().Any(itm => itm.Name == name))
            {
                mcontrol.DeleteItem(name);
                Console.WriteLine("The item was deleted.");
                loop = false;
            }
            else
            {
                Console.WriteLine("The item is not in the available menu.");
                continue;
            }
        }
    }
}