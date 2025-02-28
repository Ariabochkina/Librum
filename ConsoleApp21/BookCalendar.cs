using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library;
using Spectre.Console;

namespace Project4_Library
{
    internal class BookCalendar
    {
        private Table _table = new Table();
        private Books _books = new Books();
        public BookCalendar(Books books) 
        {
            _books = books;
            int century = ChooseCentury();
            _table.Border = TableBorder.Double;
            _table.BorderColor(Color.MediumPurple3);
            CreateCalendar(century, _books);
            AnsiConsole.Write(_table);
        }
        private int ChooseCentury()
        {
            var style = new Style().Foreground(Color.MediumPurple3);
            List<int> centures = new List<int>();
            for (int i = 15; i <= 21; i++)
            {
                centures.Add(i);
            }
            int century = AnsiConsole.Prompt(new SelectionPrompt<int>()
                    .Title($"[mediumpurple1]Выберете столетие, по которому хотите посмотреть календарь[/]\n"+
                    "[mediumpurple1](Например - \"16\" это с 1501 по 1600)[/]")
                    .HighlightStyle(style)
                    .AddChoices(centures));
            return century;
        }
        private int CountBooksInYear(int year, Books books)
        {
            int count = (from p in books.books where p.Date == year select p).Count();
            return count;
        }
        private string ChooseColor(string year, int count) 
        {
            if (count == 0)
            {
                return "[grey15]" + year + "[/]";
            }
            if (count == 1)
            {
                return "[grey30]" + year + "[/]";
            }
            if (count == 2)
            {
                return "[grey39]" + year + "[/]";
            }
            if (count == 3)
            {
                return "[grey46]" + year + "[/]";
            }
            if (count == 5)
            {
                return "[grey54]" + year + "[/]";
            }
            if (count == 7)
            {
                return "[grey70]" + year + "[/]";
            }
            if (count == 15)
            {
                return "[grey85]" + year + "[/]";
            }
            else
            {
                return year;
            }
        }
        private void CreateCalendar(int century, Books books)
        {
            for (int i = 1; i <= 10; i++)
            {
                _table.AddColumn("");
            }
            for (int i = 0; i < 10; i++)
            {
                List<string> years = new List<string>();
                for (int j = 1; j <= 10; j++) 
                {
                    int year = (century - 1) * 100 + i * 10 + j;
                    int count = CountBooksInYear(year, books);
                    years.Add(ChooseColor(year.ToString(), count));
                }
                _table.AddRow(years.ToArray());
            }
        }
    }
}
