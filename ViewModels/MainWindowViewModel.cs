using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfApp1.Infrastucture;
using WpfApp1.Models;
using System.Collections.ObjectModel;
using WpfApp1.Resources;
using WpfApp1.Services.JSON;
using WpfApp1.Services;
using Microsoft.Win32;
using Params = WpfApp1.Properties.Settings;

namespace WpfApp1.ViewModels
{
   
    internal class MainWindowViewModel : ViewModelBase
    {
        private readonly IUserDialogService _UserDialog;


        #region Title : string - Заголовок окна
        private string _Tiile = "Менеджер тестов";
        /// <summary>Заголовок окна</summary>
        public string Title
        {
            get => _Tiile;
            set => Set(ref _Tiile, value);
        }
        #endregion
        #region FooterString : string - Надпись внизу окна
        private string _FooterString = "Список тестов";
        /// <summary>Надпись внизу окна</summary>
        public string FooterString
        {
            get => _FooterString;
            set => Set(ref _FooterString, value);
        }
        #endregion

        #region Tests : ICollection<Test> - Список тестов
        private ICollection<Test> _Tests;
        /// <summary>Список тестов</summary>
        public ICollection<Test> Tests
        {
            get => _Tests;
            set => Set(ref _Tests, value);
        }
        #endregion



        #region SelectedTest : Test - Выделенный тест
        private Test _SelectedTest;
        /// <summary>Выделенный тест</summary>
        public Test SelectedTest
        {
            get => _SelectedTest;
            set => Set(ref _SelectedTest, value);
        }
        #endregion

        #region TestInfo : ObservableCollection<Test> - Информация о тесте
        private ObservableCollection<Test> _TestInfo;
        /// <summary>Информация о тесте</summary>
        public ObservableCollection<Test> TestInfo
        {
            get => _TestInfo;
            set => Set(ref _TestInfo, value);
        }
        #endregion

        #region CurrentView : ViewModelBase - Текущая страница
        private ViewModelBase _CurrentView;
        /// <summary>Текущая страница</summary>
        public ViewModelBase CurrentView
        {
            get => _CurrentView;
            set
            {
                Set(ref _CurrentView, value);

                switch (_CurrentView)
                {
                    case SolutionTestViewModel:
                        FooterString = "Прохождение теста";
                        break;
                    case EditTestViewModel:
                        FooterString = "Редактирование содеожимого теста";
                        break;
                    default:
                        FooterString = "Список достурых тестов";
                        break;
                }
            }
        }
        #endregion

        #region Комманды

        #region CloseApplicationCommand
        /// <summary>Событие закрытие окна</summary>
        public ICommand CloseApplicationCommand { get;  }

        private bool CanCloseApplicationCommandExecute(object p) => true;
        private void OnCloseApplicationCommandExecuted(object p)
        {
            Application.Current.Shutdown();
        }
        #endregion

        #region CreateTestCommand
        /// <summary> Событие добавить тест </summary>
        public ICommand CreateTestCommand { get; }
        private bool CanCreateTestCommandExecute(object t) => CurrentView is HomeViewModel;
        private void OnCreateTestCommandExecuted(object t)
        {
            bool needToAdd =false;
            if (t is null) 
            {
                t = new Test();
                needToAdd = true;
            }
            var test = (Test)t;
            if (_UserDialog.Edit(test, "Окно редактирования заголовка теста") == false && string.IsNullOrWhiteSpace(test.Name))
            {
                _UserDialog.ShowInformation("Невозможно создать тест без заголовка!", "Создание теста отменено!");
                return;
            }
            else if (string.IsNullOrWhiteSpace(test.Name))
            {
                _UserDialog.ShowInformation("Невозможно задать пустой заголовок!", "Редактирование теста отменено!");
                return;
            }

            if (needToAdd)
                ((HomeViewModel)CurrentView).AddTestCommand.Execute(test);
            else
            {
                test.Questions = JSON.LoadTest(test.Id).Questions;
                JSON.UpdateTest(test);
            }
             CurrentView = new EditTestViewModel(_UserDialog, test.Id);
        }
        #endregion

        #region StartSolutionCommand
        /// <summary> Событие пройти тест </summary>
        public ICommand StartSolutionCommand { get; }
        private bool CanStartSolutionCommandExecute(object t) => CurrentView is HomeViewModel home && home.SelectedTest != null;
        private void OnStartSolutionCommandExecute(object t)
        {
            CurrentView = new SolutionTestViewModel(((HomeViewModel)CurrentView).SelectedTest.Id, _UserDialog);
        }
        #endregion

        #region GoHomeCommand
        /// <summary> Событие пройти тест </summary>
        public ICommand GoHomeCommand { get; }
        private bool CanGoHomeCommandExecute(object t) => CurrentView is not HomeViewModel;
        private void OnGoHomeCommandExecuted(object t)
        {
            if(CurrentView is SolutionTestViewModel)
            {
                if(t is UserTest)
                {
                    ((SolutionTestViewModel)CurrentView).GetResultCommand.Execute(t);
                }
                else if(_UserDialog.Confirm("Это Действие приведет к потере прогресса!\n Вы действительно хотите прервать выполнение теста? ", "Экстренный выход из теста!", true) == false)
                    return;
            }
            CurrentView = new HomeViewModel(CreateTestCommand, _UserDialog);
        }
        #endregion
        #region LoadBaseCommand
        /// <summary> Событие загрузить базу</summary>
        public ICommand LoadBaseCommand { get; }
        private bool CanLoadBaseCommandExecute(object t) => CurrentView is HomeViewModel;
        private void OnLoadBaseCommandExecuted(object t)
        {
            var FDialog = new OpenFileDialog();
            FDialog.Filter = "JSON Files | *.json";

            if (FDialog.ShowDialog() == true && !string.IsNullOrWhiteSpace(FDialog.FileName))
            {
                Params.Default.SetBasePath = FDialog.FileName.ToBaseDirectory();
                Params.Default.Save();
                JSON.SetJsonPath(Params.Default.SetBasePath);
                CurrentView = new HomeViewModel(CreateTestCommand, _UserDialog);
            }
        }
        # endregion

        #region CreateBaseCommand
        /// <summary> Событие загрузить базу</summary>
        public ICommand CreateBaseCommand { get; }
        private bool CanCreateBaseCommandExecute(object t) => CurrentView is HomeViewModel;
        private void OnCreateBaseCommandExecuted(object t)
        {
            var FDialog = new SaveFileDialog();
            FDialog.Filter = "JSON Files | *.json";

            if (FDialog.ShowDialog() == true && !string.IsNullOrWhiteSpace(FDialog.FileName))
            {
                Params.Default.SetBasePath = FDialog.FileName.ToBaseDirectory();
                Params.Default.Save();
                JSON.SetJsonPath(Params.Default.SetBasePath);
                JSON.SaveDB(new());
                CurrentView = new HomeViewModel(CreateTestCommand, _UserDialog);
            }
        }
        # endregion


        #endregion


        public MainWindowViewModel(IUserDialogService userDialog)
        {
            _UserDialog = userDialog;
            JSON.SetJsonPath(Params.Default.SetBasePath);
            #region Команды
            CloseApplicationCommand = new RelayCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);
            CreateTestCommand = new RelayCommand(OnCreateTestCommandExecuted, CanCreateTestCommandExecute);
            StartSolutionCommand = new RelayCommand(OnStartSolutionCommandExecute, CanStartSolutionCommandExecute);
            GoHomeCommand = new RelayCommand(OnGoHomeCommandExecuted, CanGoHomeCommandExecute);
            LoadBaseCommand = new RelayCommand(OnLoadBaseCommandExecuted, CanLoadBaseCommandExecute);
            CreateBaseCommand = new RelayCommand(OnCreateBaseCommandExecuted, CanCreateBaseCommandExecute);
            #endregion
            CurrentView = new HomeViewModel(CreateTestCommand, _UserDialog);
        }

    }



}
