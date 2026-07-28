using Library;
//AI_COMMENTS
namespace Librum
{
    /// <summary>
    /// Класс, который предлагает 5 книг с наивысшей оценкой, которые еще не были оценены.
    /// </summary>
    internal class Recommendation
    {
        private readonly AllBooks _books = new(); 
        private readonly Dictionary<string, double> _gradeAuthors = []; // Словарь для хранения средней оценки авторов
        private readonly Dictionary<string, double> _gradeGenres = []; // Словарь для хранения средней оценки жанров

        /// <summary>
        /// Конструктор класса Recommendation.
        /// </summary>
        /// <param name="books">Библиотека, из которой выбираются книги</param>
        public Recommendation(AllBooks books)
        {
            _books = books; 

            // Получение уникальных авторов из библиотеки
            List<string> authors = (from p in books.Books select p.Author).Distinct().ToList();
            foreach (string author in authors) // Расчет средней оценки для каждого автора
            {
                double grade = 0;
                IEnumerable<int> grades = from p in books.Books where p.Author == author && p.Grade != -1 select p.Grade; // Оценки книг автора
                if (grades.Count() > 0) 
                {
                    grade = grades.Average(); 
                }
                _gradeAuthors.Add(author, grade); 
            }

            // Получение уникальных жанров из библиотеки
            List<string> genres = (from p in books.Books select p.Genre).Distinct().ToList();
            foreach (string genre in genres) // Расчет средней оценки для каждого жанра
            {
                double grade = 0;
                IEnumerable<int> grades = from p in books.Books where p.Genre == genre && p.Grade != -1 select p.Grade; // Оценки книг жанра
                if (grades.Count() > 0) 
                {
                    grade = grades.Average(); 
                }
                _gradeGenres.Add(genre, grade);
            }

            // Сортировка книг по средней оценке (автор + жанр) и выбор 5 лучших неоцененных книг
            List<Book> sortBooks = (from p in _books.Books orderby AverageGrade(p) descending where p.Grade == -1 select p).ToList();
            sortBooks = sortBooks.Take(sortBooks.Count >= 5 ? 5 : sortBooks.Count).ToList(); // Ограничение до 5 книг

            AllBooks books1 = new() { Books = sortBooks };
            Console.Write(books1);
        }

        /// <summary>
        /// Метод, который возвращает среднюю оценку книги на основе суммарного рейтинга по автору и жанру.
        /// </summary>
        /// <param name="book">Книга, для которой нужно найти оценку</param>
        /// <returns>Средняя оценка книги</returns>
        public double AverageGrade(Book book)
        {
            // Расчет средней оценки как суммы средней оценки автора и средней оценки жанра
            double avarageGrade = (from p in _books.Books
                                   where p == book
                                   select _gradeAuthors[p.Author] + _gradeGenres[p.Genre]).ToList()[0];
            return avarageGrade;
        }
    }
}