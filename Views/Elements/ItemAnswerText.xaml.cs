using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1.Views.Elements
{
    /// <summary>
    /// Логика взаимодействия для ItemAnswerText.xaml
    /// </summary>
    public partial class ItemAnswerText : UserControl
    {
        public bool CheckedAnswer
        {
            get { return (bool)GetValue(CheckedAnswerProperty); }
            set { SetValue(CheckedAnswerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Property1.  
        // This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CheckedAnswerProperty
            = DependencyProperty.Register(
                  "CheckedAnswer",
                  typeof(bool),
                  typeof(ItemAnswerText),
                  new PropertyMetadata(false)
              );
        public bool ValueAnswer
        {
            get { return (bool)GetValue(ValueAnswerProperty); }
            set { SetValue(ValueAnswerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Property1.  
        // This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueAnswerProperty
            = DependencyProperty.Register(
                  "ValueAnswer",
                  typeof(bool),
                  typeof(ItemAnswerText),
                  new PropertyMetadata(false)
              );
        public ItemAnswerText()
        {
            InitializeComponent();
        }
    }
}
