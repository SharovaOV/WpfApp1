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
        static JsonSerializerOptions options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            };

    static string JsonPath { get; set; }
        public static void setJsonPath(string jsonPath = "testDB2.json")
        {
            JsonPath = jsonPath;
        }
        public async static void saveDB(List<Test> tests)
        {
            string jsonDb = JsonSerializer.Serialize(tests, options);
            using (StreamWriter writer = new StreamWriter(JsonPath, false))
            {
                await writer.WriteLineAsync(jsonDb);
            }
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
            saveDB(tests);
            
            return tests.Short();

        }

        public static IList<Test> AddTest()
        {
            List<Test> tests = new(LoadFullTestList());

            tests.Add(new Test());
            tests.Last().Id = "" + (++Test.LastId);
            tests.Last().Name = $"Тест {Test.LastId}";
            saveDB(tests);

            return tests.Short();
        }

    }
}
