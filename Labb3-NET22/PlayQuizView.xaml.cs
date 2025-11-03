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
    /// Interaction logic for PlayQuizView.xaml
    /// </summary>
    public partial class PlayQuizView : UserControl
    {
        public PlayQuizViewModel ViewModel { get; set; }
        public PlayQuizView()
        {
            InitializeComponent();
            ViewModel = new PlayQuizViewModel();
            DataContext = ViewModel;
        }
        public void AnswerButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int selectedIndex = int.Parse(button.Tag.ToString());
            ViewModel.NextQuestion(selectedIndex);
        }
        public void BuildQuiz_Click(object sender, RoutedEventArgs e)
        {

            var categoryWindow = new CategorySelectionWindow(ViewModel.Quiz.GetCategories(), ViewModel.SelectedCategories);
            if (categoryWindow.ShowDialog() == true)
            {
                ViewModel.SelectedCategories = categoryWindow.SelectedCategories;
                try
                {
                    ViewModel.LoadQuestionsByCategory();
                    ViewModel.TotalAnswered = 0;
                    ViewModel.CorrectAnswers = 0;
                    ViewModel.NextQuestion(-1);
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message, "NO questions available", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        //public void BackToMenu_Click(object sender, RoutedEventArgs e)
        //{
        //   var window = Window.GetWindow(this) as MainWindow;
        //    if (window != null)
        //    {
        //        window.MainContentControl.Content = null;
        //        window.MainMenuPanel.Visibility = Visibility.Visible;
        //    }

        //}
    }
}
