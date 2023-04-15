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

            Console.WriteLine("Menu");
            Console.WriteLine("1. Order processing");
            Console.WriteLine("2. Menu management");
            Console.WriteLine("3. Management of waiters");
            Console.WriteLine("4. Exit the program");
            Console.Write("Your choice: ");
            choiceGeneral = Convert.ToInt32(Console.ReadLine());

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
                    MenuForProgram.waitersControl();
                    break;
                case 4:
                    Console.WriteLine("Goodbye!");
                    break;
                default:
                    Console.WriteLine("Wrong choice. Try again.");
                    break;
            }
            Console.WriteLine();


        }
    } // Головне меню програми
}

