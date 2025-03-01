using Library;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project4_Library
{
    internal static class Cover
    {

        internal static void SaveCover(Books _books)
        {
            if (_books.books.Count == 0)
            {

                Console.WriteLine("Нет книг");
                Menu.Wait();
                return;
            }
            string[] strings = new string[_books.books.Count + 1];
            for (int i = 0; i < strings.Length - 1; i++)
            {
                strings[i] = _books.books[i].ToString();
            }
            strings[strings.Length - 1] = "Назад";
            var style = new Style().Foreground(Color.MediumPurple3);
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
                        _books.books[i].GetCover();
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
