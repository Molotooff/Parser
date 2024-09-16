using System;
using System.IO;

namespace Parser.Class
{
    class  JR : Reader
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

            string value = _jsonText;
            string answer = string.Empty;
            int symbolCount = 1;
            value = value.TrimStart('{').TrimEnd('}').Trim();

            int indexOfChar = value.IndexOf(key);
            value = value.Substring(indexOfChar + key.Length + 1);

            foreach (char symbol in value)
            {
                switch (symbol)
                {
                    case '"':
                        value = value.Substring(value.IndexOf(symbol) + 1);
                        foreach (char symbol2 in value)
                        {
                            if (symbol2 == '"')
                                return answer;
                            answer += symbol2;
                        }
                        break;
                    case '{':
                        value = value.Substring(value.IndexOf(symbol) + 1);
                        foreach (char symbol3 in value)
                        {
                            if (symbol3 == '{')
                                symbolCount++;
                            if (symbol3 == '}')
                                symbolCount--;
                            if (symbolCount == 0)
                                return answer;
                            answer += symbol3;
                        }
                        break;
                    case '[':
                        value = value.Substring(value.IndexOf(symbol) + 1);
                        foreach (char symbol4 in value)
                        {
                            if (symbol4 == '[')
                                symbolCount++;
                            if (symbol4 == ']')
                                symbolCount--;
                            if (symbolCount == 0)
                                return answer;
                            answer += symbol4;
                        }
                        break;
                    case char digit when Char.IsDigit(digit):
                        value = value.Substring(value.IndexOf(symbol));
                        foreach (char digit2 in value)
                        {
                            if (digit2 == ',')
                                return answer;
                            answer += digit2;
                        }
                        break;
                }
            }
            return answer;
        }
    }
}
