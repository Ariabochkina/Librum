using System.Net;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using static System.Net.WebRequestMethods;
using Spectre.Console;
namespace Library
{
    public class Book
    {
        private string _name;
        public string Name { get { return _name; } set { _name = value; } }

        private string _author;
        public string Author { get { return _author; } set { _author = value; } }
        private string _genre;
        
        public string Genre { 
            get { return _genre; } 
            set { _genre = value; } 
        }
        private int _date = 2025;
        public int Date { 
            get { return _date; }
            set {

                if (value >= 1456 && value <= DateTime.Today.Year) //В 1456 году Иоганн Гутенберг издал первую печатную книгу — Библию на латинском языке
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
        public string ISBN { 
            get { return _isbn; } 
            set {
                if (!long.TryParse(value, out _) || (value.Length != 13 && value.Length != 10))
                {
                    throw new ArgumentException("Неверный ISBN");
                }
                if (value.Length != 13)
                {
                    if (value[^1] != 'X')
                    {
                        int s = 0;
                        for (int i = 0; i < 10; i++)
                        {
                            s += (value[i] - '0') * (i + 1);
                        }
                        if (s % 11 != 0)
                        {
                            throw new ArgumentException("Неверный ISBN");
                        }
                    }
                }
                else
                {
                    int s = 0;
                    for (int i = 0; i < 13; i++)
                    {
                        if (i % 2 == 0)
                        {
                            s += 1 * (value[i] - '0');
                        } else
                        {
                            s += 3 * (value[i] - '0');
                        }
                    }
                    if (s % 10 != 0)
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
                if (value <= 10 && value >= -1)
                {
                    _grade = value;
                }
                else
                {
                    throw new ArgumentException("Некорректная оценка");
                }
            }
        }
        public string GetField(string field)
        {
            if (field == "Name")
            {
                return Name;
            }
            if (field == "Author")
            {
                return Author;
            }
            if (field == "Genre")
            {
                return Genre;
            }
            if (field == "Date")
            {
                return Date.ToString();
            }
            return Grade.ToString();
        }
        private  JsonElement GetJson()
        {
            string url = $"https://openlibrary.org/api/books?bibkeys=ISBN:{ISBN}&jscmd=data&format=json";
            var client =  new WebClient();
            string response =  client.DownloadString(url);
            JsonElement root;
            try
            {
                JsonDocument _data = JsonDocument.Parse(response);
                root = _data.RootElement;
            }
            catch
            {
                throw new FormatException();
            }
            return root.GetProperty($"ISBN:{_isbn}");
        }
        public  void Fetch()
        {
            try
            {
                JsonElement root =  GetJson();
                try
                {
                    Name = root.GetProperty("title").GetString();
                }
                catch { }
                try
                {
                    if (root.GetProperty("publish_date").GetString().Length > 4)
                    {
                        if (int.TryParse(root.GetProperty("publish_date").GetString()[^4..], out _))
                        {
                            Date = int.Parse(root.GetProperty("publish_date").GetString()[^4..]);
                        }
                        else if (int.TryParse(root.GetProperty("publish_date").GetString()[0..4], out _))
                        {
                            Date = int.Parse(root.GetProperty("publish_date").GetString()[0..4]);
                        }
                    }
                    Date = int.Parse(root.GetProperty("publish_date").GetString());
                }
                catch { }
                try
                {
                    List<JsonElement> authors = root.GetProperty("authors").EnumerateArray().ToList();
                    Author = authors[0].GetProperty("name").ToString();
                }
                catch { }
                try
                {
                    List<JsonElement> genres = root.GetProperty("subjects").EnumerateArray().ToList();
                    Genre = genres[0].GetProperty("name").ToString();
                }
                catch { }
                

            }
            catch
            {
                throw new HttpRequestException();
            }

        }
        public void GetCover()
        {
            JsonElement root =  GetJson();
            using (WebClient client = new WebClient())
            {
                try
                {
                    string url = root.GetProperty("cover").GetProperty("large").GetString();
                    client.DownloadFile(new Uri(url), $@"..\..\..\..\{_isbn}.png");
                    AnsiConsole.Write($"Путь от корня проекта: {_isbn}.png\n");
                    AnsiConsole.Write(new CanvasImage($@"..\..\..\..\{_isbn}.png"));
                }
                catch
                {
                    throw new HttpRequestException();
                }
            }
        }
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
