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
    /// Interaction logic for CreateQuizView.xaml
    /// </summary>
    public partial class CreateQuizView : UserControl
    {
        public CreateQuizViewModel ViewModel { get; set; }
        
        public CreateQuizView()
        {
            InitializeComponent();
            ViewModel = new CreateQuizViewModel();
            DataContext = ViewModel;
        }
        public async void AddButton_Click(Object sender, RoutedEventArgs e)
        {
            
           await ViewModel.AddQuestionAsync(QuestionTextBox.Text,CorrectAnswerCombo.SelectedIndex, CategoryTextBox.Text,
                                            new string[] {Answer1TextBox.Text,Answer2TextBox.Text,Answer3TextBox.Text},
                                  ImagePathTextBlock.Text);

            QuestionOutPutTextBox.Text = string.Join(Environment.NewLine, ViewModel.Quiz.Questions.Select((q, index) =>
                $"{index + 1}. {q.Statement} (Correct Answer: {q.Answers[q.CorrectAnswer]} )"));

            StatusTextBlock.Text = ViewModel.StatusMessage;
        }
        public async void SaveButton_Click(Object sender, RoutedEventArgs e)
        {
            ViewModel.Title = QuizTitleTextBox.Text;
            await ViewModel.SaveQuizAsync();
            StatusTextBlock.Text = ViewModel.StatusMessage;

        }
        private void UploadImageButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFile = openFileDialog.FileName;
                ImagePathTextBlock.Text = openFileDialog.FileName;
                QuestionImagePreview.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                QuestionImagePreview.Visibility = Visibility.Visible;
            }
            ViewModel.CurrentImagePath = ImagePathTextBlock.Text;
        }
        public void ClearButton_Click(Object sender, RoutedEventArgs e)
        {
            QuestionTextBox.Clear();
            Answer1TextBox.Clear();
            Answer2TextBox.Clear();
            Answer3TextBox.Clear();
            CorrectAnswerCombo.SelectedIndex=-1;
            StatusTextBlock.Text = "Cleared!";
        }
        public void BackToMenu_Click(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this) as MainWindow;
            if (window != null)
            {
                window.MainContentControl.Content = null;
                window.MainMenuPanel.Visibility = Visibility.Visible;
            }
        }
       
    }
}
