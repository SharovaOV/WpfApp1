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

        #region DeleteTestCommand
        /// <summary> Событие удалить тест </summary>
        public ICommand DeleteTestCommand { get; }

        private bool CanDeleteTestCommandExecuted(object t) => t is Test test && TestInfo.Contains(test);
        private void OnDeleteTestCommandExecuted(object t)
        {
            if (!(t is Test test)) return;
            TestInfo = new ObservableCollection<Test>(JSON.DeleteTest(test));
            //TestInfo.Remove(test);
            //JSON.UpdateJson(test);
        }
        #endregion

        #region CreateTestCommand
        /// <summary> Событие добавить тест </summary>
        private ICommand CreateTestCommand { get; }
        private bool CanCreateTestCommandExecuted(object t) => TestInfo != null;
        private void OnCreateTestCommandExecuted(object t)
        {
            TestInfo = new ObservableCollection<Test>(JSON.AddTest());
        }
        #endregion



        #endregion


        public MainWindowViewModel()
        {
            JSON.setJsonPath();
            #region Команды
            CloseApplicationCommand = new RelayCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecuted);

            DeleteTestCommand = new RelayCommand(OnDeleteTestCommandExecuted, CanDeleteTestCommandExecuted);
            #endregion

            TestInfo = new ObservableCollection<Test>(JSON.LoadTestInfoList());

        }

    }



}
