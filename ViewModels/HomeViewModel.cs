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
        #region Свойства

        #region TestInfo : ObservableCollection<Test> - Список доступных тестов
        private ObservableCollection<Test> _TestInfo;
        /// <summary>Список доступных тестов</summary>
        public ObservableCollection<Test> TestInfo
        {
            get => _TestInfo;
            set => Set(ref _TestInfo, value);
        }
        #endregion

        #endregion


        #region Команды

        #endregion
        public HomeViewModel()
        {
            TestInfo = new(JSON.LoadTestInfoList());
        }
    }
}
