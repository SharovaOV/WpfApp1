using System;
using System.Collections.Generic;
using WpfApp1.Resources;
using System.Text.Json;
using System.Linq;
using WpfApp1.Resources;

namespace WpfApp1.Models
{
    public class Question
    {
        public string Id { get; set; }
        public TypeQuest Type { get; set; } = TypeQuest.PickValue;
        public string Value { get; set; }
        public bool MultyAnswer { get; set; }
        public TypeAnswer TypeAnswer { get; set; } = TypeAnswer.Text;
        public List<Answer> Answers { get; set; }
        public Question()
        {
            Answers = new();
        }
    }
    public class UserQuestion
    {
        public string Id { get; set; }
        public int Index { get; set; }
        public TypeQuest Type { get; set; } = TypeQuest.PickValue;
        public string Value { get; set; }
        public bool MultyAnswer { get; set; }
        public bool IsActive { get; set; } = false;
        public TypeAnswer TypeAnswer { get; set; } = TypeAnswer.Text;
        public bool IsRight { get{ 
                if(TypeAnswer != TypeAnswer.Strings)
                    return Answers.Count == Answers.Count(x => x.IsRight == true);
                return AnswerStrings.Contains(UserString.ToAnswerString());
            } }
        public List<UserAnswer> Answers { get; set; }
        public HashSet<string> AnswerStrings { get; set; }
        public string UserString { get; set; } = "";
        public UserQuestion(Question question, int index=0)
        {
            Id = question.Id;
            Index = index + 1;
            Type = question.Type;
            Value = question.Value;
            MultyAnswer = question.MultyAnswer;
            TypeAnswer = question.TypeAnswer;
            Answers = question.Answers.Select(x => new UserAnswer(x)).ToList();
            if(TypeAnswer == TypeAnswer.Strings)
            {
                AnswerStrings = new(Answers.Select(x => x.Value.ToAnswerString()).ToList());
            }
        }
    }

   
}
