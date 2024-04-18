using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Resources;
using WpfApp1.Models;
using WpfApp1.Infrastucture;
using WpfApp1.Services.JSON;
using System.Windows.Input;
using WpfApp1.Views.Pages;
using System.Windows;
namespace WpfApp1.ViewModels
{
    internal class QuestionEditViewModel : ViewModelBase
    {
        #region Передаваемые данные

        
        public static DependencyProperty ValueProperty = DependencyProperty.Register(
           nameof(ValueQuest),
           typeof(string),
           typeof(EditTest),
           new PropertyMetadata(null));
        public string ValueQuest { get; set; }
        

        public static DependencyProperty TypeAnswersQProperty = DependencyProperty.Register(
           nameof(AnswerType),
           typeof(TypeAnswer),
           typeof(EditTest),
           new PropertyMetadata(null));
        public TypeAnswer AnswerType { get; set; }


        public static DependencyProperty TitleProperty = DependencyProperty.Register(
           nameof(Title),
           typeof(string),
           typeof(EditTest),
           new PropertyMetadata(null));
        public string Title { get; set; }
        #endregion

        #region Команды
        #region SetTypeAnswersCommand
        public ICommand SetTypeAnswersCommand { get; }
        private bool CanSetTypeAnswersCommandExecute(object p) => true;
        private void OnSetTypeAnswersCommandExecuted(object p)
        {
            if (p == null) return;
            var el = (EnumLabel)p;
            if(el.Check)
                AnswerType = (TypeAnswer)el.Value;
        }
        #endregion

        #endregion

        #region TypeAnswers : List<EnumLabel>  - Вид ответа
        private List<EnumLabel> _TypeAnswers;
        /// <summary>Вид ответа</summary>
        public List<EnumLabel> TypeAnswers
        {
            get => _TypeAnswers;
            set => Set(ref _TypeAnswers, value);
        }
        #endregion
        public QuestionEditViewModel()
        {
            SetTypeAnswersCommand = new RelayCommand(OnSetTypeAnswersCommandExecuted, CanSetTypeAnswersCommandExecute);
            

        }
    }
}
