using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Models;
using WpfApp1.Services.JSON;
using WpfApp1.Infrastucture;
using System.Windows.Input;
namespace WpfApp1.ViewModels
{
    class HomeViewModel:ViewModelBase
    {
        #region Поля

        #region TestInfo : ObservableCollection<Test> - Список доступных тестов
        private ObservableCollection<Test> _TestInfo;
        /// <summary>Список доступных тестов</summary>
        public ObservableCollection<Test> TestInfo
        {
            get => _TestInfo;
            set => Set(ref _TestInfo, value);
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

        #endregion


        #region Команды
        #region DeleteTestCommand
        /// <summary> Событие удалить тест </summary>
        public ICommand DeleteTestCommand { get; }

        private bool CanDeleteTestCommandExecuted(object t) => t is Test test && TestInfo.Contains(test);
        private void OnDeleteTestCommandExecuted(object t)
        {
            if (!(t is Test test)) return;
            TestInfo = new ObservableCollection<Test>(JSON.DeleteTest(test));
            //Questions.Remove(test);
            //JSON.UpdateJson(test);
        }
        #endregion

        #endregion
        public HomeViewModel()
        {
            #region Команды
            DeleteTestCommand = new RelayCommand(OnDeleteTestCommandExecuted, CanDeleteTestCommandExecuted);
            #endregion

            TestInfo = new(JSON.LoadTestInfoList());
            SelectedTest = TestInfo.First(); 
        }
    }
}
