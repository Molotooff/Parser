using System;
using System.IO;
using Newtonsoft.Json.Linq;
using HtmlAgilityPack;
using System.Xml.Linq;

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
                reader = new XmlReader(filePath);
            }
            else if (extension == ".html" || extension == ".htm")
            {
                reader = new HtmlReader(filePath);
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

    abstract class Reader
    {
        public abstract string ReadFile();
        public abstract string GetKey(string inputKey);
        public abstract string GetValue(string key);
    }

    class JR : Reader
    {
        private string _path;
        private string _jsonText;
        private string _key;

        public JR(string inputPath)
        {
            _path = inputPath;
        }

        public override string ReadFile()
        {
            _jsonText = File.ReadAllText(_path);
            return _jsonText;
        }

        public override string GetKey(string inputKey)
        {
            _key = inputKey;
            return _key;
        }

        public override string GetValue(string key)
        {
            if (string.IsNullOrEmpty(_jsonText))
            {
                throw new InvalidOperationException("JSON-файл не был загружен.");
            }

            JObject jsonObject = JObject.Parse(_jsonText);
            JToken value = jsonObject.SelectToken(key);

            return value?.ToString() ?? $"Ключ '{key}' не найден.";
        }
    }

    class XmlReader : Reader
    {
        private string _path;
        private XDocument _xmlDoc;

        public XmlReader(string inputPath)
        {
            _path = inputPath;
        }

        public override string ReadFile()
        {
            _xmlDoc = XDocument.Load(_path);
            return _xmlDoc.ToString();
        }

        public override string GetKey(string inputKey)
        {
            return inputKey;
        }

        public override string GetValue(string key)
        {
            if (_xmlDoc == null)
            {
                throw new InvalidOperationException("XML-файл не был загружен.");
            }

            XElement element = _xmlDoc.Root.Element(key);
            return element != null ? element.Value : $"Ключ '{key}' не найден.";
        }
    }

    class HtmlReader : Reader
    {
        private string _path;
        private HtmlDocument _htmlDoc;

        public HtmlReader(string inputPath)
        {
            _path = inputPath;
        }

        public override string ReadFile()
        {
            _htmlDoc = new HtmlDocument();
            _htmlDoc.Load(_path);
            return _htmlDoc.DocumentNode.OuterHtml;
        }

        public override string GetKey(string inputKey)
        {
            return inputKey;
        }

        public override string GetValue(string key)
        {
            if (_htmlDoc == null)
            {
                throw new InvalidOperationException("HTML-файл не был загружен.");
            }

            var node = _htmlDoc.DocumentNode.SelectSingleNode($"//*[@id='{key}']") ??
                       _htmlDoc.DocumentNode.SelectSingleNode($"//*[@name='{key}']") ??
                       _htmlDoc.DocumentNode.SelectSingleNode($"//{key}");

            return node != null ? node.InnerText : $"Ключ '{key}' не найден.";
        }
    }
}
