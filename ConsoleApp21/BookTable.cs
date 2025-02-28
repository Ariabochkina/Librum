using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library;
using Spectre.Console;
using static System.Reflection.Metadata.BlobBuilder;

namespace Project4_Library
{
    internal class BookTable
    {
        private abstract class Action
        {
            public int Number { get; set; }
            public string Field { get; set; }
        }
        private class Sort : Action
        {
            public bool Ascending { get; set; }
        }
        private class Filter : Action
        {
            public IEnumerable<string> Fields { get; set; }
        }
        private List<Action> _actions = new List<Action>();
        private Table _table = new Table();
        private Books _books = new Books();
        private int _numberSort = 1;
        private int _numberFilter = 1;
        public BookTable(Books books)
        {
            _books = books;
            Update();
        }
        private int ToDelete()
        {

            List<string> actions = new List<string>();
            SelectionPrompt<string> prompt = new SelectionPrompt<string>();
            prompt.Title("[mediumpurple1]Выберете опцию для удаления[/]");
            var style = new Style().Foreground(Color.MediumPurple3);
            foreach (Action action in _actions) {
                if (action is Filter)
                {
                    actions.Add($"Фильтрация {action.Number}");
                    prompt.AddChoice($"Фильтрация {action.Number}");
                    
                }
                else
                {
                    actions.Add($"Сортировка {action.Number}");
                    prompt.AddChoice($"Сортировка {action.Number}");
                }
                prompt.HighlightStyle(style);
            }
            string option = AnsiConsole.Prompt(prompt);
            for (int i = 0; i < actions.Count; i++)
            {
                if (actions[i] == option)
                {
                    return i;
                }
            }
            return -1;
        }
        private string GetFild(string title)
        {
            string[] fields = ["Название", "Автор", "Жанр", "Год издания", "Оценка"];
            var style = new Style().Foreground(Color.MediumPurple3);
            string field = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title($"[mediumpurple1]Выберете поле для {title}[/]")
                    .HighlightStyle(style)
                    .AddChoices(fields));
            if (field == fields[0])
            {
                field = "Name";
            }
            if (field == fields[1])
            {
                field = "Author";
            }
            if (field == fields[2])
            {
                field = "Genre";
            }
            if (field == fields[3])
            {
                field = "Date";
            }
            if (field == fields[4])
            {
                field = "Grade";
            }
            return field;
        }
        private void Update()
        {
            Books filtered = new Books();
            filtered.books = new List<Book>();
            foreach (Book book in _books.books)
            {
                filtered.Add(book);
            }
            foreach (Action action in _actions)
            {
                if (action is Filter)
                {
                    Filter filter = (Filter)action;
                    filtered.Filter(action.Field, filter.Fields);
                }
                else
                {
                    Sort sort = (Sort)action;
                    filtered.Sort(action.Field, sort.Ascending);
                }
            }
            Console.Clear();
            _table = new Table();
            _table.Border = TableBorder.Double;
            _table.BorderColor(Color.MediumPurple3);
            _table.AddColumn("Название");
            _table.AddColumn("Автор");
            _table.AddColumn("Жанр");
            _table.AddColumn("Год издания");
            _table.AddColumn("Оценка");
            for (int i = 0; i < filtered.books.Count; i++)
            {
                Book book = filtered.books[i];
                _table.AddRow(book.Name, book.Author, book.Genre, book.Date.ToString(), book.Grade == -1 ? "Не оценено" : book.Grade.ToString());
            }
            string[] choose = ["Сортировка", "Фильтрация", "Удаление", "Выход"];
            AnsiConsole.Write(_table);
            var stylish = new Style().Foreground(Color.MistyRose3);
            string options = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("[hotpink3_1]Выберете опцию[/]")
                .HighlightStyle(stylish)
                .AddChoices(choose));
            if (options == choose[3])
            {
                bool answer = AnsiConsole.Prompt(new ConfirmationPrompt("[lightpink1]Уверены, что хотите выйти? (Изменения сохранятся)[/]"));
                if (answer)
                {
                    foreach (Action action in _actions)
                    {
                        if (action is Filter)
                        {
                            Filter filter = (Filter)action;
                            _books.Filter(action.Field, filter.Fields);
                        }
                        else
                        {
                            Sort sort = (Sort)action;
                            _books.Sort(action.Field, sort.Ascending);
                        }
                    }
                    return;
                }
            }
            if (options == choose[0])
            {
                string field = GetFild("сортировки");
                _actions.Add(new Sort { Field = field , Number = _numberSort++});
                bool ascending = AnsiConsole.Prompt(new ConfirmationPrompt("[hotpink3_1]По возрастанию?[/]"));
                (_actions[_actions.Count - 1] as Sort).Ascending = ascending;
            }
            if (options == choose[1])
            {
                string field = GetFild("фильтрации");
                var styles = new Style().Foreground(Color.MistyRose3);
                var values = AnsiConsole.Prompt(new MultiSelectionPrompt<string>()
                    .Title("[hotpink3_1]Выберете значения[/]")
                    .HighlightStyle(styles)
                    .AddChoices((from p in _books.books select p.GetField(field)).Distinct().ToList()));
                _actions.Add(new Filter { Field = field, Fields = values, Number = _numberFilter++ }); 
            }
            if (options == choose[2])
            {
                if (_actions.Count > 0)
                {
                    _actions.Remove(_actions[ToDelete()]);
                }
            }
            Update();
        }
    }
}
