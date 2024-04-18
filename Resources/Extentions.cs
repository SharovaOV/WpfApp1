using System;
using System.IO;
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
            str = string.Join(",", str.Split(new char[] { '.' }));
            return string.Join("", str.Split(new char[] {'!', ' ','\t','\n','\r' })).ToLower();
        }
        public static string ToBaseDirectory(this string path, string dopPath = "")
        {
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path)) return path;
            string result="";
            string ToBaseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            if (!string.IsNullOrWhiteSpace(dopPath)) { 
                if (!Directory.Exists(ToBaseDirectory + dopPath))
                {
                    Directory.CreateDirectory(ToBaseDirectory + dopPath);
                }

                result = dopPath + "\\";
            }
            result +=  Path.GetFileName(path);
           
            if (path != result && path != AppDomain.CurrentDomain.BaseDirectory + result && File.Exists(path))
            {
                File.Copy(path, result, true);
            }

            return result;
        }
        public static string ToFullPath(this string path, string dopPath = "")
        {
            string ToBaseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            if (!string.IsNullOrWhiteSpace(dopPath)) {
                ToBaseDirectory += dopPath + "\\";
            }


            return ToBaseDirectory + Path.GetFileName(path);
        }
    }
}
