using Library;
using Spectre.Console;
namespace Project4_Library
{
    /// <summary>
    /// Класс, который предлагает пользователю загрузить обложку для выбранной книги.
    /// </summary>
    internal static class Cover
    {

        /// <summary>
        /// Метод, который позволяет пользователю загрузить обложку для выбранной книги.
        /// </summary>
        /// <param name="_books">Библиотека, в которой хранятся книги</param>
        internal static void SaveCover(AllBooks _books)
        {
            Console.Clear();
            if (_books.Books.Count == 0)
            {

                Console.WriteLine("Нет книг");
                return;
            }
            string[] strings = new string[_books.Books.Count + 1];
            for (int i = 0; i < strings.Length - 1; i++)
            {
                strings[i] = _books.Books[i].ToString();
            }
            strings[^1] = "Назад";
            Style style = new Style().Foreground(Color.MediumPurple3);
            string book = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("[mediumpurple1]Выберете книгу для которой хотите загрузить обложку[/]")
                .HighlightStyle(style)
                .AddChoices(strings));
            for (int i = 0; i < strings.Length - 1; i++)
            {
                if (strings[i] == book)
                {
                    try
                    {
                        _books.Books[i].GetCover();
                    }
                    catch
                    {
                        Console.WriteLine($"Не удалось найти обложку");
                    }
                    return;
                }
            }
        }
    }
}