using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.Views.Windows;

namespace WpfApp1.Infrastucture
{
    class ManageQuestCommand : Base.Command
    {
        private QuestionEdit _Window;
        public override bool CanExecute(object parameter) => _Window == null;

        public override void Execute(object parameter)
        {
            _Window = new QuestionEdit {
                Owner = Application.Current.MainWindow
            };

            _Window.Closed += OnWindowClosed;
            _Window.ShowDialog();
        }

        private void OnWindowClosed(object sender, EventArgs e)
        {
            ((Window)sender).Closed -= OnWindowClosed;
            _Window = null;
        }
    }
   
}
