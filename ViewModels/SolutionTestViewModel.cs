using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Models;
using WpfApp1.Infrastucture;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using WpfApp1.Views.Elements;
using WpfApp1.Services.JSON;
using WpfApp1.Services;
using WpfApp1.Resources;
using System.Windows.Input;

namespace WpfApp1.ViewModels
{
    class SolutionTestViewModel : ViewModelBase
    {
        private IUserDialogService _UserDialog;
        public bool IsReadOnly{ get; set; }

        #region TestSolution : UserTest - Тест для решения
        private UserTest _TestSolution;
        
        /// <summary>Тест для решения</summary>
        public UserTest TestSolution
        {
            get => _TestSolution;
            set => Set(ref _TestSolution, value);
        }
        #endregion

        #region CurrentViewAnswers : UserControl - Текущий вид ответов
        private UserControl _CurrentViewAnswers;
        /// <summary>DescriptionProperty</summary>
        public UserControl CurrentViewAnswers
        {
            get => _CurrentViewAnswers;
            set => Set(ref _CurrentViewAnswers, value);
        }
        #endregion

        #region Answers : ObservableCollection<Answer> - Варианты ответов
        private ObservableCollection<UserAnswer> _Answers;
        /// <summary>Варианты ответов</summary>
        public ObservableCollection<UserAnswer> Answers
        {
            get => _Answers;
            set => Set(ref _Answers, value);
        }
        #endregion

        #region Questions : ObservableCollection<UserQuestion> - Коллекция воросов
        private ObservableCollection<UserQuestion> _Questions;
        /// <summary>Коллекция воросов</summary>
        public ObservableCollection<UserQuestion> Questions
        {
            get => _Questions;
            set => Set(ref _Questions, value);
            
        }
        #endregion

        #region CurrentQuest : UserQuestion - Текущий вопрос
        private UserQuestion _CurrentQuest;
        /// <summary>Текущий вопрос</summary>
        public UserQuestion CurrentQuest
        {
            get => _CurrentQuest;
            set
            {
                value.IsActive = true;
                if (_CurrentQuest != null) SetAnswers(_CurrentQuest.Index - 1);
                Set(ref _CurrentQuest, value);
                
                Answers = new(value.Answers);
                switch (value.TypeAnswer)
                {
                    case TypeAnswer.Text:
                        CurrentViewAnswers = new TableSolutionText { DataContext = this };
                        break;
                    case TypeAnswer.Image:
                        CurrentViewAnswers = new TableSolutionImg { DataContext = this };
                        break;
                    case TypeAnswer.Strings:
                        CurrentViewAnswers = new TableSolutionStrings { DataContext = this };
                        break;
                    default:
                        CurrentViewAnswers = new TableSolutionText { DataContext = this };
                        break;
                }
            }
        }
        #endregion

       

        #region Команды
        #region ItemQuestCommand
        /// <summary> Событие загрузить по номеру тест </summary>
        public ICommand ItemQuestCommand { get; }
        private bool CanItemQuestCommandExecute(object t) => t is UserQuestion;
        private void OnItemQuestCommandExecuted(object t)
        {
            CurrentQuest = (UserQuestion)t;
        }
        #endregion

        #region NextQuestCommand
        /// <summary> Событие Загрузить следующий тест </summary>
        public ICommand NextQuestCommand { get; }
        private bool CanNextQuestCommandExecute(object t) => CurrentQuest.Index < TestSolution.Questions.Count;
        private void OnNextQuestCommandExecuted(object t)
        {
            MoveIndex(1);
        }
        #endregion
        
        #region PreviewQuestCommand
        /// <summary> Событие Загрузить следующий тест </summary>
        public ICommand PreviewQuestCommand { get; }
        private bool CanPreviewQuestCommandExecute(object t) => CurrentQuest.Index > 1;
        private void OnPreviewQuestCommandExecuted(object t)
        {
            MoveIndex(-1);
        }
        #endregion

        #region GetResultCommand
        /// <summary> Событие Загрузить следующий тест </summary>
        public ICommand GetResultCommand { get; }
        private bool CanGetResultCommandExecute(object t) =>true;
        private void OnGetResultCommandExecuted(object t)
        {
            SetAnswers(CurrentQuest.Index - 1);
            _UserDialog.ShowInformation(
                $"Ваш результат {TestSolution.RightCount} из {TestSolution.Questions.Count}",
                 "Результат тестирования");                
        }
        #endregion
        #endregion


        public SolutionTestViewModel(string testId, IUserDialogService userDialog)
        {
            _UserDialog = userDialog;
            CurrentViewAnswers = new TableSolutionText { DataContext = this };
            TestSolution = new(JSON.LoadTest(testId));

            Questions = new(TestSolution.Questions);
            CurrentQuest = TestSolution.Questions.First();
            ItemQuestCommand = new RelayCommand(OnItemQuestCommandExecuted, CanItemQuestCommandExecute);
            NextQuestCommand = new RelayCommand(OnNextQuestCommandExecuted, CanNextQuestCommandExecute);
            PreviewQuestCommand = new RelayCommand(OnPreviewQuestCommandExecuted, CanPreviewQuestCommandExecute);
            GetResultCommand = new RelayCommand(OnGetResultCommandExecuted, CanGetResultCommandExecute);
        }

        
        private void UpdateQuestions(int index)
        {
            Questions = new(TestSolution.Questions);
            ItemQuestCommand.Execute(Questions[index]);
        }

        private void SetAnswers(int index)
        {
            if (IsReadOnly) return;
            if(Questions[index].TypeAnswer != TypeAnswer.Strings)
                TestSolution.Questions[index].Answers = Questions[index].Answers;
            else
                TestSolution.Questions[index].UserString = Questions[index].UserString;
        }

        private void MoveIndex(int difIndex)
        {
            TestSolution.Questions[CurrentQuest.Index - 1].IsActive = false;
            UpdateQuestions(CurrentQuest.Index + difIndex - 1);
        }
    }
}
