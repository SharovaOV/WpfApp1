using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Resources
{
    public static class Extentions
    {
        public static string ToAnswerString(this string str)
        {
            return string.Join("", str.Split(new char[] { '.', ' ' })).ToLower();
        }
    }
}
