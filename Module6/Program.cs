using System.Data;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Globalization;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using Data;
using Models;

internal class Program
{
    private enum Menu
    {
        Додавання,
        Редагування,
        Видалення,
        Вихід,
    }

    private enum Mode
    {
        Однокористувацький,
        Багатокористувацький,
    }

    private enum Property
    {
        Назва,
        Студія,
        Стиль,
        Дата,
        Режим,
        Кількість,
    }

    public static void PrintInfo(string text, List<Game> items)
    {
        Console.WriteLine();
        if (items.Count > 0)
        {
            Console.WriteLine(text);
            Console.WriteLine();
            Console.WriteLine("{0,-30} {1,-25} {2,-20} {3,-15} {4,-15} {5,-15}", "Назва гри", "Студія", "Стиль", "Дата релізу", "Режим", "Кількість проданих копій");
            Console.WriteLine();

            foreach (var item in items)
            {
                Console.WriteLine("{0,-30} {1,-25} {2,-20} {3,-15} {4,-15} {5,-15}", item.Name, item.Studio, item.Style, item.DateRelease, item.GameplayMode, item.NumberSold);
            }
        }
        else
        {
            Console.WriteLine("Інформацію не знайдено.");
        }

        Console.WriteLine();
    }

    public static int MultipleChoice(bool canCancel, Enum userEnum, int spacingPerLine = 20, int optionsPerLine = 5, int startX = 1, int startY = 1)
    {
        int currentSelection = 0;
        ConsoleKey key;
        Console.CursorVisible = false;
        int length = Enum.GetValues(userEnum.GetType()).Length;
        do
        {
            for (int i = 0; i < length; i++)
            {
                Console.SetCursorPosition(startX + ((i % optionsPerLine) * spacingPerLine), startY + (i / optionsPerLine));

                if (i == currentSelection)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }

                Console.Write(Enum.Parse(userEnum.GetType(), i.ToString()));

                Console.ResetColor();
            }

            key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    {
                        if (currentSelection % optionsPerLine > 0)
                        {
                            currentSelection--;
                        }

                        break;
                    }

                case ConsoleKey.RightArrow:
                    {
                        if (currentSelection % optionsPerLine < optionsPerLine - 1)
                        {
                            currentSelection++;
                        }

                        break;
                    }

                case ConsoleKey.UpArrow:
                    {
                        if (currentSelection >= optionsPerLine)
                        {
                            currentSelection -= optionsPerLine;
                        }

                        break;
                    }

                case ConsoleKey.DownArrow:
                    {
                        if (currentSelection + optionsPerLine < length)
                        {
                            currentSelection += optionsPerLine;
                        }

                        break;
                    }

                case ConsoleKey.Escape:
                    {
                        if (canCancel)
                        {
                            return -1;
                        }

                        break;
                    }
            }
        } while (key != ConsoleKey.Enter);

        Console.CursorVisible = true;

        return currentSelection;
    }

    private static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        using (DataContex dc = new DataContex())
        {
            var valorant = new Game("Valorant", "Riot Games", "Shooter", new DateOnly(2020, 6, 2, new JulianCalendar()), Game.Mode.Multiplayer, 15000000);
            var dota2 = new Game("Dota 2", "Valve Corporation", "MOBA", new DateOnly(2013, 7, 9, new JulianCalendar()), Game.Mode.Multiplayer, 500000);
            var genshin = new Game("Genshin Impact", "miHoYo", "RPG", new DateOnly(2020, 12, 10, new JulianCalendar()), Game.Mode.SinglePlayer, 40000000);
            var cyberpunk = new Game("Cyberpunk 2077", "CD Projekt RED", "action RPG", new DateOnly(2020, 9, 28, new JulianCalendar()), Game.Mode.SinglePlayer, 15000000);
            var cs2 = new Game("Counter-Strike 2", "Valve Corporation", "Shooter", new DateOnly(2023, 9, 27, new JulianCalendar()), Game.Mode.Multiplayer, 1400000);
            var theWitcher3 = new Game("The Witcher 3: Wild Hunt", "CD Projekt RED", "Action RPG", new DateOnly(2015, 5, 19, new JulianCalendar()), Game.Mode.SinglePlayer, 30000000);
            var amongUs = new Game("Among Us", "InnerSloth", "Social Deduction", new DateOnly(2018, 6, 15, new JulianCalendar()), Game.Mode.Multiplayer, 50000000);
            var horizonZeroDawn = new Game("Horizon Zero Dawn", "Guerrilla Games", "Action RPG", new DateOnly(2017, 2, 28, new JulianCalendar()), Game.Mode.SinglePlayer, 20000000);
            var minecraft = new Game("Minecraft", "Mojang Studios", "Sandbox", new DateOnly(2011, 11, 18, new JulianCalendar()), Game.Mode.Multiplayer, 238000000);
            var skyrim = new Game("The Elder Scrolls V: Skyrim", "Bethesda Game Studios", "Action RPG", new DateOnly(2011, 11, 11, new JulianCalendar()), Game.Mode.SinglePlayer, 30000000);

            dc.Games.Add(valorant);
            dc.Games.Add(dota2);
            dc.Games.Add(genshin);
            dc.Games.Add(cyberpunk);
            dc.Games.Add(cs2);
            dc.Games.Add(theWitcher3);
            dc.Games.Add(amongUs);
            dc.Games.Add(horizonZeroDawn);
            dc.Games.Add(minecraft);
            dc.Games.Add(skyrim);
            dc.SaveChanges();

            var games = dc.Games.ToList();
            PrintInfo("Всі гри: ", games);
            Console.WriteLine("Тут і в подальшому для продовження натисніть будь-яку клавішу.");
            Console.ReadKey();
            Console.Clear();

            Console.WriteLine("Пошук за назвою гри \nВведіть назву гри: ");
            string inputName = Convert.ToString(Console.ReadLine());
            var gameSearchByName = dc.Games.Where(g => g.Name == inputName).ToList();
            PrintInfo("Інформація про цю гру: ", gameSearchByName);
            Console.ReadKey();
            Console.Clear();

            Console.WriteLine("Пошук за назвою студії \nВведіть назву студії: ");
            string inputStudio = Convert.ToString(Console.ReadLine());
            var gamesSearchByStudio = dc.Games.Where(g => g.Studio == inputStudio).ToList();
            PrintInfo("Ігри цієї студії: ", gamesSearchByStudio);
            Console.ReadKey();
            Console.Clear();

            Console.WriteLine("Пошук за стилем гри \nВведіть стиль гри: ");
            string inputStyle = Convert.ToString(Console.ReadLine());
            var gamesSearchByStyle = dc.Games.Where(g => g.Style == inputStyle).ToList();
            PrintInfo("Ігри цього стилю: ", gamesSearchByStyle);
            Console.ReadKey();
            Console.Clear();

            Console.WriteLine("Пошук за роком випуску гри \nВведіть рік випуску гри: ");
            int inputYear = Convert.ToInt32(Console.ReadLine());
            var gamesSearchByYear = dc.Games.Where(g => g.DateRelease >= new DateOnly(inputYear, 1, 1, new JulianCalendar()) && g.DateRelease <= new DateOnly(inputYear, 12, 31, new JulianCalendar())).ToList();
            PrintInfo("Ігри цього року: ", gamesSearchByYear);
            Console.ReadKey();
            Console.Clear();

            var gamesSingle = dc.Games.Where(g => g.GameplayMode == Game.Mode.SinglePlayer).ToList();
            PrintInfo("Однокористувацькі ігри: ", gamesSingle);
            Console.ReadKey();
            Console.Clear();

            var gamesMulti = dc.Games.Where(g => g.GameplayMode == Game.Mode.Multiplayer).ToList();
            PrintInfo("Багатокористувацькі ігри: ", gamesMulti);
            Console.ReadKey();
            Console.Clear();

            var gamesMostPopular = dc.Games.OrderByDescending(g => g.NumberSold).Take(3).ToList();
            PrintInfo("Топ-3 найпопулярніших ігор (за кількістю проданих копій): ", gamesMostPopular);
            Console.ReadKey();
            Console.Clear();

            var gamesLeastPopular = dc.Games.OrderBy(g => g.NumberSold).Take(3).ToList();
            PrintInfo("Топ-3 найнепопулярніших ігор (за кількістю проданих копій): ", gamesLeastPopular);
            Console.ReadKey();
            Console.Clear();

            Console.WriteLine("*управління за допомогою стрілочок*");
            while (true)
            {
                int input = MultipleChoice(true, new Menu());
                switch ((Menu)input)
                {
                    case Menu.Додавання:
                        {
                            Console.Clear();
                            Console.WriteLine("Введіть назву гри: ");
                            string name = Convert.ToString(Console.ReadLine());
                            Console.Clear();

                            Console.WriteLine("Введіть назву студії гри: ");
                            string studio = Convert.ToString(Console.ReadLine());
                            Console.Clear();

                            Console.WriteLine("Введіть стиль гри: ");
                            string style = Convert.ToString(Console.ReadLine());
                            Console.Clear();

                            DateOnly dateRelease;
                            while (true)
                            {
                                Console.WriteLine("Введіть дату релізу гри (у форматі yyyy-MM-dd): ");
                                string dateInput = Console.ReadLine();

                                if (DateOnly.TryParse(dateInput, out dateRelease))
                                {
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Некоректний формат дати!");
                                }
                            }
                            Console.Clear();

                            Console.WriteLine("Виберіть режим гри");
                            Game.Mode mode = Game.Mode.Multiplayer;
                            int inputMode = MultipleChoice(true, new Mode());

                            switch ((Mode)inputMode)
                            {
                                case Mode.Однокористувацький:
                                    mode = Game.Mode.SinglePlayer;
                                    break;
                                case Mode.Багатокористувацький:
                                    mode = Game.Mode.Multiplayer;
                                    break;
                                default:
                                    break;
                            }

                            Console.Clear();

                            Console.WriteLine("Введіть кількість проданих копій гри: ");
                            int count = Convert.ToInt32(Console.ReadLine());

                            Game newGame = new Game(name, studio, style, dateRelease, mode, count);
                            dc.Games.Add(newGame);
                            dc.SaveChanges();
                            Console.Clear();
                            break;
                        }

                    case Menu.Редагування:
                        {
                            Console.Clear();
                            while (true)
                            {
                                Console.WriteLine("Введіть назву гри, дані якої хочете змінити: ");
                                string name = Convert.ToString(Console.ReadLine());
                                Console.Clear();

                                if (dc.Games.FirstOrDefault(g => g.Name == name) != null)
                                {
                                    int inputProperty = MultipleChoice(true, new Property());

                                    switch ((Property)inputProperty)
                                    {
                                        case Property.Назва:
                                            Console.Clear();
                                            Console.WriteLine("Введіть нову назву гри: ");
                                            string newName = Convert.ToString(Console.ReadLine());
                                            dc.Games.FirstOrDefault(g => g.Name == name).Name = newName;
                                            break;

                                        case Property.Студія:
                                            Console.Clear();
                                            Console.WriteLine("Введіть нову студію гри: ");
                                            string newStudio = Convert.ToString(Console.ReadLine());
                                            dc.Games.FirstOrDefault(g => g.Name == name).Studio = newStudio;
                                            break;

                                        case Property.Стиль:
                                            Console.Clear();
                                            Console.WriteLine("Введіть нову стиль гри: ");
                                            string newStyle = Convert.ToString(Console.ReadLine());
                                            dc.Games.FirstOrDefault(g => g.Name == name).Style = newStyle;
                                            break;

                                        case Property.Дата:
                                            Console.Clear();
                                            DateOnly newdateRelease;
                                            while (true)
                                            {
                                                Console.WriteLine("Введіть нову дату релізу гри (у форматі yyyy-MM-dd): ");
                                                string dateInput = Console.ReadLine();

                                                if (DateOnly.TryParse(dateInput, out newdateRelease))
                                                {
                                                    break;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Некоректний формат дати!");
                                                }

                                            }

                                            dc.Games.FirstOrDefault(g => g.Name == name).DateRelease = newdateRelease;
                                            break;

                                        case Property.Режим:
                                            Console.Clear();
                                            Console.WriteLine("Виберіть новий режим гри: ");
                                            int inputGameMode = MultipleChoice(true, new Mode());

                                            switch ((Mode)inputGameMode)
                                            {
                                               case Mode.Однокористувацький:
                                                dc.Games.FirstOrDefault(g => g.Name == name).GameplayMode = Game.Mode.SinglePlayer;
                                                break;
                                               case Mode.Багатокористувацький:
                                                dc.Games.FirstOrDefault(g => g.Name == name).GameplayMode = Game.Mode.Multiplayer;
                                                break;
                                               default:
                                                break;
                                            }

                                            break;

                                        case Property.Кількість:
                                            Console.Clear();
                                            Console.WriteLine("Введіть нову кількість проданих копій гри: ");
                                            int newNumber = Convert.ToInt32(Console.ReadLine());
                                            dc.Games.FirstOrDefault(g => g.Name == name).NumberSold = newNumber;
                                            break;
                                        default:
                                            break;
                                    }

                                    dc.SaveChanges();
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Гру не знайдено.");
                                    Console.ReadKey();
                                    Console.Clear();
                                }
                            }

                            Console.Clear();
                            break;
                        }

                    case Menu.Видалення:
                        {
                            Console.Clear();
                            while (true)
                            {
                                Console.WriteLine("Введіть назву гри, яку хочете видалити: ");
                                string name = Convert.ToString(Console.ReadLine());
                                Console.Clear();

                                if (dc.Games.FirstOrDefault(g => g.Name == name) != null)
                                {
                                    dc.Games.Remove(dc.Games.FirstOrDefault(g => g.Name == name));
                                    dc.SaveChanges();
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Гру не знайдено.");
                                    Console.ReadKey();
                                    Console.Clear();
                                }
                                break;
                            }

                            Console.Clear();
                            break;
                        }

                    case Menu.Вихід:
                        Environment.Exit(0);
                        break;

                    default:
                        break;
                }

                var games1 = dc.Games.ToList();
                PrintInfo("Всі гри: ", games1);
                Console.ReadLine();
                Console.Clear();
            }
        }
    }
}