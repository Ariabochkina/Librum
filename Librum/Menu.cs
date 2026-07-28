using Spectre.Console; 
using Library; 
using Files;
//AI_COMMENTS
namespace Librum
{
    /// <summary>
    /// Класс Menu предоставляет текстовый пользовательский интерфейс для взаимодействия с библиотекой книг.
    /// </summary>
    public static class Menu
    {
        private static AllBooks? _books = null; // Библиотека книг
        private static string? _path = null; // Путь к файлу с данными
        private static bool _json = true; // Флаг для определения формата файла (JSON или CSV)

        /// <summary>
        /// Метод, который предлагает пользователю нажать любую клавишу перед продолжением работы программы.
        /// </summary>
        internal static void Wait()
        {
            Console.WriteLine("Нажмите любую клавишу чтобы продолжить");
            Console.ReadKey();
        }

        /// <summary>
        /// Метод, который предлагает пользователю ввести путь к файлу,
        /// из которого будет загружена информация о книгах.
        /// </summary>
        public static void Start()
        {
            while (_books == null) 
            {
               AnsiConsole.Clear();
                _path = AnsiConsole.Prompt(new TextPrompt<string>("[mediumpurple1]" + "Введите путь" + "[/]")); // Ввод пути к файлу
                try
                {
                    _books = new AllBooks(_path); 
                }
                catch (Exception ex) 
                {
                    Console.WriteLine(ex.Message); 
                    Wait();
                }
            }
            Run(); 
        }

        /// <summary>
        /// Метод, который предлагает пользователю выбрать формат ввода данных.
        /// </summary>
        /// <returns>Строка, содержащая выбранный формат ("JSON" или "CSV")</returns>
        private static string Format()
        {
            return AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("Выберете формат для ввода") 
                .AddChoices("JSON", "CSV")); 
        }

        /// <summary>
        /// Основной цикл программы, предоставляющий пользователю выбор операций.
        /// </summary>
        private static void Run()
        {
            string[] options = ["Просмотр всех книг", "Добавление новой книги",
                "Редактирование информации о книге", "Удаление книги", "Интерактивная таблица",
                "Календарь", "Рекоммендации", "Экспорт", "Импорт", "Исправление всех данных на основе openlibrary",
                "Добавление книги по ISBN", "Загрузка обложки по вашей книге", "Выход"];

            for (int i = 0; i < options.Length - 1; i++) // Применение стилей к опциям
            {
                options[i] = "[mediumpurple1]" + options[i] + "[/]";
            }
            options[^1] = "[hotpink3_1]" + options[^1] + "[/]"; // Стиль для опции "Выход"

            while (true) 
            {
                Style style = new Style().Foreground(Color.MistyRose3); // Стиль для выбора опций
                AnsiConsole.Clear();
                string selection = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("[hotpink3_1][italic]Выберете опцию[/][/]") 
                    .HighlightStyle(style) 
                    .AddChoices(options));

                if (selection == options[^1]) // Если выбрано "Выход"
                {
                    if (AnsiConsole.Prompt(new ConfirmationPrompt("Вы уверены, что хотите выйти? Все изменения сохранятся."))) // Подтверждение выхода
                    {
                        if (_json) // Если формат JSON
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
                        else // Если формат CSV
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
                }

                if (selection == options[0]) // Если выбрано "Просмотр всех книг"
                {
                   AnsiConsole.Clear();
                    Console.WriteLine(_books); 
                    Wait();
                }

                if (selection == options[1]) // Если выбрано "Добавление новой книги"
                {
                   AnsiConsole.Clear();
                    Add.AddBook(_books); 
                }

                if (selection == options[3]) // Если выбрано "Удаление книги"
                {
                   AnsiConsole.Clear();
                    Delete.DeleteBook(_books); 
                }

                if (selection == options[2]) // Если выбрано "Редактирование информации о книге"
                {
                   AnsiConsole.Clear();
                    Change.ChangeBook(_books); 
                }

                if (selection == options[4]) // Если выбрано "Интерактивная таблица"
                {
                   AnsiConsole.Clear();
                    BookTable table = new(_books); 
                }

                if (selection == options[5]) // Если выбрано "Календарь"
                {
                   AnsiConsole.Clear();
                    BookCalendar calendar = new(_books);
                    Wait();
                }

                if (selection == options[6]) // Если выбрано "Рекомендации"
                {
                   AnsiConsole.Clear();
                    Recommendation rec = new(_books); 
                    Wait();
                }

                if (selection == options[7]) // Если выбрано "Экспорт"
                {
                   AnsiConsole.Clear();
                    string path = AnsiConsole.Prompt(new TextPrompt<string>("[mediumpurple1]Введите путь[/]")); 
                    if (Format() == "JSON") // Если выбран JSON
                    {
                        try
                        {
                            JSON.ExportJson(path, _books); 
                        }
                        catch (Exception ex) 
                        {
                            Console.WriteLine(ex.Message);
                            Wait();
                        }
                    }
                    else // Если выбран CSV
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

                if (selection == options[8]) // Если выбрано "Импорт"
                {
                   AnsiConsole.Clear();
                    string path = AnsiConsole.Prompt(new TextPrompt<string>("[mediumpurple1]Введите путь[/]")); 
                    if (Format() == "JSON") // Если выбран JSON
                    {
                        try
                        {
                            _books = new AllBooks(path); 
                            _path = path;
                            _json = true;
                        }
                        catch (Exception ex) 
                        {
                            Console.WriteLine(ex.Message);
                            Wait();
                        }
                    }
                    else // Если выбран CSV
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

                if (selection == options[9]) // Если выбрано "Сверить и исправить данные на основе openlibrary"
                {
                    if (AnsiConsole.Prompt(new ConfirmationPrompt("[mediumpurple1]Вы уверены? В случае наличия данного ISBN, данные изменятся[/]"))) 
                    {
                        _books.Fetch(); 
                    }
                }

                if (selection == options[10]) // Если выбрано "Добавить книгу по ISBN"
                {
                    string newIsbn = AnsiConsole.Prompt(new TextPrompt<string>("[mediumpurple1]Введите ISBN[/]")); 
                    Book newBook = new();
                    try
                    {
                        newBook.ISBN = newIsbn;
                        newBook.Fetch(); // Загрузка данных по ISBN
                        _books.Add(newBook); 
                    }
                    catch
                    {
                        Console.WriteLine("Не удалось добавить книгу");
                        Wait();
                    }
                }

                if (selection == options[11]) // Если выбрано "Загрузить обложку"
                {
                    AnsiConsole.Clear();
                    Cover.SaveCover(_books); 
                    Wait();
                }
            }
        }
    }
}