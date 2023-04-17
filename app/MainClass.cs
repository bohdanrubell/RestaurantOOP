using ConsoleTableExt;
using Microsoft.EntityFrameworkCore;
using RestaurantAppOOP.app;
using RestaurantAppOOP.db;
using RestaurantAppOOP.models;

public class MainClass
{
    public static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        GeneralMenu(); // Запуск основного меню
    }

    public static void GeneralMenu()
    {

        int choiceGeneral = 0;
        while (choiceGeneral != 4)
        {
        
            PrintGeneralMenuTable();
            
            Console.Write("Your choice: ");
            try
            {
                choiceGeneral = Convert.ToInt32(Console.ReadLine());
                if (choiceGeneral > 4)
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
            catch (FormatException e)
            {
                choiceGeneral = 0;
                Console.Clear();
                Console.WriteLine("Invalid format. Please, try again.");
            }
            catch (ArgumentOutOfRangeException)
            {
                choiceGeneral = 0;
                Console.Clear();
                Console.WriteLine("Invalid choise. Please, try again.");
            }

            
                switch (choiceGeneral)
                {
                    case 1:
                        Console.Clear();
                        MenuForProgram.orderProcessing();
                        break;
                    case 2:
                        Console.Clear();
                        MenuForProgram.menuRestaurantControl();
                        break;
                    case 3:
                        Console.Clear();
                        MenuForProgram.waitersControl();
                        break;
                    case 4:
                        Console.WriteLine("Goodbye!");
                        break;
                }

                Console.WriteLine();
            

        }
    } // Головне меню програми
    private static void PrintGeneralMenuTable()
    {
        List<string> MenuItems = new List<string>
        {
            "1. Order processing",
            "2. Menu management",
            "3. Management of waiters",
            "4. Exit the program"
        };
        ConsoleTableBuilder.From(MenuItems)
            .WithCharMapDefinition(
                CharMapDefinition.FramePipDefinition,
                new Dictionary<HeaderCharMapPositions, char>
                {
                    { HeaderCharMapPositions.TopLeft, '?' },
                    { HeaderCharMapPositions.TopCenter, '?' },
                    { HeaderCharMapPositions.TopRight, '?' },
                    { HeaderCharMapPositions.BottomLeft, '?' },
                    { HeaderCharMapPositions.BottomCenter, '?' },
                    { HeaderCharMapPositions.BottomRight, '?' },
                    { HeaderCharMapPositions.BorderTop, '?' },
                    { HeaderCharMapPositions.BorderRight, '?' },
                    { HeaderCharMapPositions.BorderBottom, '?' },
                    { HeaderCharMapPositions.BorderLeft, '?' },
                    { HeaderCharMapPositions.Divider, '?' },
                })
            .WithTitle("General menu")
            .ExportAndWriteLine(TableAligntment.Left);
    }
}

