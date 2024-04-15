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
        public EnumLabel() { }
        public EnumLabel(int value, string label)
        {
            Value = value;
            Label = label;
        }
    }
}
