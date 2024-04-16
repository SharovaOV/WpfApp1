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


namespace WpfApp1.ViewModels
{
   
    internal class MainWindowViewModel : ViewModelBase
    {
        enum TabWindow
        {
            Start=0,
            EditTest,
            DoTest
        }
        private readonly IUserDialogService _UserDialog;

        #region Title : string - Заголовок окна
        private string _Tiile = "Заголовок окна";
        /// <summary>Заголовок окна</summary>
        public string Title
        {
            get => _Tiile;
            set => Set(ref _Tiile, value);
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
            set => Set(ref _CurrentView, value);
        }
        #endregion

        #region Комманды

        #region CloseApplicationCommand
        /// <summary>Событие закрытие окна</summary>
        public ICommand CloseApplicationCommand { get;  }

        private bool CanCloseApplicationCommandExecuted(object p) => true;
        private void OnCloseApplicationCommandExecuted(object p)
        {
            Application.Current.Shutdown();
        }
        #endregion

        #region CreateTestCommand
        /// <summary> Событие добавить тест </summary>
        public ICommand CreateTestCommand { get; }
        private bool CanCreateTestCommandExecuted(object t)=>true;
        private void OnCreateTestCommandExecuted(object t)
        {
            bool needToAdd =false;
            if (t is null) 
            {
                t = new Test();
                needToAdd = true;
            }
            var test = (Test)t;
            if (_UserDialog.Edit(test) == false && string.IsNullOrWhiteSpace(test.Name))
            {
                _UserDialog.Confirm("Невозможно создать тест без заголовка!", "Создание теста отменено!");
                return;
            }
            else if (string.IsNullOrWhiteSpace(test.Name))
            {
                _UserDialog.Confirm("Невозможно задать пустой заголовок!", "Редактирование теста отменено!");
                return;
            }

            if(needToAdd)
                ((HomeViewModel)CurrentView).AddTestCommand.Execute(test);
             CurrentView = new EditTestViewModel(_UserDialog, test.Id);
        }
        #endregion

        #endregion


        public MainWindowViewModel(IUserDialogService userDialog)
        {
            _UserDialog = userDialog;
            JSON.SetJsonPath();
            #region Команды
            CloseApplicationCommand = new RelayCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecuted);

            //DeleteTestCommand = new RelayCommand(OnDeleteTestCommandExecuted, CanDeleteTestCommandExecuted);

            CreateTestCommand = new RelayCommand(OnCreateTestCommandExecuted, CanCreateTestCommandExecuted);
            #endregion
            CurrentView = new HomeViewModel(CreateTestCommand, _UserDialog);

            //TestInfo = new ObservableCollection<Test>(JSON.LoadTestInfoList());           

        }

    }



}
