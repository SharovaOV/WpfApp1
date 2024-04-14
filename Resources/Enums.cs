using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Resources
{
    public enum TypeQuest
    {
        PickValue = 0,
        WriteValue
    }
    public enum TypeAnswer
    {
        Text = 0,
        Image,
        String
    }

    public static class EnumLable
    {
        static string[] _TypeQuest = { "Выбор варианта", "Вписать ответ" };
        public static string[] TypeQuest { get => _TypeQuest; }
        public static string GetTypeQuest(TypeQuest typeQuest)
        {
            return TypeQuest[(int)typeQuest];
        }

        static string[] _TypeAnswer = { " Текстовое представление ", "Представление изображением", "Набор правильного написанных ответов" };
        public static string[] TypeAnswer { get => _TypeAnswer; }
        public static string GetTypeAnswer(TypeAnswer typeAnswer)
        {
            return TypeAnswer[(int)typeAnswer];
        }

        
    }
}
