using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public class EnumLabel
    {
        public int Value { get; set; }
        public string Label { get; set; }
        public bool Check { get; set; } = false;
        public EnumLabel() { }
        public EnumLabel(int value, string label, bool check = false)
        {
            Value = value;
            Label = label;
            Check = check;
        }
    }
}
