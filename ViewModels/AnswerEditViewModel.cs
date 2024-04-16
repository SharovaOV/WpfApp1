using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Infrastucture;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;
using WpfApp1.Resources;
using WpfApp1.Views.Pages;
using Microsoft.Win32;
using System.IO;
namespace WpfApp1.ViewModels
{

    class AnswerEditViewModel :ViewModelBase
    {
        public static DependencyProperty ValueProperty = DependencyProperty.Register(
           nameof(ValueAnswer),
           typeof(string),
           typeof(MainWindow),
           new PropertyMetadata(null));
        private string _ValueAnswer;
        public string ValueAnswer {
            get => _ValueAnswer;
            set => Set(ref _ValueAnswer, value);
        }

        #region CurrentView : UserControl - Текущий элемент
        public static DependencyProperty CurrentViewProperty = DependencyProperty.Register(
           nameof(CurrentView),
           typeof(UserControl),
           typeof(MainWindow),
           new PropertyMetadata(null));
        public UserControl CurrentView { get; set; }
        #endregion

        #region Команды
        #region LoadImageCommand
        private ICommand _LoadImageCommand;
        public ICommand LoadImageCommand => _LoadImageCommand ??= new LambdaCommand(OnLoadImageCommandExrcuted);
       
        private void OnLoadImageCommandExrcuted(object p)
        {
            var FDialog = new OpenFileDialog();
            FDialog.Filter = "Image Files | *.jpg; *.jpeg; *.png";

            if(FDialog.ShowDialog()==true)
            {
                ValueAnswer = FDialog.FileName;
            }
        }
        #endregion

  

        #endregion

        public AnswerEditViewModel()
        {
            
        }
    }
}
