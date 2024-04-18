using System.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Models;
using WpfApp1.Views.Windows;
using WpfApp1.ViewModels;
using WpfApp1.Resources;
using WpfApp1.Views.Pages;
using WpfApp1.Views.Elements;
using Params = WpfApp1.Properties.Settings;

namespace WpfApp1.Services
{
    class WindowUserDialogService : IUserDialogService
    {
        public bool Confirm(string Message, string Caption, bool Exclamation = false)
        {
          return  MessageBox.Show(
                Message,
                Caption,
                MessageBoxButton.YesNo,
                Exclamation ? MessageBoxImage.Exclamation : MessageBoxImage.Question) == MessageBoxResult.Yes;
        }
        
        public bool Edit(object item, string title="", int type =-1)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));

            switch(item)
            {
                case Question question:
                    return EditQuestion(question, title);
                case Test test:
                    return EditTest(test, title);
                case Answer answer:
                    return EditAnswer(answer, title, type);
                default:
                    throw new NotSupportedException($"Редактирование объукта типа {item.GetType().Name} не поддерживается.");
            }
        }

        public void ShowError(string Message, string Caption) => MessageBox.Show(Message, Caption, MessageBoxButton.OK, MessageBoxImage.Error);

        public bool ShowInformation(string Information, string Caption) => MessageBox.Show(Information, Caption, MessageBoxButton.OK, MessageBoxImage.Information) == MessageBoxResult.OK;

        public void ShowWorning(string Message, string Caption) => MessageBox.Show(Message, Caption, MessageBoxButton.OK, MessageBoxImage.Warning);

        private static bool EditQuestion(Question question, string title ="")
        {
            var dataContext = new QuestionEditViewModel
            {
                ValueQuest = question.Value,
                AnswerType = question.TypeAnswer,
                Title = string.IsNullOrEmpty(title) ? "Окно редактирования вопроса" : title,
                TypeAnswers = EnumView.EnumLabels(EnumView.TypeAnswer, (int)question.TypeAnswer)
        };
            var dlg = new QuestionEdit
            {
                DataContext = dataContext
            };

            if (dlg.ShowDialog() == false) return false;

            question.Value = dataContext.ValueQuest;
            question.TypeAnswer = dataContext.AnswerType;
            return true;
        }
        private static bool EditTest(Test test, string title = "")
        {
            var dataContext = new CreateTestViewModel
            {
                NameTest = test.Name,                
                Title = string.IsNullOrEmpty(title) ? "Окно редактирования теста" : title,
            };
            var dlg = new CreateTest
            {
                DataContext = dataContext
            };

            if (dlg.ShowDialog() == false) return false;

            test.Name = dataContext.NameTest;
            return true;
        }
        private static bool EditAnswer(Answer answer, string title = "", int type=-1)
        {
            var dataContext = new AnswerEditViewModel
            {
                ValueAnswer = (type == (int)TypeAnswer.Image) ?answer.FullPath : answer.Value,
                CurrentView = (type==(int)TypeAnswer.Image) ? new QuestIMG() : new QuestText(),
                Title = string.IsNullOrEmpty(title) ? "Окно редактирования теста" : title,
            };
            var dlg = new AnswerEdit
            {
                DataContext = dataContext
            };

            if (dlg.ShowDialog() == false) return false;
            if(type != (int)TypeAnswer.Image)
                answer.Value = dataContext.ValueAnswer;
            else
                answer.Value = dataContext.ValueAnswer.ToBaseDirectory(Params.Default.ImagePath);
            return true;
        }
    }
 
}
