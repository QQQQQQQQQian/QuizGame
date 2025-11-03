using Labb3_NET22.DataModels;
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

namespace Labb3_NET22
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Play_Click(object sender, RoutedEventArgs e)
        {
            
            MainContentControl.Content = new PlayQuizView();
            (sender as Button).Parent.SetValue(UIElement.VisibilityProperty, Visibility.Collapsed);

        }
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            MainContentControl.Content= new EditQuizView();
            (sender as Button).Parent.SetValue(UIElement.VisibilityProperty, Visibility.Collapsed);

        }
        private void Create_Click(object sender, RoutedEventArgs e)
        {
            MainContentControl.Content=new CreateQuizView();
            (sender as Button).Parent.SetValue(UIElement.VisibilityProperty, Visibility.Collapsed);
        }
    }
}
