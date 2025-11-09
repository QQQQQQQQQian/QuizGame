using Labb3_NET22.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace Labb3_NET22
{
    public class EditQuizViewModel:INotifyPropertyChanged
    {
        public Quiz Quiz { get; set; }

        private Question _selectedQuestion;
       
       
        public Question SelectedQuestion
        {
            get => _selectedQuestion;
            set
            {
                _selectedQuestion = value;
                OnPropertyChanged(nameof(SelectedQuestion));
                OnPropertyChanged(nameof(SelectedQuestion.Answer1));
                OnPropertyChanged(nameof(SelectedQuestion.Answer2));
                OnPropertyChanged(nameof(SelectedQuestion.Answer3));
            }
        }

       

        public EditQuizViewModel()
        {
            Quiz = new Quiz("New Quiz");
        }

        public async Task LoadQuizForEditAsync(string filePath)
        {
            Quiz = await Quiz.LoadFromJsonAsync(filePath);
            foreach(var q in Quiz.myQuestions)
            {
                if (q.Answers == null || !q.Answers.Any())
                {
                    q.Answers = new string[3];
                }
                q.CorrectAnswerIndex = q.CorrectAnswer;
            }
            if (Quiz.myQuestions == null || !Quiz.myQuestions.Any())
            {
                throw new InvalidOperationException("Quiz contains no questions.");
            }
            OnPropertyChanged(nameof(Quiz));
            OnPropertyChanged(nameof(SelectedQuestion));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        public async Task SaveCurrentQuestionAsync()
        {
            if(SelectedQuestion == null) return;

            int index = Quiz.myQuestions.IndexOf(SelectedQuestion);
            if(index >=0)
            {
                SelectedQuestion.CorrectAnswer = SelectedQuestion.CorrectAnswerIndex;
                Quiz.myQuestions[index] = SelectedQuestion;
            }
            else
            {
                Quiz.myQuestions.Add(SelectedQuestion);
            }
            Quiz.SyncQuestions();
            MessageBox.Show("Question updated in memory.");
        }
        public async Task SaveQuizAsync()
        {
            
            try
            {
                await SaveCurrentQuestionAsync();

                //Quiz.SyncQuestions();

                string filePath = string.IsNullOrEmpty(Quiz.FilePath) ? Quiz.GetQuizFilePath(Quiz.Title) : Quiz.FilePath;
                string tempPath= filePath + ".tmp";

                string json =JsonSerializer.Serialize(Quiz, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(tempPath, json);

                if(File.Exists(filePath))
                
                    File.Delete(filePath);
                File.Move(tempPath, filePath);

                //await Quiz.SaveToJsonAsync(Quiz.Title);
                MessageBox.Show("Quiz saved successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving quiz: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
