using System.Collections.Generic;

namespace WpfApp1.Models
{
    public class Test
    {
        static int _LastId = 0;
        public static int LastId {get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public int CountQuestions { get; set; }
        public List<Question> Questions { get; set; }
    }
}
