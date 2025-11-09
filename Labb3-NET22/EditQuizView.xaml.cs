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
    /// Interaction logic for EditQuizView.xaml
    /// </summary>
    public partial class EditQuizView : UserControl
    {
        public EditQuizViewModel ViewModel { get; set; }
        public EditQuizView()
        {
            InitializeComponent();
            ViewModel= new EditQuizViewModel();
            DataContext = ViewModel;

        }
        private async void OpenQuiz_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*",
                Title = "Open Quiz File"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                string quizTitle = System.IO.Path.GetFileNameWithoutExtension(filePath);
                try
                {
                     await ViewModel.LoadQuizForEditAsync(filePath);
                    MessageBox.Show("Quiz loaded successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading quiz: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private async void UploadImage_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image Files (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp|All Files (*.*)|*.*",
                Title = "Select Image File"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                string imagePath = openFileDialog.FileName;
                ViewModel.SelectedQuestion.ImagePath = imagePath;

                ViewModel.OnPropertyChanged(nameof(ViewModel.SelectedQuestion.ImagePath));
                ViewModel.OnPropertyChanged(nameof(ViewModel.SelectedQuestion.ImagePreview));
                MessageBox.Show("Image uploaded successfully.");
                //await Task.CompletedTask;
            }
        }
        private async void SaveQuestion_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.SaveCurrentQuestionAsync();
        }
        private async void SaveQuiz_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.SaveQuizAsync();
        }
    }
}
