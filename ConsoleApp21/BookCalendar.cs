using Library;
using Spectre.Console;
//AI_COMMENTS
namespace Project4_Library
{
    /// <summary>
    /// Класс, который создает календарь, отображающий количество книг,
    /// выпущенных в каждом году столетия, выбранного пользователем.
    /// </summary>
    internal class BookCalendar
    {
        private readonly Table _table = new();
        private readonly AllBooks _books = new();
        /// <summary>
        /// Метод создает экземпляр класса BookCalendar.
        /// </summary>
        /// <param name="books">Библиотека, для которой создается календарь</param>
        /// <remarks>
        ///  
        public BookCalendar(AllBooks books)
        {
            _books = books;
            int century = ChooseCentury(); // Выбор столетия пользователем
            _table.Border = TableBorder.Double; // Установка двойной границы для таблицы
            _table.BorderColor(Color.MediumPurple3); // Установка цвета границы таблицы
            CreateCalendar(century, _books);
            AnsiConsole.Write(_table);
        }
        /// <summary>
        /// Метод запрашивает у пользователя столетие, по которому
        /// хочет посмотреть календарь.
        /// </summary>
        /// <returns>Выбранное столетие</returns>
        /// <remarks>
        ///  
        ///
        private int ChooseCentury()
        {
            Style style = new Style().Foreground(Color.MediumPurple3);
            List<int> centures = [];
            for (int i = 15; i <= 21; i++)
            {
                centures.Add(i);
            }
            int century = AnsiConsole.Prompt(new SelectionPrompt<int>()
                    .Title($"[mediumpurple1]Выберете столетие, по которому хотите посмотреть календарь[/]\n" +
                    "[mediumpurple1](Например - \"16\" это с 1501 по 1600)[/]")
                    .HighlightStyle(style)
                    .AddChoices(centures));
            return century;
        }
        /// <summary>
        /// Метод подсчитывает количество книг, выпущенных в году year
        /// </summary>
        /// <param name="year">Год, количество книг которого нужно подсчитать</param>
        /// <param name="books">Библиотека, в которой нужно искать книги</param>
        /// <returns>Количество книг, выпущенных в году year</returns>
        /// <remarks>
        ///  
        ///
        private int CountBooksInYear(int year, AllBooks books)
        {
            int count = (from p in books.Books where p.Date == year select p).Count(); // Подсчет книг с помощью LINQ
            return count;
        }
        /// <summary>
        /// Метод, который по количеству книг, выпущенных в году year,
        /// выбирает цвет, в котором будет отображаться год в календаре.
        /// </summary>
        /// <param name="year">Год, цвет которого нужно выбрать</param>
        /// <param name="count">Количество книг, выпущенных в году year</param>
        /// <returns>Строка, которая будет отображаться в календаре</returns>
        /// <remarks>
        ///  
        ///
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
        /// <summary>
        /// Метод, который создает календарь, отображающий количество книг,
        /// выпущенных в каждом году столетия, выбранного пользователем.
        /// </summary>
        /// <param name="century">Выбранное столетие</param>
        /// <param name="books">Библиотека, для которой создается календарь</param>
        /// <remarks>
        ///  
        ///
        private void CreateCalendar(int century, AllBooks books)
        {
            for (int i = 1; i <= 10; i++)
            {
                _table.AddColumn("");
            }
            for (int i = 0; i < 10; i++)
            {
                List<string> years = [];
                for (int j = 1; j <= 10; j++)
                {
                    int year = ((century - 1) * 100) + (i * 10) + j; // Расчет года
                    int count = CountBooksInYear(year, books);
                    years.Add(ChooseColor(year.ToString(), count)); // Добавление года с цветовым оформлением
                }
                _table.AddRow(years.ToArray());
            }
        }
    }
}