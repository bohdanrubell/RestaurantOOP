using RestaurantAppOOP;
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
            Console.WriteLine("3. Аналітика та звітність");
            Console.WriteLine("4. Вийти з програми");
            Console.Write("Ваш вибір: ");
            choiceGeneral = Convert.ToInt32(Console.ReadLine());

            switch (choiceGeneral)
            {
                case 1:
                    Console.Clear();
                    menuForProgram.orderProcessing();
                    break;
                case 2:
                    Console.Clear();
                    menuForProgram.menuRestaurantControl();
                    break;
                case 3:
                    //В розробці
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
/*
using (RestaurantContext db = new RestaurantContext())
{
    var data = from o in db.Orders
               join w in db.Waiters on o.IdWaiter equals w.Id
               join d in db.OrderedDishes on o.Id equals d.IdOrder
               join m in db.Menus on d.IdMenu equals m.Id
               where o.Id == 28
               group new { m.Name, d.Number, w.NameWaiter } by o into g
               select new
               {
                   OrderId = g.Key.Id,
                   OrderDate = g.Key.DateOrder,
                   Waiter = g.Select(x => x.NameWaiter).FirstOrDefault(),
                   Dishes = g.Select(x => new { Name = x.Name, Number = x.Number })
               };
    foreach (var item in data)
    {
        Console.WriteLine(item.OrderId);
        Console.WriteLine(item.OrderDate);
        Console.WriteLine(item.Waiter);
        foreach (var dish in item.Dishes)
        {
            Console.WriteLine($"\t{dish.Name} x {dish.Number}");
        }
    }*/

