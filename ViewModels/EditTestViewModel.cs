using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp1.Models;
using WpfApp1.Services.JSON;
using WpfApp1.Services;
using WpfApp1.Infrastucture;
using WpfApp1.Resources;
using WpfApp1.Views.Windows;
using System.Windows;

namespace WpfApp1.ViewModels
{
    class EditTestViewModel : ViewModelBase
    {
        #region Поля
        private readonly IUserDialogService _UserDialog;

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
                if(_SelectedQuestion != null)
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
            if (t is not Question question) return;
            Questions.Remove(question);
        }
        #endregion

        #region DeleteAnswerCommand
        /// <summary> Событие удалить вопрос </summary>
        public ICommand DeleteAnswerCommand{ get; }

        private bool CanDeleteAnswerCommandExecute(object t) => t is Answer answer && Answers.Contains(answer);
        private void OnDeleteAnswerCommandExecuted(object t)
        {
            if (t is not Answer answer) return;
            Answers.Remove(answer);
        }
        #endregion

        #region EdiqQuestionCommand
        private ICommand _EdiqQuestionCommand;
        public ICommand EdiqQuestionCommand => _EdiqQuestionCommand ??= new LambdaCommand(OnEdiqQuestionCommandExrcuted, CanEdiqQuestionCommandExecute);
        private static bool CanEdiqQuestionCommandExecute(object p) => p is Question;
        private void OnEdiqQuestionCommandExrcuted(object p)
        {
            Question question = (Question)p;
            TypeAnswer typeanswer = question.TypeAnswer;

            if(_UserDialog.Edit(question))
            {
                if(typeanswer != question.TypeAnswer)
                {
                    question.Answers = new();
                    Answers = new();
                    SelectedQestion = question;
                }
            }
        }
        #endregion
        #region EdiqAnswerCommand
        private ICommand _EdiqAnswerCommand;
        public ICommand EdiqAnswerCommand => _EdiqAnswerCommand ??= new LambdaCommand(OnEdiqAnswerCommandExrcuted, CanEdiqAnswerCommandExecute);
        private static bool CanEdiqAnswerCommandExecute(object p) => p is Answer;
        private void OnEdiqAnswerCommandExrcuted(object p)
        {
            if (_UserDialog.Edit(p, type: (int)SelectedQestion.TypeAnswer))
            {

                Answers = Answers;
                SelectedAnswer = (Answer)p;
            }
        }
        #endregion

        #region CreateQuestionCommand
        public ICommand CreateQuestionCommand { get; }
        private bool CanCreateQuestionCommandExecute(object p)=>true;
        private void OnCreateQuestionCommandExrcuted(object p)
        {
            Question question = new();
            
            if (_UserDialog.Edit(question))
            {
                Questions.Add(question);
                SelectedQestion = question;
            }
        }
        #endregion
        #region CreateAnswerCommand
        public ICommand CreateAnswerCommand { get; }
        private bool CanCreateAnswerCommandExecute(object p) => SelectedQestion is not null;
        private void OnCreateAnswerCommandExrcuted(object p)
        {
            Answer answer = new();

            if (_UserDialog.Edit(answer,type: (int)SelectedQestion.TypeAnswer))
            {
                SelectedQestion.Answers.Add(answer);
                Answers = new(SelectedQestion.Answers);
                SelectedAnswer = answer;
            }
        }
        #endregion
        #endregion
        public EditTestViewModel(IUserDialogService userDialog, string testId)
        {
            _UserDialog = userDialog;
            #region Команды
            CreateQuestionCommand = new RelayCommand(OnCreateQuestionCommandExrcuted, CanCreateQuestionCommandExecute);
            CreateAnswerCommand = new RelayCommand(OnCreateAnswerCommandExrcuted, CanCreateAnswerCommandExecute);
            DeleteQuestionCommand = new RelayCommand(OnDeleteQuestionCommandExecuted, CanDeleteQuestionCommandExecute);
            DeleteAnswerCommand = new RelayCommand(OnDeleteAnswerCommandExecuted, CanDeleteAnswerCommandExecute);
            #endregion
            Test test = JSON.LoadTest(testId);
            Questions = (test.CountQuestions == 0)? new(): new(JSON.LoadTest(testId).Questions);
            SelectedQestion = Questions.Count == 0 ? null : Questions.First();
            Answers = (SelectedQestion == null)? new() : new(SelectedQestion.Answers);
            SelectedAnswer = Answers.Count == 0 ? null : Answers.First();
        }
    }
}
