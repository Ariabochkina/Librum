using Library;
using Spectre.Console; 
using System.Text.Encodings.Web; 
using System.Text.Json; 
using System.Text.Unicode; 


namespace Files
{
    /// <summary>
    /// Класс, который предоставляет метод для экспорта библиотеки в файл в формате JSON.
    /// </summary>
    public static class JSON
    {
        /// <summary>
        /// Метод, который экспортирует библиотеку в файл в формате JSON.
        /// Если файл не существует, то спрашивает, нужно ли его создать.
        /// </summary>
        /// <param name="path">Путь до файла, в который будет производиться экспорт</param>
        /// <param name="books">Библиотека, которую нужно экспортировать</param>
        public static void ExportJson(string path, AllBooks books)
        {
            if (!File.Exists(path))
            {
                bool answer = AnsiConsole.Prompt(new ConfirmationPrompt("Создать новый файл?"));
                if (answer) 
                {
                    WriteJson(path, books);
                }
            }
            else 
            {
                WriteJson(path, books);
            }
        }
        /// <summary>
        /// Метод, который записывает библиотеку в файл в формате JSON.
        /// </summary>
        /// <param name="path">Путь до файла, в который будет производиться запись</param>
        /// <param name="books">Библиотека, которую нужно записать</param>
        private static void WriteJson(string path, AllBooks books)
        {
            try
            {
                JsonSerializerOptions options1 = EncodingSettings();
                string json = JsonSerializer.Serialize(books, options1); // Сериализация библиотеки в JSON
                File.WriteAllText(path, json); // Запись JSON в файл
            }
            catch
            {

                string error = AnsiConsole.Prompt(new TextPrompt<string>($"[lightpink1]Не удалось записать в файл" +
            $"\nВведите любую букву чтобы продолжить[/]"));
                Console.WriteLine(error);
            }
        }

        private static JsonSerializerOptions EncodingSettings()
        {
            JsonSerializerOptions options1 = new()
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic), // Настройка кодировки для поддержки кириллицы и латиницы
                WriteIndented = true // Форматирование JSON с отступами
            };
            return options1;
        }
    }
}