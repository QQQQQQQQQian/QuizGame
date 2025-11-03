using Labb3_NET22.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Labb3_NET22
{
    public class PlayQuizViewModel:INotifyPropertyChanged
    {
        public Quiz Quiz { get;set; }
        public Question CurrentQuestion { get;set; }
        public int SelectedAnswerIndex { get;set; }
        public int CorrectAnswers { get;set; }
        public int TotalAnswered{ get;set; }

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
        public List<string> SelectedCategories { get; set; } = new List<string>();
        public void LoadQuestionsByCategory()
        {
            var filteredQuestions = Quiz.Questions
                .Where(q => SelectedCategories.Contains(q.Category))
                .ToList();
            if (filteredQuestions.Count > 0)
            {
                Quiz = new Quiz(Quiz.Title);
                foreach (var question in filteredQuestions)
                {
                    Quiz.AddQuestion(question.Statement, question.CorrectAnswer, question.Category, question.Answers.ToArray());
                }
                CurrentQuestion= Quiz.GetRandomQuestion();
                SelectedAnswerIndex = -1;
                TotalAnswered = 0;
                CorrectAnswers = 0;
                OnPropertyChanged("CurrentQuestion");
                OnPropertyChanged("ScoreText");
            }
            else
            {
                throw new InvalidOperationException("No questions available for the selected categories.");
            }
        }
        public PlayQuizViewModel()
        {
            Quiz = new Quiz();
           
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name ="")
        {
           if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        public void NextQuestion(int selectedIndex)
        {
            TotalAnswered++;
            if (CurrentQuestion.IsCorrect(selectedIndex))
            {
                CorrectAnswers++;
            }
            
            CurrentQuestion = Quiz.GetRandomQuestion();
            OnPropertyChanged("CurrentQuestion");
            OnPropertyChanged("ScoreText");
            
        }
        
    }
}
