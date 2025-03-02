using Library;
using ServiceStack.Text;
using Spectre.Console;
//AI_COMMENTS
namespace Files
{
    /// <summary>
    /// Класс, предоставляющий методы для импорта и экспорта информации о книгах в формате CSV.
    /// </summary>
    public static class CSV
    {
        /// <summary>
        /// Метод, который импортирует информацию о книгах из CSV файла.
        /// </summary>
        /// <param name="path">Путь до CSV файла</param>
        /// <returns>Объект, хранящий информацию о книгах</returns>
        /// <exception cref="FileNotFoundException">Если файла не существует</exception>
        /// <exception cref="IOException">Если не удалось прочитать файл</exception>
        /// <exception cref="FormatException">Если формат файла не CSV</exception>
        public static AllBooks ImportCSV(string path)
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
                AllBooks books = new() { Books = CsvSerializer.DeserializeFromString<List<Book>>(csv) }; // Десериализация CSV в список книг
                return books;
            }
            catch
            {
                throw new FormatException("Неверный формат CSV");
            }
        }

        /// <summary>
        /// Метод, который экспортирует информацию о книгах из библиотеки в файл в формате CSV.
        /// Если файл не существует, то спрашивает, нужно ли его создать.
        /// </summary>
        /// <param name="path">Путь до файла, в который будет производиться экспорт</param>
        /// <param name="books">Библиотека, которую нужно экспортировать</param>
        public static void ExportCSV(string path, AllBooks books)
        {
            if (!File.Exists(path))
            {
                bool answer = AnsiConsole.Prompt(new ConfirmationPrompt("Создать новый файл?"));
                if (answer)
                {
                    WriteCSV(path, books);
                }
            }
            else
            {
                WriteCSV(path, books);
            }
        }
        /// <summary>
        /// Метод, который записывает информацию о книгах в файл в формате CSV.
        /// </summary>
        /// <param name="path">Путь до файла, в который будет производиться запись</param>
        /// <param name="books">Библиотека, которую нужно записать</param>
        /// <exception cref="IOException">Если не удалось записать в файл</exception>
        private static void WriteCSV(string path, AllBooks books)
        {
            try
            {
                File.WriteAllText(path, CsvSerializer.SerializeToCsv(books.Books)); // Сериализация и запись в файл
            }
            catch
            {
                throw new IOException("Не удалось записать");
            }
        }
    }
}