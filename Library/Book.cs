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
            set {
                string[] options = ["Фантастика", "Детектив", "Роман", "История", "Научная литература"];
                if (options.Contains(value))
                { 
                    _genre = value; 
                } 
                else
                {
                    throw new ArgumentException("Нет такого жанра");
                }
            } 
        }
        private int _date;
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
