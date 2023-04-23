using System.Text;
using ConsoleTableExt;
using RestaurantAppOOP.control;
using RestaurantAppOOP.models;

namespace RestaurantAppOOP.methods;

public class WaiterMethods
{
    private static WaiterControl waoControl = WaiterControl.getInstance();
    public static void PrintAllWaiters()
    {
        List<string> waiters = new List<String>();
        foreach (var w in waoControl.FindAllWaiters())
        {
            waiters.Add(w.NameWaiter);
        }

        ConsoleTableBuilder.From(waiters)
            .WithCharMapDefinition(
                CharMapDefinition.FramePipDefinition,
                new Dictionary<HeaderCharMapPositions, char> {
                    {HeaderCharMapPositions.TopLeft, '?' },
                    {HeaderCharMapPositions.TopCenter, '?' },
                    {HeaderCharMapPositions.TopRight, '?' },
                    {HeaderCharMapPositions.BottomLeft, '?' },
                    {HeaderCharMapPositions.BottomCenter, '?' },
                    {HeaderCharMapPositions.BottomRight, '?' },
                    {HeaderCharMapPositions.BorderTop, '?' },
                    {HeaderCharMapPositions.BorderRight, '?' },
                    {HeaderCharMapPositions.BorderBottom, '?' },
                    {HeaderCharMapPositions.BorderLeft, '?' },
                    {HeaderCharMapPositions.Divider, '?' },
                })
            .ExportAndWriteLine(TableAligntment.Left);
        waiters.Clear();
    }
    
    public static void CreateNewWaiter()
    {
        string input = null;
        while (true)
        {
            Console.Write("Enter the name of the new waiter:");
            try
            {
                input = Console.ReadLine();
                waoControl.CheckWaiter(input);
                break;
            }
            catch (FormatException)
            {
                Console.Clear();
                Console.WriteLine("Invalid format. Please, try again.");
                continue;
            }
            catch (InvalidOperationException)
            {
                Console.Clear();
                Console.WriteLine("Waiter with this name already exists");
                continue;
            }
        }
        waoControl.CreateWaiter(input);
        Console.Clear();
        Console.WriteLine($@"Waiter {input} is added to DB.");
        
    } // Створення нового офіціанта +


    public static void DeleteWaiter()
    {
        bool loop = true;
        string name = null;
        while (loop)
        {
            PrintAllWaiters();
            Console.Write("Enter the name waiter: ");
            try
            {
                name = Console.ReadLine();
                if (name.ToLower() == "end")
                {
                    loop = false;
                }
                else
                {
                    waoControl.NotWaiter(name);
                    waoControl.DeleteWaiter(name);
                    Console.WriteLine($"Waiter {name} was deleted.");
                    loop = false;
                }
               
            }
            catch (InvalidOperationException )
            {
                Console.Clear();
                Console.WriteLine("The waiter is not in the list of waiters.");
                continue;
            }
        }
    }
}