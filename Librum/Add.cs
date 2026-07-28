using Library;
using Spectre.Console;
//AI_COMMENTS
namespace Librum
{
    /// <summary>
    /// Класс, предоставляющий метод для добавления книги в библиотеку.
    /// </summary>
    internal static class Add
    {

        /// <summary>
        /// Метод добавления книги. Запрашивает информацию о книге (название, автор, жанр, дату, ISBN)
        /// и добавляет ее в библиотеку. Если информация некорректна, то выводится сообщение об ошибке.
        /// </summary>
        /// <param name="_books">Библиотека, в которую добавляется книга</param>
        internal static void AddBook(AllBooks _books)
        {
            while (true)
            {
               AnsiConsole.Clear();
                bool next = AnsiConsole.Prompt(new ConfirmationPrompt("[mediumpurple1]" + "Хотите продолжить?" + "[/]"));
                if (!next)
                {
                    return;
                }
               AnsiConsole.Clear();
                try
                {
                    string[] options = ["Фантастика", "Детектив", "Роман", "История", "Научная литература"];
                    string name = AnsiConsole.Prompt(new TextPrompt<string>("[mediumpurple1]Введите название[/]"));
                    string author = AnsiConsole.Prompt(new TextPrompt<string>("[mediumpurple1]Введите автора[/]"));
                    Style style = new Style().Foreground(Color.MediumPurple3); // Стиль для выбора жанра
                    string genre = AnsiConsole.Prompt(new SelectionPrompt<string>()
                        .Title("[mediumpurple1]Выберете жанр[/]")
                        .HighlightStyle(style)
                        .AddChoices(options));
                    int date = AnsiConsole.Prompt(new TextPrompt<int>("[mediumpurple1]Введите дату[/]"));
                    string isbn = AnsiConsole.Prompt(new TextPrompt<string>("[mediumpurple1]Введите ISBN[/]"));
                    int grade = -1;
                    Book book = new()
                    {
                        Name = name,
                        Author = author,
                        Genre = genre,
                        Date = date,
                        ISBN = isbn,
                        Grade = grade
                    };
                    _books.Add(book);
                    break;
                }
                catch (ArgumentException ex)
                {
                    string error = AnsiConsole.Prompt(new TextPrompt<string>($"[lightpink1]{ex.Message}" +
                        $"\nВведите любую букву чтобы продолжить[/]"));
                    Console.WriteLine(error);
                }
                catch (Exception)
                {
                    Console.WriteLine("Некорректный формат");
                    Menu.Wait();
                }
            }
        }
    }
}