using System.Text;
using ConsoleTableExt;
using RestaurantAppOOP.control;
using RestaurantAppOOP.models;

namespace RestaurantAppOOP.methods;

public class WaiterMethods
{
    
    public static void PrintAllWaiters()
    {
        WaiterControl dao = WaiterControl.getInstance();
        List<string> waiters = new List<String>();

        foreach (var w in dao.FindAllWaiters())
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
            .WithTitle("Waiters available")
            .ExportAndWriteLine(TableAligntment.Left);

    }
    
    public static void CreateNewWaiter()
    {
        
        
        Console.Write("Enter the name of the new waiter:");
        string input = Console.ReadLine();
        WaiterControl dao = WaiterControl.getInstance();
        dao.CreateWaiter(input);
        Console.WriteLine($@"Waiter {input} is added to DB.");
    }


    public static void DeleteWaiter()
    {
        WaiterControl dao = WaiterControl.getInstance();
        Console.Write("Enter the name: ");
        string delW = Console.ReadLine();
        dao.DeleteWaiter(delW);
    }
}