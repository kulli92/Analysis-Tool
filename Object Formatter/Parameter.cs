using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Object_Formatter
{
    public class Parameter
    {
        public int Index { get; set; }
        public string ParamName { get; set; }
        public string MemoryAddress { get; set; }
        public string MinValue { get; set; }
        public string MaxValue { get; set; }
        public string Value { get; set; } = "";
    }
}
