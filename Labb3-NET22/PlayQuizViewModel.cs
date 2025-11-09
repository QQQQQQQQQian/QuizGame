using Labb3_NET22.DataModels;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Labb3_NET22
{
    public class PlayQuizViewModel:INotifyPropertyChanged
    {
        public Quiz Quiz { get;set; }
        //public Question CurrentQuestion { get;set; }
        public int SelectedAnswerIndex { get;set; }
        public int CorrectAnswers { get;set; }
        public int TotalAnswered{ get;set; }
        public List<Question> AllQuestions { get; set; } = new List<Question>();
        public ObservableCollection<Question> Questions { get => Quiz?.myQuestions; }
        private Question _currentQuestion;
        public List<Question> remainingQuestions;
        public bool IsQuizCompleted { get; private set; }
        public Question CurrentQuestion
        {
            get => _currentQuestion;
            set
            {
                _currentQuestion = value;
                OnPropertyChanged(nameof(CurrentQuestion));
            }
        }

        public List<string> SelectedCategories { get; set; } = new List<string>();
        public string ScoreText
        {
            get
            {
                int percent =0;
                if(TotalAnswered > 0)
                {
                    percent = (int)((double)CorrectAnswers / TotalAnswered * 100);
                }
                return $"Score: {CorrectAnswers}/{TotalAnswered} ({percent}%)";
            }
        }
        public PlayQuizViewModel()
        {
            Quiz = new Quiz();

        }

        public async Task LoadQuizAsync(string title)
        {
            var quiz = await Quiz.LoadFromJsonAsync(title);
            Quiz = quiz;
            if (Quiz.myQuestions == null || !Quiz.myQuestions.Any())
                quiz.myQuestions = new ObservableCollection<Question>();

           

            CurrentQuestion = Quiz.GetRandomQuestion();
            if (CurrentQuestion == null)
                throw new InvalidOperationException("Failed to get a random question.");

            SelectedAnswerIndex = -1;
            TotalAnswered = 0;
            CorrectAnswers = 0;
            OnPropertyChanged(nameof(Quiz));
            OnPropertyChanged(nameof(Questions));
            OnPropertyChanged(nameof(SelectedAnswerIndex));
            OnPropertyChanged(nameof(ScoreText));

        }
        public async Task<List<Question>> LoadAllQuestionsAsync()
        {
            string folder =Quiz.GetQuizFolderPath();
            var allQuestions = new List<Question>();
            var files = Directory.GetFiles(folder, "*.json");
            foreach (var file in files)
            {
                try
                {
                    string json = await File.ReadAllTextAsync(file);
                    var quiz = JsonSerializer.Deserialize<Quiz>(json);
                    if (quiz?.myQuestions != null)
                    {
                        allQuestions.AddRange(quiz.myQuestions);
                    }
                }
                catch { }
            }
            return allQuestions;

        }
        public void BuildQuizByCategory()
        {
            if (AllQuestions == null || !AllQuestions.Any())
                throw new InvalidOperationException("No questions in menory.");
            var filteredQuestions = AllQuestions.
                Where(q => SelectedCategories.Contains(q.Category)).ToList();
            if (!filteredQuestions.Any())
                throw new InvalidOperationException("No questions for the selected categories.");
            var uniqueQuestions = filteredQuestions
                .GroupBy(q =>new { q.Statement, q.CorrectAnswer })
                .Select(g => g.First())
                .ToList();

            var tempQuiz = new Quiz("Custom Quiz");
            foreach (var question in uniqueQuestions)
            {
                var newQuestion = new Question(
                question.Statement,
                question.CorrectAnswer,
                question.Answers.ToArray(),
                question.Category,
                question.ImagePath
                );
                tempQuiz.myQuestions.Add(newQuestion);
            }
            
               
            Quiz = tempQuiz;


            CurrentQuestion = Quiz.myQuestions.FirstOrDefault();
            SelectedAnswerIndex = -1;
            TotalAnswered = 0;
            CorrectAnswers = 0;
            
            OnPropertyChanged(nameof(Quiz));
            OnPropertyChanged(nameof(CurrentQuestion));
            OnPropertyChanged(nameof(ScoreText));

            MessageBox.Show($"Filtered questions count: {filteredQuestions.Count}");
            MessageBox.Show($"Unique questions count: {uniqueQuestions.Count}");


        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name ="")
        {
           if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
      
        public void NextQuestion(int selectedIndex)
        {
            if (CurrentQuestion == null || Quiz == null || IsQuizCompleted)
            {
                MessageBox.Show($"Quiz completed!Your score: {CorrectAnswers}/{TotalAnswered}", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if(remainingQuestions == null)
            {
                remainingQuestions = Quiz.myQuestions.ToList();
            }
            if(CurrentQuestion==null && remainingQuestions.Count>0)
                CurrentQuestion = remainingQuestions[0];
            if(CurrentQuestion==null)
                return;
            TotalAnswered++;
            if (CurrentQuestion.IsCorrect(selectedIndex))
                CorrectAnswers++;
            remainingQuestions.Remove(CurrentQuestion);
            if (remainingQuestions.Count==0)
            {
                IsQuizCompleted = true;
                CurrentQuestion = null;
                OnPropertyChanged(nameof(CurrentQuestion));
                OnPropertyChanged(nameof(ScoreText));
                MessageBox.Show($"Quiz completed!\\nFinal score: {CorrectAnswers}/{TotalAnswered}", "Quiz Completed", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            //if (TotalAnswered >= Quiz.myQuestions.Count)
            //{
            //    IsQuizCompleted = true;
            //    CurrentQuestion = null;
            //    OnPropertyChanged(nameof(CurrentQuestion));
            //    OnPropertyChanged(nameof(ScoreText));
            //    MessageBox.Show($"Quiz completed!\\nFinal score: {CorrectAnswers}/{TotalAnswered}", "Quiz Completed", MessageBoxButton.OK, MessageBoxImage.Information);
            //    return;
            //}
            int index = Quiz.Randomizer.Next(remainingQuestions.Count);
            CurrentQuestion = remainingQuestions[index];
            SelectedAnswerIndex = -1;
            OnPropertyChanged("CurrentQuestion");
            OnPropertyChanged("ScoreText");
            
        }
        //public void DebugCurrentQuestion()
        //{
        //    if (CurrentQuestion != null)
        //    {
        //        Debug.WriteLine($"Statement: {CurrentQuestion.Statement}");
        //        Debug.WriteLine($"Answers: {string.Join(", ", CurrentQuestion.Answers)}");
        //    }
        //    else
        //    {
        //        Debug.WriteLine("CurrentQuestion is null");
        //    }
        //}

    }
}
