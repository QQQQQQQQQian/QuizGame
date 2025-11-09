using Labb3_NET22.DataModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
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
using Path = System.IO.Path;

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
            this.Loaded += PlayQuizView_Loaded;
        }
        public async void PlayQuizView_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.Quiz = await Quiz.LoadFromJsonAsync("Happy Time!");
                ViewModel.CurrentQuestion = ViewModel.Quiz.myQuestions.FirstOrDefault();
                ViewModel.SelectedAnswerIndex = -1;
                ViewModel.TotalAnswered = 0;
                ViewModel.CorrectAnswers = 0;

                if (ViewModel.SelectedCategories == null || ViewModel.SelectedCategories.Count == 0)
                    ViewModel.SelectedCategories = ViewModel.Quiz.GetCategories();

                ViewModel.OnPropertyChanged(nameof(ViewModel.CurrentQuestion));
                ViewModel.OnPropertyChanged(nameof(ViewModel.ScoreText));

               
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading quiz: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
        public void AnswerButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int selectedIndex = int.Parse(button.Tag.ToString());
            ViewModel.NextQuestion(selectedIndex);
        }
        private async void BuildQuiz_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Load all questions first
                var quizFolder = Quiz.GetQuizFolderPath();
                var allQuestions = new List<Question>();
                var files = Directory.GetFiles(quizFolder, "*.json");
                foreach (var file in files)
                {
                    try
                    {
                        string json = await File.ReadAllTextAsync(file);
                        var quiz = JsonSerializer.Deserialize<Quiz>(json);
                        if (quiz?.myQuestions != null)
                            allQuestions.AddRange(quiz.myQuestions);
                    }
                    catch { }
                }
                ViewModel.AllQuestions = allQuestions;

                if (ViewModel.AllQuestions == null || !ViewModel.AllQuestions.Any())
                {
                    MessageBox.Show("No questions available in memory.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Show category selection dialog
                var allCategories = ViewModel.AllQuestions.Select(q => q.Category).Distinct().ToList();
                var categoryWindow = new CategorySelectionWindow(allCategories, ViewModel.SelectedCategories);

                if (categoryWindow.ShowDialog() == true)
                {
                    ViewModel.SelectedCategories = categoryWindow.SelectedCategories;
                    try
                    {
                        ViewModel.BuildQuizByCategory();
                        ViewModel.TotalAnswered = 0;
                        ViewModel.CorrectAnswers = 0;
                        

                        MessageBox.Show("Quiz built successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (InvalidOperationException ex)
                    {
                        MessageBox.Show(ex.Message, "No questions available", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error building quiz: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
