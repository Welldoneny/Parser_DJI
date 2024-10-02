using FirstEF6App;

namespace lb_6
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Вас приветствует программа-парсер сайта производителя дронов DJI\n" +
                "1 - обновить данные с сайта\n" +
                "2 - вывести данные с БД\n");
            while (true)
            {
                int x = Int32.Parse(Console.ReadLine());
                if (x == 1)
                {
                    try
                    {
                        using (DroneContext db = new DroneContext())
                        {
                            foreach (Drone item in db.Drones.ToList())
                            {
                                db.Remove(item);
                            }
                            db.SaveChanges();
                        }
                        var parser = new Parser();
                        string url = "https://dji-pro.ru/catalog/kvadrokoptery/";
                        parser.GetLinks(url);
                        List<string> links = await parser.GetLinks(url);
                        foreach (string link in links)
                        {
                            parser.Parse(link);
                        }
                        Console.WriteLine("Данные успешно обновлены");
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Не удалось обновить данные");
                    }

                }
                else if (x == 2)
                {
                    using (DroneContext db = new DroneContext())
                    {
                        var drones = db.Drones;
                        foreach (var item in drones)
                        {
                            Console.WriteLine(item.Id.ToString() + ": " + item.Name +  " - " + item.Price
                                + item.Characteristics);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Неизвестная команда");
                }
            }

        }
    }
}
