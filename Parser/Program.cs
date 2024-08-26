using Parser.Class;
using System;
using System.IO;

namespace Parser
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Введите путь до файла: ");
            string filePath = Console.ReadLine();

            Reader reader = null;

            string extension = Path.GetExtension(filePath).ToLower();
            if (extension == ".json")
            {
                reader = new JR(filePath);
            }
            else if (extension == ".xml")
            {
                reader = new XMLR(filePath);
            }
            else if (extension == ".html" || extension == ".htm")
            {
                reader = new HTMLR(filePath);
            }
            else
            {
                Console.WriteLine("Файл должен быть с расширением .json, .xml или .html");
                return;
            }

            reader.ReadFile();
            Console.WriteLine("Введите ключ для поиска значения:");
            string key = Console.ReadLine();
            string value = reader.GetValue(key);
            Console.WriteLine($"Значение для ключа '{key}': {value}");
        }
    }
}
