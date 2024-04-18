using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.Models;
using WpfApp1.Views.Pages;
namespace WpfApp1.ViewModels
{
    class CreateTestViewModel : ViewModelBase
    {
        public static DependencyProperty ValueProperty = DependencyProperty.Register(
           nameof(NameTest),
           typeof(string),
           typeof(MainWindow),
           new PropertyMetadata(null));
        public string NameTest { get; set; }


        #region Title : string - Заголовок окна
        private string _Title;
        /// <summary>Заголовок окна</summary>
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }
        #endregion

        public CreateTestViewModel() { }
    }
}
