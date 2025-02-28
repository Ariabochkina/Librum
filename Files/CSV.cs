using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library;
using ServiceStack.Text;
using Spectre.Console;
using static System.Reflection.Metadata.BlobBuilder;

namespace Files
{
    public static class CSV
    {
        public static Books ImportCSV(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Нет файла");
            }
            string csv;
            try
            {
                csv = File.ReadAllText(path);

            }
            catch
            {
                throw new IOException("Не удалось прочитать из файла");
            }
            try
            {
                Books books = new Books { books = CsvSerializer.DeserializeFromString<List<Book>>(csv) };
                return books;
            }
            catch
            {
                throw new FormatException("Неверный формат CSV");
            }
        }
        public static void ExportCSV(string path, Books books)
        {
            if (!File.Exists(path))
            {
                bool answer = AnsiConsole.Prompt(new ConfirmationPrompt("Создать новый файл?"));
                if (answer)
                {
                    try
                    {
                        File.WriteAllText(path, CsvSerializer.SerializeToCsv(books.books));
                    }
                    catch
                    {
                        throw new IOException("Не удалось записать");
                    }

                }
            }
            else
            {
                try
                {
                    File.WriteAllText(path, CsvSerializer.SerializeToCsv(books.books));
                }
                catch
                {
                    throw new IOException("Не удалось записать");
                }
            }
        }
    }
}
