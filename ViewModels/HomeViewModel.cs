using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WpfApp1.Models;
using WpfApp1.Services.JSON;
using WpfApp1.Infrastucture;
using System.Windows.Input;
using WpfApp1.Services;
namespace WpfApp1.ViewModels
{
    class HomeViewModel:ViewModelBase
    {
        private readonly IUserDialogService _UserDialog;
        
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


        #region Команды
        #region DeleteTestCommand
        /// <summary> Событие удалить тест </summary>
        public ICommand DeleteTestCommand { get; }

        private bool CanDeleteTestCommandExecute(object t) => t is Test test && TestInfo.Contains(test);
        private void OnDeleteTestCommandExecuted(object t)
        {
            if (t is not Test test) return;
            TestInfo = new ObservableCollection<Test>(JSON.DeleteTest(test));
            //TestInfo.Remove(test);
            //JSON.UpdateJson(test);
        }
        #endregion
        #region EditTestCommand
        /// <summary> Редактировать тест </summary>
        public ICommand EditTestCommand { get; set; }
        #endregion
        #region AddTestCommand
        /// <summary> Событие удалить тест </summary>
        public ICommand AddTestCommand { get; }

        private bool CanAddTestCommandExecute(object t) => t is Test && t != null;
        private void OnAddTestCommandExecuted(object t)
        {
            TestInfo = new(JSON.AddTest((Test)t));            
        }
        #endregion
        #endregion
        public HomeViewModel(ICommand editTestCommand, IUserDialogService userDialog)
        {
            _UserDialog = userDialog;
            EditTestCommand = editTestCommand;
            #region Команды
            DeleteTestCommand = new RelayCommand(OnDeleteTestCommandExecuted, CanDeleteTestCommandExecute);
            AddTestCommand = new RelayCommand(OnAddTestCommandExecuted, CanAddTestCommandExecute);
            #endregion
            IList<Test> tests = JSON.LoadTestInfoList();
            TestInfo = tests==null ? new() : new(JSON.LoadTestInfoList());
            
            SelectedTest = (TestInfo != null && TestInfo.Count>0)? TestInfo.First(): null; 
        }
    }
}
