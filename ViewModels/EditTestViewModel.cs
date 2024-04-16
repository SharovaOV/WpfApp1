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
using WpfApp1.Views.Windows;
using System.Windows;

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

        #region SelectedQestion : Question - Выделе вопрос
        private Question _SelectedQuestion;
        /// <summary>Выделенный вопрос</summary>
        public Question SelectedQestion
        {
            get => _SelectedQuestion;
            set{ 
                Set(ref _SelectedQuestion, value);
                Answers = new(value.Answers);
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


        #endregion


        #region Команды

        #region DeleteQuestionCommand
        /// <summary> Событие удалить вопрос </summary>
        public ICommand DeleteQuestionCommand { get; }

        private bool CanDeleteQuestionCommandExecute(object t) => t is Question question && Questions.Contains(question);
        private void OnDeleteQuestionCommandExecuted(object t)
        {
            if (!(t is Question question)) return;
            Questions.Remove(question);
        }
        #endregion

        #region DeleteAnswerCommand
        /// <summary> Событие удалить вопрос </summary>
        public ICommand DeleteAnswerCommand{ get; }

        private bool CanDeleteAnswerCommandExecute(object t) => t is Answer answer && Answers.Contains(answer);
        private void OnDeleteAnswerCommandExecuted(object t)
        {
            if (!(t is Answer answer)) return;
            Answers.Remove(answer);
        }
        #endregion

        #region EdiqQuestionCommand
        private ICommand _EdiqQuestionCommand;
        public ICommand EdiqQuestionCommand => _EdiqQuestionCommand ??= new LambdaCommand(OnEdiqQuestionCommandExrcuted, CanEdiqQuestionCommandExecute);
        private static bool CanEdiqQuestionCommandExecute(object p) => p is Question;
        private void OnEdiqQuestionCommandExrcuted(object p)
        {
            var question = (Question)p;
            var dlg = new QuestionEdit {
                Owner = Application.Current.MainWindow,
                DataContext = new QuestionEditViewModel
                {
                    ValueQuest = question.Value,
                    AnswerType = question.TypeAnswer
                }};



            if (dlg.ShowDialog() == true)
                MessageBox.Show("Сохранил");
            else
                MessageBox.Show("Отменил");

        }
        #endregion
        #endregion
        public EditTestViewModel()
        {
            #region Команды
            DeleteQuestionCommand = new RelayCommand(OnDeleteQuestionCommandExecuted, CanDeleteQuestionCommandExecute);
            DeleteAnswerCommand = new RelayCommand(OnDeleteAnswerCommandExecuted, CanDeleteAnswerCommandExecute);
            #endregion

            Questions = new(JSON.LoadTest("1").Questions);
            SelectedQestion = Questions.First();
            Answers = new(SelectedQestion.Answers);
            SelectedAnswer = Answers.First();
        }
    }
}
