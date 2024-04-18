using System.Collections.Generic;
using System.Linq;

namespace WpfApp1.Models
{
    public class Test
    {
        static int _LastId = 0;
        public static int LastId { get => _LastId; set => _LastId = value; } 
        public string Id { get; set; }
        public string Name { get; set; }
        public int CountQuestions { get; set; }
        public List<Question> Questions { get; set; }
        public Test()
        {
            Id = "" + (++_LastId);
            Questions = new();
        }
    }
    public class UserTest
    {

        public string Id { get; set; }
        public string Name { get; set; }
        public List<UserQuestion> Questions { get; set; }
        public int RightCount { get => Questions.Count(x => x.IsRight == true); }
        public UserTest(Test test){
            Id = test.Id;
            Name = test.Name;
            Questions = test.Questions.Select((x, index) => new UserQuestion(x, index)).ToList(); 
        }
    }
}
