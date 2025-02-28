using Library;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project4_Library
{
    internal static class Add
    {

        internal static void AddBook(Books _books)
        {
            while (true)
            {
                Console.Clear();
                bool next = AnsiConsole.Prompt(new ConfirmationPrompt("[mediumpurple1]" + "Хотите продолжить?" + "[/]"));
                if (!next)
                {
                    return;
                }
                Console.Clear();
                try
                {
                    string[] options = ["Фантастика", "Детектив", "Роман", "История", "Научная литература"];
                    string name = AnsiConsole.Prompt(new TextPrompt<string>("[mediumpurple1]Введите название[/]"));
                    string author = AnsiConsole.Prompt(new TextPrompt<string>("[mediumpurple1]Введите автора[/]"));
                    var style = new Style().Foreground(Color.MediumPurple3);
                    string genre = AnsiConsole.Prompt(new SelectionPrompt<string>()
                        .Title("[mediumpurple1]Выберете жанр[/]")
                        .HighlightStyle(style)
                        .AddChoices(options));
                    int date = AnsiConsole.Prompt(new TextPrompt<int>("[mediumpurple1]Введите дату[/]"));
                    string isbn = AnsiConsole.Prompt(new TextPrompt<string>("[mediumpurple1]Введите ISBN[/]"));
                    int grade = -1;
                    Book book = new Book();
                    book.Name = name;
                    book.Author = author;
                    book.Genre = genre;
                    book.Date = date;
                    book.ISBN = isbn;
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
