using System.Net; 
using System.Text.Json;
using Spectre.Console;
//AI_COMMENTS
namespace Library
{
    /// <summary>
    /// Класс, который хранит информацию о книге.
    /// </summary>
    public class Book
    {
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (value is null)
                {
                    throw new FormatException("Некорректное имя");
                }
                _name = value;
            }
        }

        private string _author;
        public string Author
        {
            get => _author;
            set
            {
                if (value is null)
                {
                    throw new FormatException("Некорректное имя");
                }
                _author = value;
            }
        }
        private string _genre;

        public string Genre
        {
            get => _genre;
            set
            {
                if (value is null)
                {
                    throw new FormatException("Некорректное имя");
                }
                _genre = value;
            }
        }
        private int _date = 2025;
        public int Date
        {
            get => _date;
            set
            {
                if (value >= 1456 && value <= DateTime.Today.Year)
                {
                    _date = value;
                }
                else
                {
                    throw new ArgumentException("Неверная дата (доступно с 1456 по настоящий год)");
                }
            }
        }
        private string _isbn;
        public string ISBN
        {
            get => _isbn;
            set
            {
                if (!long.TryParse(value, out _) || (value.Length != 13 && value.Length != 10)) // Проверка на корректность ISBN
                {
                    throw new ArgumentException("Неверный ISBN");
                }
                if (value.Length != 13) // Проверка для ISBN-10
                {
                    if (value[^1] != 'X')
                    {
                        int s = 0;
                        for (int i = 0; i < 10; i++) // Расчет контрольной суммы для ISBN-10
                        {
                            s += (value[i] - '0') * (i + 1);
                        }
                        if (s % 11 != 0) // Проверка контрольной суммы
                        {
                            throw new ArgumentException("Неверный ISBN");
                        }
                    }
                }
                else // Проверка для ISBN-13
                {
                    int s = 0;
                    for (int i = 0; i < 13; i++) // Расчет контрольной суммы для ISBN-13
                    {
                        if (i % 2 == 0) // Четные и нечетные позиции обрабатываются по-разному
                        {
                            s += 1 * (value[i] - '0');
                        }
                        else
                        {
                            s += 3 * (value[i] - '0');
                        }
                    }
                    if (s % 10 != 0) // Проверка контрольной суммы
                    {
                        throw new ArgumentException("Неверный ISBN");
                    }
                }
                _isbn = value;
            }
        }

        private int _grade = -1;
        public int Grade
        {
            get => _grade;
            set
            {
                if (value <= 10 && value >= -1) // Проверка на корректность оценки (от -1 до 10)
                {
                    _grade = value;
                }
                else
                {
                    throw new ArgumentException("Некорректная оценка");
                }
            }
        }

        /// <summary>
        /// Метод, который возвращает строковое представление поля книги.
        /// </summary>
        public string GetField(string field)
        {
            if (field == "Name") { return Name; }
            if (field == "Author") { return Author; }
            if (field == "Genre") { return Genre; }
            if (field == "Date") { return Date.ToString(); }
            return Grade.ToString(); 
        }

        /// <summary>
        /// Метод, который загружает из интернета JSON-объект, описывающий книгу.
        /// </summary>
        private JsonElement GetJson()
        {
            string url = $"https://openlibrary.org/api/books?bibkeys=ISBN:{ISBN}&jscmd=data&format=json"; // Формирование URL для запроса
            WebClient client = new(); // Создание клиента для выполнения запроса
            string response = client.DownloadString(url); // Получение JSON-ответа
            JsonElement root;
            try
            {
                JsonDocument data = JsonDocument.Parse(response); 
                root = data.RootElement; // Получение корневого элемента
            }
            catch
            {
                throw new FormatException(); 
            }
            return root.GetProperty($"ISBN:{_isbn}"); // Возвращает данные по ISBN
        }

        /// <summary>
        /// Метод, который извлекает информацию о книге из JSON-объекта.
        /// </summary>
        public void Fetch()
        {
            try
            {
                JsonElement root = GetJson(); // Получение JSON-данных
                try { Name = root.GetProperty("title").GetString(); } catch { } // Обновление названия книги
                try
                {
                    if (root.GetProperty("publish_date").GetString().Length > 4) // Проверка формата даты
                    {
                        if (int.TryParse(root.GetProperty("publish_date").GetString()[^4..], out _)) // Извлечение года из строки
                        {
                            Date = int.Parse(root.GetProperty("publish_date").GetString()[^4..]);
                        }
                        else if (int.TryParse(root.GetProperty("publish_date").GetString()[0..4], out _))
                        {
                            Date = int.Parse(root.GetProperty("publish_date").GetString()[0..4]);
                        }
                    }
                    Date = int.Parse(root.GetProperty("publish_date").GetString()); // Обновление года издания
                }
                catch { }
                try
                {
                    List<JsonElement> authors = [.. root.GetProperty("authors").EnumerateArray()]; // Получение списка авторов
                    Author = authors[0].GetProperty("name").ToString(); // Обновление автора книги
                }
                catch { }
                try
                {
                    List<JsonElement> genres = [.. root.GetProperty("subjects").EnumerateArray()]; // Получение списка жанров
                    Genre = genres[0].GetProperty("name").ToString(); // Обновление жанра книги
                }
                catch { }
            }
            catch
            {
                throw new HttpRequestException(); 
            }
        }

        /// <summary>
        /// Метод, который загружает обложку книги из OpenLibrary API.
        /// </summary>
        public void GetCover()
        {
            JsonElement root = GetJson(); // Получение JSON-данных
            using WebClient client = new();
            try
            {
                string url = root.GetProperty("cover").GetProperty("large").GetString(); // Получение URL обложки
                client.DownloadFile(new Uri(url), $@"..\..\..\..\{_isbn}.png"); // Сохранение обложки в файл
               AnsiConsole.Clear();
                AnsiConsole.Write($"Путь от корня проекта: {_isbn}.png\n");
                AnsiConsole.Write(new CanvasImage($@"..\..\..\..\{_isbn}.png"));
            }
            catch
            {
                throw new HttpRequestException();
            }
        }

        /// <summary>
        /// Метод, который возвращает строковое представление книги.
        /// </summary>
        public override string ToString()
        {
            string text = "";
            text += $"Название: {Name}\n"; 
            text += $"Автор: {Author}\n"; 
            text += $"Жанр: {Genre}\n"; 
            text += $"Год издания: {Date}\n"; 
            return text; 
        }
    }
}