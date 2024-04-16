using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Models;

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
        Strings
    }

    public static class EnumView
    {
        static string[] _TypeQuest = { "Выбор варианта", "Вписать ответ" };
        public static string[] TypeQuest { get => _TypeQuest; }
        public static string GetTypeQuest(TypeQuest typeQuest)
        {
            return TypeQuest[(int)typeQuest];
        }

        static string[] _TypeAnswer = { "Текстовое представление ", "Представление изображением", "Набор правильного написанных ответов" };
        public static string[] TypeAnswer { get => _TypeAnswer; }
        public static string GetTypeAnswer(TypeAnswer typeAnswer)
        {
            return TypeAnswer[(int)typeAnswer];
        }

        public static List<EnumLabel>EnumLabels(string[] labels, int check)
        {
            List<EnumLabel> enumLabels = new();
            for(int i=0; i< labels.Length; i++)
            {
                enumLabels.Add(new(i, labels[i], check == i));
            }

            return enumLabels;
        }

        
    }
}
