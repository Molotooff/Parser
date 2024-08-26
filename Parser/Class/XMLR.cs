using System;
using System.IO;

namespace Parser.Class
{
    class XMLR : Reader
    {
        private string _path;
        private string _xmlDoc;
        private string _key;

        public XMLR(string inputPath)
        {
            _path = inputPath;
        }

        public override string ReadFile()
        {
            _xmlDoc = File.ReadAllText(_path);
            return _xmlDoc.ToString();
        }

        public override string GetKey(string inputKey)
        {
            _key = inputKey;
            return _key;
        }

        public override string GetValue(string key)
        {
            string value = _xmlDoc;
            if (_xmlDoc == null)
            {
                throw new InvalidOperationException("XML-файл не был загружен.");
            }

            int indexOfChar = value.IndexOf(key);
            value = value.Substring(indexOfChar + key.Length + 1);
            indexOfChar = value.IndexOf($"/{key}");
            value = value.Substring(0, indexOfChar - 1);
            return value;

        }
    }
}
