using Library;
using Spectre.Console;

namespace Project4_Library
{
    /// <summary>
    /// Класс, предоставляющий метод для удаления книги из библиотеки.
    /// </summary>
    internal static class Delete
    {
        /// <summary>
        /// Метод, который выводит список книг, и предлагает пользователю выбрать, какую из них удалить.
        /// </summary>
        /// <param name="_books">Библиотека, из которой будет удалена книга</param>
        internal static void DeleteBook(AllBooks _books)
        {
            if (_books.Books.Count == 0)
            {
                Console.WriteLine("Нет книг");
                Menu.Wait();
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
                .Title("[mediumpurple1]Выберете книгу для удаления[/]")
                .HighlightStyle(style)
                .AddChoices(strings));
            for (int i = 0; i < strings.Length - 1; i++)
            {
                if (strings[i] == book)
                {
                    _books.Books.Remove(_books.Books[i]);
                    return;
                }
            }
        }
    }
}