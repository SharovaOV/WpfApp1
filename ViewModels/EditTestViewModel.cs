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
using System.Windows.Controls;
using WpfApp1.Views.Elements;

namespace WpfApp1.ViewModels
{
    class EditTestViewModel : ViewModelBase
    {
        string _TestId { get; set; }
        Test _Test { get; set; }
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

        #region CurrentTypeView : UserControl - Вид представления ответа
        private UserControl _CurrentTypeView;
        /// <summary>Вид представления ответа</summary>
        public UserControl CurrentTypeView
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
                if (_SelectedQuestion != null)
                {                    
                    Answers = new(value.Answers);
                    CurrentTypeView = _SelectedQuestion.TypeAnswer switch
                    {
                        TypeAnswer.Text => new TableAnswersCheckText { DataContext = this },
                        TypeAnswer.Image => new TableAnswersCheckImg { DataContext = this },
                        TypeAnswer.Strings => new TableAnswersCheckStrings { DataContext = this },
                        _ => new TableAnswersCheckText { DataContext = this },
                    };
                }
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
            UpdateTest();
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
            SelectedQestion.Answers = Answers.ToList();
            UpdateTest();
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

            if(_UserDialog.Edit(question)&& !string.IsNullOrWhiteSpace(question.Value))
            {
                if(typeanswer != question.TypeAnswer)
                {
                    question.Answers = new();
                    Answers = new();
                    SelectedQestion = question;
                }
                UpdateTest();
            }
        }
        #endregion
        #region EdiqAnswerCommand
        private ICommand _EdiqAnswerCommand;
        public ICommand EdiqAnswerCommand => _EdiqAnswerCommand ??= new LambdaCommand(OnEdiqAnswerCommandExrcuted, CanEdiqAnswerCommandExecute);
        private static bool CanEdiqAnswerCommandExecute(object p) => p is Answer;
        private void OnEdiqAnswerCommandExrcuted(object p)
        {
            if (_UserDialog.Edit(p, type: (int)SelectedQestion.TypeAnswer) && !string.IsNullOrEmpty(((Answer)p).Value))
            {
                Answers = Answers;
                SelectedAnswer = (Answer)p;
            }
            UpdateTest();
        }
        #endregion

        #region CreateQuestionCommand
        public ICommand CreateQuestionCommand { get; }
        private bool CanCreateQuestionCommandExecute(object p)=>true;
        private void OnCreateQuestionCommandExrcuted(object p)
        {
            Question question = new();
            
            if (_UserDialog.Edit(question) && !string.IsNullOrWhiteSpace(question.Value))
            {
                int LastId = 0;
                if(Questions.Count >0)
                {
                    LastId = Convert.ToInt32( Questions.Last().Id.Split(".").Last());
                }
                question.Id = _TestId + "." + (++LastId);
                Questions.Add(question);
                SelectedQestion = question;
                UpdateTest();
            }
        }
        #endregion
        #region CreateAnswerCommand
        public ICommand CreateAnswerCommand { get; }
        private bool CanCreateAnswerCommandExecute(object p) => SelectedQestion is not null;
        private void OnCreateAnswerCommandExrcuted(object p)
        {
            Answer answer = new();

            if (_UserDialog.Edit(answer,type: (int)SelectedQestion.TypeAnswer) && !string.IsNullOrWhiteSpace(answer.Value) )
            {
                int LastId = 0;
                if (Answers.Count > 0)
                {
                    LastId = Convert.ToInt32(Answers.Last().Id.Split("a").Last());
                }
                answer.Right = SelectedQestion.TypeAnswer == TypeAnswer.Strings || Answers.Count == 0;
                    
                answer.Id = SelectedQestion + "a" + (++LastId);
                SelectedQestion.Answers.Add(answer);
                Answers = new(SelectedQestion.Answers);
                SelectedAnswer = answer;
                JSON.UpdateTest(_Test);
            }
        }
        #endregion

        #region CheckedCommand
        public ICommand CheckedCommand { get; }
        private bool CanCheckedCommandExecute(object check)=>true;
        private void OnCheckedCommandExrcuted(object check)
        {            
            SelectedAnswer.Right = (bool)check == true ? false : true;
            JSON.UpdateTest(_Test);
        }
        #endregion
        #endregion
        public EditTestViewModel(IUserDialogService userDialog, string testId)
        {
            _UserDialog = userDialog;
            #region Команды
            _TestId = testId;
            CurrentTypeView = new TableAnswersCheckText(){DataContext=this};
            CreateQuestionCommand = new RelayCommand(OnCreateQuestionCommandExrcuted, CanCreateQuestionCommandExecute);
            CreateAnswerCommand = new RelayCommand(OnCreateAnswerCommandExrcuted, CanCreateAnswerCommandExecute);
            DeleteQuestionCommand = new RelayCommand(OnDeleteQuestionCommandExecuted, CanDeleteQuestionCommandExecute);
            DeleteAnswerCommand = new RelayCommand(OnDeleteAnswerCommandExecuted, CanDeleteAnswerCommandExecute);
            CheckedCommand = new RelayCommand(OnCheckedCommandExrcuted, CanCheckedCommandExecute);
            #endregion
            _Test = JSON.LoadTest(_TestId);
            ReloadQuestions();
        }
         private void UpdateTest()
        {
            _Test.Questions = Questions.ToList();
            JSON.UpdateTest(_Test);
            ReloadQuestions();
        }

        private void ReloadQuestions()
        {
            int index = (SelectedQestion != null) ? Questions.IndexOf(SelectedQestion) : - 1;
            Questions = (_Test.Questions.Count == 0) ? new() : new(_Test.Questions);
            SelectedQestion = null;
            SelectedQestion = Questions.Count == 0 ? null
                : (index == -1) ? Questions.First()
                : Questions[index];

            ReloaderAnswers();
        }

        private void ReloaderAnswers()
        {
            int index = (SelectedAnswer != null) ? Answers.IndexOf(SelectedAnswer) : -1;
            Answers = (SelectedQestion == null) ? new() : new(SelectedQestion.Answers);
            SelectedAnswer = Answers.Count == 0 ? null
                : (index == -1) ? Answers.First()
                : Answers[index];
        }

    }
}
