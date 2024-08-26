using System;
using System.IO;

namespace Parser.Class
{
    class HTMLR : Reader
    {
        private string _path;
        private string _htmlDoc;
        private string _key;

        public HTMLR(string inputPath)
        {
            _path = inputPath;
        }

        public override string ReadFile()
        {
            _htmlDoc = File.ReadAllText(_path);
            return _htmlDoc.ToString();
        }

        public override string GetKey(string inputKey)
        {
            _key = inputKey;
            return _key;
        }

        public override string GetValue(string key)
        {
            string value = _htmlDoc;

            if (_htmlDoc == null)
            {
                throw new InvalidOperationException("HTML-файл не был загружен.");
            }

            string startTag = $"<{key}>";
            string endTag = $"</{key}>";

            int startIndex = _htmlDoc.IndexOf(startTag);

            if (startIndex == -1)
            {
                throw new ArgumentException($"Тег <{key}> не найден в HTML-документе.");
            }

            startIndex += startTag.Length;

            int endIndex = _htmlDoc.IndexOf(endTag, startIndex);
            if (endIndex == -1)
            {
                throw new ArgumentException($"Конечный тег </{key}> не найден в HTML-документе.");
            }

            value = _htmlDoc.Substring(startIndex, endIndex - startIndex);

            return value.Trim();

        }
    }
}
