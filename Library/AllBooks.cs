using System.Text.Encodings.Web; 
using System.Text.Json; 
using System.Text.Unicode;
//AI_COMMENTS
namespace Library
{
    /// <summary>
    /// Класс, который хранит информацию о библиотеке.
    /// </summary>
    public class AllBooks
    {
        public List<Book> Books { get; set; }

        /// <summary>
        /// Создает экземпляр AllBooks, загрузив информацию из файла.
        /// </summary>
        public AllBooks(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Нет такого пути");
            }
            AllBooks data = new();
            string json;
            JsonSerializerOptions options1 = EncodingSettings();
            try
            {
                json = File.ReadAllText(path);
            }
            catch
            {
                throw new IOException("Невозможно открыть файл");
            }
            try
            {
                data = JsonSerializer.Deserialize<AllBooks>(json, options1); // Десериализация JSON
            }
            catch
            {
                throw new FormatException("Некорректный формат данных");
            }
            Books = data.Books;
            if (Books == null)
            {
                throw new FormatException("Некорректный формат данных");
            }
        }

        private static JsonSerializerOptions EncodingSettings()
        {
            JsonSerializerOptions options1 = new()
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic) // Настройка кодировки для JSON
            };
            return options1;
        }

        public AllBooks() { } 

        /// <summary>
        /// Добавляет книгу в библиотеку.
        /// </summary>
        public void Add(Book book)
        {
            Books.Add(book); // Добавление книги в список
        }

        /// <summary>
        /// Фильтрует библиотеку по указанному полю и значениям.
        /// </summary>
        public void Filter(string field, IEnumerable<string> value)
        {
            if (field == "Name") 
            {
                Books = Books.Where(p => value.Contains(p.Name)).ToList();
            }
            if (field == "Author") 
            {
                Books = Books.Where(p => value.Contains(p.Author)).ToList();
            }
            if (field == "Genre") 
            {
                Books = Books.Where(p => value.Contains(p.Genre)).ToList();
            }
            if (field == "Date") 
            {
                Books = Books.Where(p => value.Contains(p.Date.ToString())).ToList();
            }
            if (field == "Grade") 
            {
                Books = Books.Where(p => value.Contains(p.Grade.ToString())).ToList();
            }
        }

        /// <summary>
        /// Сортирует библиотеку по указанному полю.
        /// </summary>
        public void Sort(string field, bool ascending)
        {
            if (field == "Name") 
            {
                Books = [.. Books.OrderBy(p => p.Name)];
            }
            if (field == "Author") 
            {
                Books = [.. Books.OrderBy(p => p.Author)];
            }
            if (field == "Genre") 
            {
                Books = [.. Books.OrderBy(p => p.Genre)];
            }
            if (field == "Date") 
            {
                Books = [.. Books.OrderBy(p => p.Date)];
            }
            if (field == "Grade") 
            {
                Books = [.. Books.OrderBy(p => p.Grade)];
            }
            if (!ascending) // Реверс списка для сортировки по убыванию
            {
                Books.Reverse();
            }
        }

        /// <summary>
        /// Загружает информацию о книгах из интернета.
        /// </summary>
        public void Fetch()
        {
            for (int i = 0; i < Books.Count; i++) 
            {
                try
                {
                    Books[i].Fetch(); // Загрузка информации для каждой книги
                }
                catch { } 
            }
        }

        /// <summary>
        /// Возвращает строковое представление библиотеки.
        /// </summary>
        public override string ToString()
        {
            string text = "";
            foreach (Book book in Books) 
            {
                text += book.ToString(); 
                text += "---------------------\n"; 
            }
            return text; 
        }
    }
}