using Spectre.Console;
using Library;
using System.Text.Json;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using static System.Net.Mime.MediaTypeNames;
using Files;
namespace Project4_Library
{
    public static class Menu
    {
        private static Books _books = null;
        private static string _path = null;
        private static bool _json = true;
        internal static void Wait()
        {
            Console.WriteLine("Нажмите любую клавишу чтобы продолжить");
            Console.ReadKey();
        }
        public static void Start()
        {
            while (_books == null)
            {
                Console.Clear();
                _path = AnsiConsole.Prompt(new TextPrompt<string>("[mediumpurple1]"+"Введите путь"+"[/]"));
                try
                {
                    _books = new Books(_path);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Wait();
                }
            }
            Run();
        }        
        private static string Format()
        {
            return AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("Выберете формат для ввода")
                .AddChoices("JSON", "CSV"));
        }
        private static void Run()
        {
            string[] options = ["Просмотр всех книг", "Добавление новой книги", 
                "Редактирование информации о книге", "Удаление книги", "Интерактивная таблица", 
                "Календарь", "Рекоммендации", "Экспорт", "Импорт", "Сверить и исправить данные на основе openlibrary", 
                "Добавить книгу по ISBN", "Загрузить обложку", "Выход"];
            for (int i = 0; i < options.Length - 1; i++)
            {
                options[i] = "[mediumpurple1]" + options[i] + "[/]";
            }
            options[options.Length - 1] = "[hotpink3_1]" + options[options.Length - 1] + "[/]";
            while (true)
            {
                var style = new Style().Foreground(Color.MistyRose3);
                Console.Clear();
                var selection = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("[hotpink3_1][italic]Выберете опцию[/][/]")
                    .HighlightStyle(style)
                    .AddChoices(options));
                if (selection == options[options.Length - 1])
                {

                    if (_json)
                    {
                        try
                        {
                            JSON.ExportJson(_path, _books);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Wait();
                        }
                    }
                    else
                    {
                        try
                        {
                            CSV.ExportCSV(_path, _books);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Wait();
                        }
                    }
                    return;
                }
                if (selection == options[0])
                {
                    Console.Clear();
                    Console.WriteLine(_books);
                    Wait();
                }
                if (selection == options[1])
                {
                    Console.Clear();
                    Add.AddBook(_books);
                }
                if (selection == options[3])
                {
                    Console.Clear();
                    Delete.DeleteBook(_books);
                }
                if(selection == options[2])
                {
                    Console.Clear();
                    Change.ChangeBook(_books);
                }
                if(selection == options[4])
                {
                    Console.Clear();
                    BookTable table = new BookTable(_books);
                }
                if (selection == options[5])
                {
                    Console.Clear();
                    BookCalendar calendar = new BookCalendar(_books);
                    Wait();
                }
                if (selection == options[6])
                {
                    Console.Clear();
                    Recommendation rec = new Recommendation(_books);
                    Wait();
                }
                if (selection == options[7])
                {
                    Console.Clear();
                    string path = AnsiConsole.Prompt(new TextPrompt<string>("Введите путь"));
                    if (Format() == "JSON")
                    {
                        try
                        {
                            JSON.ExportJson(path, _books);
                        }
                        catch(Exception ex) 
                        {
                            Console.WriteLine(ex.Message);
                            Wait();
                        }
                    }
                    else
                    {
                        try
                        {
                            CSV.ExportCSV(path, _books);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Wait();
                        }
                    }
                }
                if (selection == options[8])
                {
                    Console.Clear();
                    string path = AnsiConsole.Prompt(new TextPrompt<string>("Введите путь"));
                    if (Format() == "JSON")
                    {
                        try
                        {
                            _books = new Books(path);
                            _path = path;
                            _json = true;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Wait();
                        }
                    }
                    else
                    {
                        try
                        {
                            _books = CSV.ImportCSV(path);
                            _path = path;
                            _json = false;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Wait();
                        }
                    }
                }
                if (selection == options[9])
                {
                    if (AnsiConsole.Prompt(new ConfirmationPrompt("Вы уверены? В случае наличия данного ISBN, данные изменятся.")))
                    {
                        _books.Fetch();
                    }
                }
                if (selection == options[10])
                {
                    string newIsbn = AnsiConsole.Prompt(new TextPrompt<string>("Введите ISBN"));
                    Book newBook = new Book();
                    try
                    {
                        newBook.ISBN = newIsbn;
                        newBook.Fetch();
                        _books.Add(newBook);
                    }
                    catch
                    {
                        Console.WriteLine("Не удалось добавить книгу");
                        Wait();
                    }
                }
                if (selection == options[11])
                {
                    Console.Clear();
                    Cover.SaveCover(_books);
                    Wait();
                }
            }
        }
    }
}