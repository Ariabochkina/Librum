using Library; 
using Spectre.Console; 

namespace Project4_Library
{
    /// <summary>
    /// Класс, представляющий таблицу книг с возможностью сортировки и фильтрации.
    /// </summary>
    internal class BookTable
    {
        /// <summary>
        /// Абстрактный класс для действий над книгами, содержащий номер и поле действия.
        /// </summary>
        private abstract class Action
        {
            public int Number { get; set; } // Номер действия
            public string Field { get; set; } // Поле, к которому применяется действие
        }

        /// <summary>
        /// Класс, представляющий действие сортировки, с возможностью указания направления.
        /// </summary>
        private class Sort : Action
        {
            public bool Ascending { get; set; } // Направление сортировки (по возрастанию или убыванию)
        }

        /// <summary>
        /// Класс, представляющий действие фильтрации, с возможностью указания полей.
        /// </summary>
        private class Filter : Action
        {
            public IEnumerable<string> Fields { get; set; } 
        }

        private readonly List<Action> _actions = []; // Список действий (сортировок и фильтраций)
        private Table _table = new(); 
        private readonly AllBooks _books = new(); 
        private int _numberSort = 1; // Счетчик для нумерации сортировок
        private int _numberFilter = 1; // Счетчик для нумерации фильтраций

        /// <summary>
        /// Конструктор класса BookTable, инициализирует экземпляр с заданными книгами и обновляет таблицу.
        /// </summary>
        /// <param name="books">Библиотека книг, используемая для создания таблицы.</param>
        public BookTable(AllBooks books)
        {
            _books = books; 
            Update(); 
        }

        /// <summary>
        /// Метод, который выводит список существующих команд сортировки/фильтрации
        /// и предлагает пользователю выбрать, какую из них удалить.
        /// </summary>
        /// <returns>Номер команды, которую нужно удалить, или -1, если пользователь не выбрал ничего.</returns>
        private int ToDelete()
        {
            List<string> actions = []; // Список для отображения действий
            SelectionPrompt<string> prompt = new();
            prompt.Title("[mediumpurple1]Выберете опцию для удаления[/]");
            Style style = new Style().Foreground(Color.MediumPurple3);
            foreach (Action action in _actions) 
            {
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
            for (int i = 0; i < actions.Count; i++) // Поиск выбранного действия
            {
                if (actions[i] == option)
                {
                    return i; 
                }
            }
            return -1; 
        }

        /// <summary>
        /// Метод, который запрашивает у пользователя поле для сортировки/фильтрации
        /// и возвращает его название.
        /// </summary>
        /// <param name="title">Название поля, которое нужно выбрать</param>
        /// <returns>Строка, содержащая название поля</returns>
        private string GetFild(string title)
        {
            string[] fields = ["Название", "Автор", "Жанр", "Год издания", "Оценка"]; 
            Style style = new Style().Foreground(Color.MediumPurple3); // Стиль для выбора
            string field = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title($"[mediumpurple1]Выберете поле для {title}[/]") 
                    .HighlightStyle(style) 
                    .AddChoices(fields));
            if (field == fields[0]) { field = "Name"; }
            if (field == fields[1]) { field = "Author"; }
            if (field == fields[2]) { field = "Genre"; }
            if (field == fields[3]) { field = "Date"; }
            if (field == fields[4]) { field = "Grade"; }
            return field; 
        }

        /// <summary>
        /// Метод, который обновляет представление таблицы
        /// на основе существующих сортировок и фильтраций.
        /// </summary>
        private void Update()
        {
            AllBooks filtered = new(); // Создание временной библиотеки для фильтрации
            filtered.Books = [];
            foreach (Book book in _books.Books) // Копирование книг из основной библиотеки
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
            _table.Border = TableBorder.Double; // Установка двойной границы
            _table.BorderColor(Color.MediumPurple3); // Установка цвета границы
            _table.AddColumn("Название"); 
            _table.AddColumn("Автор");
            _table.AddColumn("Жанр");
            _table.AddColumn("Год издания");
            _table.AddColumn("Оценка");
            for (int i = 0; i < filtered.Books.Count; i++) // Заполнение таблицы данными
            {
                Book book = filtered.Books[i];
                _table.AddRow(book.Name, book.Author, book.Genre, book.Date.ToString(), book.Grade == -1 ? "Не оценено" : book.Grade.ToString());
            }
            string[] choose = ["Сортировка", "Фильтрация", "Удаление", "Выход"]; 
            AnsiConsole.Write(_table); 
            Style stylish = new Style().Foreground(Color.MistyRose3); 
            string options = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("[hotpink3_1]Выберете опцию[/]") 
                .HighlightStyle(stylish) 
                .AddChoices(choose)); 
            if (options == choose[3]) // Если выбрано "Выход"
            {
                bool answer = AnsiConsole.Prompt(new ConfirmationPrompt("[lightpink1]Уверены, что хотите выйти? (Изменения сохранятся)[/]"));
                if (answer) 
                {
                    foreach (Action action in _actions) // Применение всех действий к основной библиотеке
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
            if (options == choose[0]) // Если выбрано "Сортировка"
            {
                if (_books.Books.Count > 0) 
                {
                    string field = GetFild("сортировки"); 
                    _actions.Add(new Sort { Field = field, Number = _numberSort++ }); 
                    bool ascending = AnsiConsole.Prompt(new ConfirmationPrompt("[hotpink3_1]По возрастанию?[/]")); // Выбор направления
                    (_actions[^1] as Sort).Ascending = ascending; // Установка направления
                }
            }
            if (options == choose[1]) // Если выбрано "Фильтрация"
            {
                if (_books.Books.Count > 0) 
                {
                    string field = GetFild("фильтрации"); 
                    Style styles = new Style().Foreground(Color.MistyRose3);
                    List<string> values = AnsiConsole.Prompt(new MultiSelectionPrompt<string>()
                        .Title("[hotpink3_1]Выберете значения[/]") 
                        .HighlightStyle(styles) 
                        .AddChoices((from p in _books.Books select p.GetField(field)).Distinct().ToList())); // Уникальные значения для фильтрации
                    _actions.Add(new Filter { Field = field, Fields = values, Number = _numberFilter++ }); // Добавление фильтрации
                }
            }
            if (options == choose[2]) // Если выбрано "Удаление"
            {
                if (_actions.Count > 0) 
                {
                    _actions.Remove(_actions[ToDelete()]); 
                }
            }
            Update(); // Рекурсивное обновление таблицы
        }
    }
}