using RestaurantAppOOP.app;

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

            Console.WriteLine("МЕНЮ");
            Console.WriteLine("1. Обробка замовлень");
            Console.WriteLine("2. Управління меню");
            Console.WriteLine("3. Управління офіціантами");
            Console.WriteLine("4. Вийти з програми");
            Console.Write("Ваш вибір: ");
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
                    Console.WriteLine("До побачення!");
                    break;
                default:
                    Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                    break;
            }
            Console.WriteLine();


        }
    } // Головне меню програми
}

