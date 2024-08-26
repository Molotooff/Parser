using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Class
{
    abstract class Reader
    {
        public abstract string ReadFile();
        public abstract string GetKey(string inputKey);
        public abstract string GetValue(string key);
    }
}
