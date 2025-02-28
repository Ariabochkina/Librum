using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library;
using Microsoft.Extensions.Options;
using ServiceStack;

namespace Project4_Library
{
    internal static class Change
    {
        internal static void ChangeBook(Books _books)
        {
            if (_books.books.Count == 0)
            {
                Console.WriteLine("Нет книг");
                Menu.Wait();
                return;
            }
            string[] strings = new string[_books.books.Count + 1];
            strings[strings.Length - 1] = "Назад";
            for (int i = 0; i < strings.Length - 1; i++)
            {
                strings[i] = _books.books[i].ToString();
            }
            var style = new Style().Foreground(Color.MediumPurple3);
            string book = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("[mediumpurple1]Выберете книгу для редактирования[/]")
                .HighlightStyle(style)
                .AddChoices(strings));
            
            
            for (int i = 0; i < strings.Length - 1; i++)
            {
                if (strings[i] == book)
                {
                    ChangeField(_books, i);
                    return;
                }
            }
        }
        private static void ChangeField(Books _books, int i)
        {
            string[] fields = ["Название", "Автор", "Жанр", "Год издания", "ISBN", "Оценка"];
            string[] options = ["Фантастика", "Детектив", "Роман", "История", "Научная литература"];
            var stylish = new Style().Foreground(Color.MistyRose3);
            string field = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("[hotpink3_1]Выберете поле для изменения[/]")
                .HighlightStyle(stylish)
                .AddChoices(fields));
            if (field == fields[0])
            {
                _books.books[i].Name = AnsiConsole.Prompt(new TextPrompt<string>("[hotpink3_1]Введите имя[/]"));
            }
            if (field == fields[1])
            {
                _books.books[i].Author = AnsiConsole.Prompt(new TextPrompt<string>("[hotpink3_1]Введите автора[/]"));
            }
            if (field == fields[2])
            {
                var styles = new Style().Foreground(Color.MistyRose3);
                _books.books[i].Genre = AnsiConsole.Prompt(new SelectionPrompt<string>()
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
                        _books.books[i].Date = AnsiConsole.Prompt(new TextPrompt<int>("[hotpink3_1]Введите год издания[/]"));
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
                        _books.books[i].ISBN = AnsiConsole.Prompt(new TextPrompt<string>("[hotpink3_1]Введите ISBN[/]"));
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
                        _books.books[i].Grade = AnsiConsole.Prompt(new TextPrompt<int>("[hotpink3_1]Введите оценку[/]"));
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
