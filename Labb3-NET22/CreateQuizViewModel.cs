using Labb3_NET22.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace Labb3_NET22
{
    public class CreateQuizViewModel : INotifyPropertyChanged
    {
        public string  CurrentImagePath {get; set;}
        public string Title { get; set; }

        public string QuestionStatement { get; set; }
        public string[] AnswerOptions { get; set; } = new string[3];
        public int CorrectAnswerIndex { get; set; }
        public string StatusMessage { get; set; }
        public string QuestionCategory { get; set; }
        

        public Quiz Quiz { get; set; }
        public CreateQuizViewModel()
        {
            Quiz = new Quiz();
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string name="")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        public async Task AddQuestionAsync(string statement, int correctIndex, string category, string[] answers, string imagePath=null)
        {
            if(string.IsNullOrWhiteSpace(statement) || string.IsNullOrWhiteSpace(category)
                ||answers.Any(a => string.IsNullOrWhiteSpace(a))
                ||correctIndex < 0 || correctIndex >= answers.Length)
            {
                StatusMessage = "Please fill in all fields correctly.";
                OnPropertyChanged("StatusMessage");
                return;
            }
            string savedImagePath = null;
            if (!string.IsNullOrEmpty(imagePath))
            {
                string imagesFolder=Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "QuizGame", "Images");
                if (!Directory.Exists(imagesFolder))
                {
                    Directory.CreateDirectory(imagesFolder);
                }
                string fileName = Path.GetFileName(imagePath);
                savedImagePath = Path.Combine(imagesFolder, fileName);
                try
                {
                    File.Copy(imagePath, savedImagePath, true);
                }
                catch (Exception ex)
                {
                    StatusMessage = $"Error copying image: {ex.Message}";
                    OnPropertyChanged("StatusMessage");
                    return;
                }
            }
            

            Quiz.AddQuestion(statement, correctIndex, category, answers, savedImagePath);
            CurrentImagePath = null;
            try
            {
                await Quiz.SaveToJsonAsync(Quiz.Title);
                StatusMessage = $"Question added successfully! Total: {Quiz.Questions.Count()}";
                Debug.WriteLine($"Questions in quiz: {Quiz.Questions.Count()}");


            }
            catch (Exception ex) 
            { 
                StatusMessage = ex.Message;
            }

            

            QuestionStatement = string.Empty;
            AnswerOptions = new string[3];
            CorrectAnswerIndex = 0;
            QuestionCategory = string.Empty;

            

            OnPropertyChanged("QuestionStatement");
            OnPropertyChanged("AnswerOptions");
            OnPropertyChanged("CorrectAnswerIndex");
            OnPropertyChanged("StatusMessage");

        }
        public async Task SaveQuizAsync()
        {
            if (string.IsNullOrWhiteSpace(Title))
            {
                StatusMessage = "Please provide a title for the quiz.";
                OnPropertyChanged("StatusMessage");
                return;
            }
          

            try
            {
                string FilePath = Quiz.GetQuizFilePath(Title);
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                string jsonString = JsonSerializer.Serialize(Quiz, options);
                await File.WriteAllTextAsync(FilePath, jsonString);
                
                StatusMessage = $"Quiz saved successfully! Path: {FilePath}";
                OnPropertyChanged("StatusMessage");


            }
            catch (Exception ex)
            {
                StatusMessage = $"Error saving quiz: {ex.Message}";
                OnPropertyChanged("StatusMessage");

            }
        }
    }
}