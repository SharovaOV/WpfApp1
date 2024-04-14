using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using WpfApp1.Models;
using WpfApp1.Resources;

namespace WpfApp1.Services.JSON
{
    public static class JsonExtension
    {
        public static Answer Parser(this Answer answer, JsonElement root)
        {
            answer.Id = root.GetProperty("Id").GetString();
            answer.Value = root.GetProperty("Value").GetString();
            answer.Right = root.GetProperty("Right").GetBoolean();

            return answer;
        }

        public static Question Parser(this Question question, JsonElement root)
        {
            try
            {
                question.Id = root.GetProperty("Id").GetString();
                question.Type = (TypeQuest)root.GetProperty("Type").GetInt32();
                question.Value = root.GetProperty("Value").GetString();
                question.MultyAnswer = root.GetProperty("MultyAnswer").GetBoolean();
                question.TypeAnswer = (TypeAnswer)root.GetProperty("TypeAnswer").GetInt32();
                question.Answers = root.GetProperty("Answers").EnumerateArray().Skip(1).Select(x => (new Answer()).Parser(x)).ToList();
                return question;
            }
            catch
            {
                return question;
            }
               
        }

        public static Test Parser(this Test test, JsonElement root)
        {
            try
            {
                test.Id = root.GetProperty("Id").GetString();
                test.Name = root.GetProperty("Name").GetString();
                test.Questions = root.GetProperty("Questions").EnumerateArray().Select(x => (new Question()).Parser(x)).ToList();
                test.CountQuestions = test.Questions.Count();
                Test.LastId = Math.Max(root.GetProperty("Id").GetInt32() , Test.LastId);
                return test;
            }
            catch
            {
                return test;
            }
        }
        public static Test TestInfoParser(this Test test, JsonElement root)
        {
            try
            {
                test.Id = root.GetProperty("Id").GetString();
                test.Name = root.GetProperty("Name").GetString();
                test.CountQuestions = root.GetProperty("Questions").EnumerateArray().Count();

                return test;
            }
            catch
            {
                return test;
            }
        }

        public static  List<Test> Short(this List<Test> tests)
        {
            tests.ForEach(x => x.Questions = null);
            return tests;
        }

    }
}
