using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace Library
{
    public class Books
    {
        public List<Book> books { get; set; } = null;
        public Books(string path) {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Нет такого пути");
            }
            Books data = new Books();
            string json;
            var options1 = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic)
            };
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
                data = JsonSerializer.Deserialize<Books>(json, options1);
            }
            catch
            {
                throw new FormatException("Некорректный формат данных");
            }
            books = data.books;
        }
        public Books() { }

        public void Add(Book book)
        {
            books.Add(book);
        }
        public void Remove(Book book)
        {
            books.Remove(book);
        }
        public void Filter(string field, IEnumerable<string> value)
        {
            if (field == "Name")
            {
                books = books.Where(p => value.Contains(p.Name)).ToList();
            }
            if (field == "Author")
            {
                books = books.Where(p => value.Contains(p.Author)).ToList();
            }
            if (field == "Genre")
            {
                books = books.Where(p => value.Contains(p.Genre)).ToList();
            }
            if (field == "Date")
            {
                books = books.Where(p => value.Contains(p.Date.ToString())).ToList();
            }
            if (field == "Grade")
            {
                books = books.Where(p => value.Contains(p.Grade.ToString())).ToList();
            }
        }
        public void Sort(string field, bool ascending)
        {
            if (field == "Name")
            {
                books = books.OrderBy(p => p.Name).ToList();
            }
            if (field == "Author")
            {
                books = books.OrderBy(p => p.Author).ToList();
            }
            if (field == "Genre")
            {
                books = books.OrderBy(p => p.Genre).ToList();
            }
            if (field == "Date")
            {
                books = books.OrderBy(p => p.Date).ToList();
            }
            if (field == "Grade")
            {
                books = books.OrderBy(p => p.Grade).ToList();
            }
            if (!ascending)
            {
                books.Reverse();
            }
        }
        public override string ToString()
        {
            string text = "";
            foreach (Book book in books) {
                text += book.ToString();
                text += "---------------------\n";
            }
            return text;
        }
    }
}
