using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using WpfApp1.Models;
using System.Text.Unicode;
using System.Text.Encodings.Web;


namespace WpfApp1.Services.JSON
{
    public static class JSON
    {
        static JsonSerializerOptions options = new()
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            WriteIndented = true
        };

    static string JsonPath { get; set; }
        public static void SetJsonPath(string jsonPath = "testDB2.json")
        {
            JsonPath = jsonPath;
            if (!File.Exists(jsonPath))
                SaveDB(new());
            
        }
        public static void SaveDB(List<Test> tests)
        {
            string jsonDb = JsonSerializer.Serialize(tests, options);
            File.WriteAllText(JsonPath, jsonDb, Encoding.UTF8);
        }

        public static IList<Test> LoadFullTestList()
        {
            IList<Test> Tests;

            using (JsonDocument document = JsonDocument.Parse(File.ReadAllText(JsonPath, System.Text.Encoding.UTF8)))
            {
                Tests = document.RootElement.EnumerateArray()
                    .Select(x =>(new Test()).Parser(x)).ToList();
            }

            return Tests;
        }
        public static IList<Test> LoadTestInfoList()
        {            
            IList<Test> Tests;

            using (JsonDocument document = JsonDocument.Parse(File.ReadAllText(JsonPath, System.Text.Encoding.UTF8)))
            {
                Tests = document.RootElement.EnumerateArray()
                    .Select(x =>(new Test()).TestInfoParser(x)).ToList();
            }

            return Tests;
        }

        public static Test LoadTest(string id)
        {
            Test test;

            using (JsonDocument document = JsonDocument.Parse(File.ReadAllText(JsonPath, System.Text.Encoding.UTF8)))
            {
                test = document.RootElement.EnumerateArray()
                    .Where(x => x.GetProperty("Id").GetString() ==  id)
                    .Select(x =>(new Test()).Parser(x))
                    .Single();
            }

            return test;
        }

               
        public static IList<Test> DeleteTest(Test test)
        {
            List<Test> tests = LoadFullTestList().ToList();
            tests.Remove(tests.Where(x=>x.Id == test.Id).Single());
            SaveDB(tests);
            
            return tests.Short();

        }       

        public static IList<Test> AddTest(string str)
        {
            List<Test> tests = new(LoadFullTestList());

            tests.Add(new Test());

            tests.Last().Name = string.IsNullOrWhiteSpace(str) ? $"Тест {Test.LastId}" : str;
            SaveDB(tests);

            return tests.Short();
        }

        public static IList<Test> AddTest(Test test)
        {
            List<Test> tests = new(LoadFullTestList());

            tests.Add(test);
            SaveDB(tests);

            return tests.Short();
        }
        public static void UpdateTest(Test test)
        {
            List<Test> tests = new(LoadFullTestList());

            for(int i=0;i<tests.Count; i++)
            {
                if (tests[i].Id == test.Id)
                {
                    tests[i] = test;
                }
            }
            SaveDB(tests);
        }

    }
}
