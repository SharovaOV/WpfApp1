using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp1.Models;
using WpfApp1.Services.JSON;
using WpfApp1.Infrastucture;
using WpfApp1.Resources;

namespace WpfApp1.ViewModels
{
    class EditTestViewModel : ViewModelBase
    {
        #region Поля

        #region TestInfo : ObservableCollection<Test> - Список доступных тестов
        private ObservableCollection<Question> _Questions;
        /// <summary>Список доступных тестов</summary>
        public ObservableCollection<Question> Questions
        {
            get => _Questions;
            set => Set(ref _Questions, value);
        }
        #endregion

        #region CurrentTypeView : ViewModelBase - Вид представления ответа
        private ViewModelBase _CurrentTypeView;
        /// <summary>Вид представления ответа</summary>
        public ViewModelBase CurrentTypeView
        {
            get => _CurrentTypeView;
            set => Set(ref _CurrentTypeView, value);
        }
        #endregion

        #region SelectedTest : Test - Выделенный тест
        private Question _SelectedTest;
        /// <summary>Выделенный тест</summary>
        public Question SelectedQestion
        {
            get => _SelectedTest;
            set{ 
                Set(ref _SelectedTest, value); 

            }
        }
        #endregion

        #region Answers : ObservableCollection<Answer> - Ответы
        private ObservableCollection<Answer> _Answers;
        /// <summary>Ответы</summary>
        public ObservableCollection<Answer> Answers
        {
            get => _Answers;
            set => Set(ref _Answers, value);
        }
        #endregion

        #region SelectedAnswer : Answer - Выделенный ответ
        private Answer _SelectedAnswer;
        /// <summary>Выделенный ответ</summary>
        public Answer SelectedAnswer
        {
            get => _SelectedAnswer;
            set => Set(ref _SelectedAnswer, value);
        }
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

        #endregion


        #region Команды

        #region DeleteQuestionCommand
        /// <summary> Событие удалить вопрос </summary>
        public ICommand DeleteQuestionCommand { get; }

        private bool CanDeleteQuestionCommandExecuted(object t) => t is Question question && Questions.Contains(question);
        private void OnDeleteQuestionCommandExecuted(object t)
        {
            if (!(t is Question question)) return;
            Questions.Remove(question);
        }
        #endregion

        #region DeleteAnswerCommand
        /// <summary> Событие удалить вопрос </summary>
        public ICommand DeleteAnswerCommand{ get; }

        private bool CanDeleteAnswerCommandExecuted(object t) => t is Answer answer && Answers.Contains(answer);
        private void OnDeleteAnswerCommandExecuted(object t)
        {
            if (!(t is Answer answer)) return;
            Answers.Remove(answer);
        }
        #endregion

        #endregion
        public EditTestViewModel()
        {
            #region Команды
            DeleteQuestionCommand = new RelayCommand(OnDeleteQuestionCommandExecuted, CanDeleteQuestionCommandExecuted);
            DeleteAnswerCommand = new RelayCommand(OnDeleteAnswerCommandExecuted, CanDeleteAnswerCommandExecuted);
            #endregion

            Questions = new(JSON.LoadTest("1").Questions);
            SelectedQestion = Questions.First();
            Answers = new(SelectedQestion.Answers);
            SelectedAnswer = Answers.First();
            TypeAnswers = EnumView.EnumLabels(EnumView.TypeAnswer);
        }
    }
}
