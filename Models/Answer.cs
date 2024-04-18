using System.Text.Json.Serialization;
using WpfApp1.Resources;
using Params = WpfApp1.Properties.Settings;
namespace WpfApp1.Models
{
    public class Answer
    {
        public string Id { get; set; }
        public string Value { get; set; }
        public bool Right { get; set; }
        [JsonIgnore]
        public string FullPath { get => Value.ToFullPath(Params.Default.ImagePath); }
    }

   public class UserAnswer
    {
        public string Id { get; set; }
        public string Value { get; set;}
        public bool Right { get; set; }
        public bool UserChecked { get; set; } = false;
        public bool IsRight { get => Right == UserChecked; }
        public string FullPath { get => Value.ToFullPath(Params.Default.ImagePath); }
        public UserAnswer(Answer answer)
        {
            Id = answer.Id;
            Value = answer.Value;
            Right = answer.Right;   
        } 
        
        

    }
}
