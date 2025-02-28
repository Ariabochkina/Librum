using Library;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace Files
{
    public static class JSON
    {
        public static void ExportJson(string path, Books books)
        {
            if (!File.Exists(path))
            {
                bool answer = AnsiConsole.Prompt(new ConfirmationPrompt("Создать новый файл?"));
                if (answer)
                {
                    try
                    {
                        var options1 = new JsonSerializerOptions
                        {
                            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                            WriteIndented = true
                        };
                        string json = JsonSerializer.Serialize(books, options1);
                        File.WriteAllText(path, json);
                    }
                    catch
                    {
                        string error = AnsiConsole.Prompt(new TextPrompt<string>($"[lightpink1]Не удалось записать в файл" +
                    $"\nВведите любую букву чтобы продолжить[/]"));
                        Console.WriteLine(error);

                    }
                }
            }
            else
            {
                try
                {
                    var options1 = new JsonSerializerOptions
                    {
                        Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                        WriteIndented = true
                    };
                    string json = JsonSerializer.Serialize(books, options1);
                    File.WriteAllText(path, json);
                }
                catch
                {
                    string error = AnsiConsole.Prompt(new TextPrompt<string>($"[lightpink1]Не удалось записать в файл" +
                $"\nВведите любую букву чтобы продолжить[/]"));
                    Console.WriteLine(error);

                }
            }
        }
    }
}
