using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace Project4_Library
{
    internal class Recommendation
    {
        private Books _books = new Books();
        private Dictionary<string, double> _gradeAuthors = new Dictionary<string, double>();
        private Dictionary<string, double> _gradeGenres = new Dictionary<string, double>();
        public Recommendation(Books books)
        {
            _books = books;
            List<string> authors = (from p in books.books select p.Author).Distinct().ToList();
            foreach (string author in authors)
            {
                double grade = 0;
                IEnumerable<int> grades = (from p in books.books where p.Author == author && p.Grade != -1 select p.Grade);
                if (grades.Count() > 0)
                {
                    grade = grades.Average();
                }
                
                _gradeAuthors.Add(author, grade);
            }
            List<string> genres = (from p in books.books select p.Genre).Distinct().ToList();
            foreach (string genre in genres)
            {
                double grade = 0;
                IEnumerable<int> grades = (from p in books.books where p.Genre == genre && p.Grade != -1 select p.Grade);
                if (grades.Count() > 0) 
                { 
                    grade = grades.Average(); 
                }
                _gradeGenres.Add(genre, grade);
            }
            List<Book> sortBooks = (from p in _books.books orderby AverageGrade(p) descending where p.Grade == -1 select p).ToList();
            sortBooks = sortBooks.Take(sortBooks.Count >= 5 ? 5 : sortBooks.Count).ToList();
            Books books1 = new Books { books = sortBooks };
            Console.Write(books1);
        }
        public double AverageGrade(Book book)
        {
            double avarageGrade = (from p in _books.books
                                   where p == book
                                   select _gradeAuthors[p.Author] + _gradeGenres[p.Genre]).ToList()[0];
            return avarageGrade;
        }
    }
}
