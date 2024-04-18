using System.Collections.Generic;
using WpfApp1.Resources;
namespace WpfApp1.Models
{
    public class Answer
    {
        public string Id { get; set; }
        public string Value { get; set; }
        public bool Right { get; set; }
    }

   public class UserAnswer
    {
        public string Id { get; set; }
        public string Value { get; set;}
        public bool Right { get; set; }
        public bool UserChecked { get; set; } = false;
        public bool IsRight { get => Right == UserChecked; }
        public UserAnswer(Answer answer)
        {
            Id = answer.Id;
            Value = answer.Value;
            Right = answer.Right;   
        } 
        
        

    }
}
