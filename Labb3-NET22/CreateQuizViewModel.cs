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
    public class CreateQuizViewModel : INotifyPropertyChanged
    {
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
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string name="")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        public void AddQuestion(string statement, string[] answers, int correctIndex,string category)
        {
            if(string.IsNullOrWhiteSpace(statement)
                ||answers.Any(a => string.IsNullOrWhiteSpace(a))
                ||correctIndex < 0 || correctIndex >= answers.Length)
            {
                StatusMessage = "Please fill in all fields correctly.";
                OnPropertyChanged("StatusMessage");
                return;
            }
            
            Quiz.AddQuestion(statement,correctIndex, category, answers);

            QuestionStatement = string.Empty;
            AnswerOptions = new string[3];
            CorrectAnswerIndex = 0;

            StatusMessage = $"Question added successfully! Total: {Quiz.Questions.Count()}";

            OnPropertyChanged("QuestionStatement");
            OnPropertyChanged("AnswerOptions");
            OnPropertyChanged("CorrectAnswerIndex");
            OnPropertyChanged("StatusMessage");

        }
        public void SaveQuiz()
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
                File.WriteAllText(FilePath, jsonString);
                
                StatusMessage = $"Quiz saved successfully! to {FilePath}";
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