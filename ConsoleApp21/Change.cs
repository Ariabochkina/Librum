using Spectre.Console;
using Library;
//AI_COMMENTS
namespace Project4_Library
{
    /// <summary>
    /// Класс, который содержит статические методы, позволяющие изменять информацию о книге.
    /// </summary>
    internal static class Change
    {
        /// <summary>
        /// Метод, который позволяет изменять информацию о книге.
        /// </summary>
        /// <param name="_books">Библиотека, в которой хранится книга, которую нужно изменить</param>
        
        internal static void ChangeBook(AllBooks _books)
        {
            if (_books.Books.Count == 0)
            {
                Console.WriteLine("Нет книг");
                Menu.Wait();
                return;
            }
            string[] allOptions = new string[_books.Books.Count + 1]; // Массив для отображения книг и опции "Назад"
            allOptions[^1] = "Назад";
            for (int i = 0; i < allOptions.Length - 1; i++)
            {
                allOptions[i] = _books.Books[i].ToString();
            }
            Style style = new Style().Foreground(Color.MediumPurple3);
            string book = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("[mediumpurple1]Выберете книгу для редактирования[/]")
                .PageSize(4)
                .HighlightStyle(style)
                .AddChoices(allOptions));


            for (int i = 0; i < allOptions.Length - 1; i++)
            {
                if (allOptions[i] == book)
                {
                    ChangeField(_books, i);
                    return;
                }
            }
        }
        /// <summary>
        /// Метод, который позволяет изменить информацию о конкретной книге.
        /// </summary>
        /// <param name="_books">Библиотека, в которой хранится книга, которую нужно изменить</param>
        /// <param name="i">Номер книги, которую нужно изменить</param>
        private static void ChangeField(AllBooks _books, int i)
        {
            string[] fields = ["Название", "Автор", "Жанр", "Год издания", "ISBN", "Оценка"];
            string[] options = ["Фантастика", "Детектив", "Роман", "История", "Научная литература"];
            Style stylish = new Style().Foreground(Color.MistyRose3);
            string field = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("[hotpink3_1]Выберете поле для изменения[/]")
                .HighlightStyle(stylish)
                .AddChoices(fields));
            if (field == fields[0])
            {
                _books.Books[i].Name = AnsiConsole.Prompt(new TextPrompt<string>("[hotpink3_1]Введите имя[/]"));
            }
            if (field == fields[1])
            {
                _books.Books[i].Author = AnsiConsole.Prompt(new TextPrompt<string>("[hotpink3_1]Введите автора[/]"));
            }
            if (field == fields[2])
            {
                Style styles = new Style().Foreground(Color.MistyRose3);
                _books.Books[i].Genre = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title("[hotpink3_1]Введите жанр[/]")
                    .HighlightStyle(styles)
                    .AddChoices(options));
            }
            if (field == fields[3])
            {
                while (true)
                {
                    try
                    {
                        _books.Books[i].Date = AnsiConsole.Prompt(new TextPrompt<int>("[hotpink3_1]Введите год издания[/]"));
                        break;
                    }
                    catch
                    {
                        string error = AnsiConsole.Prompt(new TextPrompt<string>($"[lightpink1]Такого не бывает..." +
                $"\nВведите любую букву чтобы продолжить[/]"));
                        Console.WriteLine(error);
                    }
                }
            }
            if (field == fields[4])
            {
                while (true)
                {
                    try
                    {
                        _books.Books[i].ISBN = AnsiConsole.Prompt(new TextPrompt<string>("[hotpink3_1]Введите ISBN[/]"));
                        break;
                    }
                    catch
                    {
                        string error = AnsiConsole.Prompt(new TextPrompt<string>($"[lightpink1]Такого не бывает..." +
                $"\nВведите любую букву чтобы продолжить[/]"));
                        Console.WriteLine(error);
                    }
                }
            }
            if (field == fields[5])
            {
                while (true)
                {
                    try
                    {
                        _books.Books[i].Grade = AnsiConsole.Prompt(new TextPrompt<int>("[hotpink3_1]Введите оценку[/]"));
                        break;
                    }
                    catch
                    {
                        string error = AnsiConsole.Prompt(new TextPrompt<string>($"[lightpink1]Такого не бывает..." +
                $"\nВведите любую букву чтобы продолжить[/]"));
                        Console.WriteLine(error);
                    }
                }
            }
        }
    }
}