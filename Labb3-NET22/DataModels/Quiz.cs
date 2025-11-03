using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.IO;

namespace Labb3_NET22.DataModels;   

public class Quiz
{
    private IEnumerable<Question> _questions;
    private string _title = string.Empty;
    public IEnumerable<Question> Questions => _questions;
    public string Title => _title;

    //Add new fields to use Random class for random number generation
    //Add a field to use List <Question> to store questions,because IEnumerable is read-only
    private List<Question> myQuestions;
    public Random Randomizer { get; set; }
    public List<string> GetCategories()
    {
        if(myQuestions == null || !myQuestions.Any())
            return new List<string>();
        return myQuestions
                .Select(q=>q.Category)
                .Where(c=>!string.IsNullOrWhiteSpace(c))
                .Distinct()
                .OrderBy(c=>c).ToList();

    }
    public Quiz(string title ="")
    {
        _title = title;
        myQuestions = new List<Question>();
        
        _questions = new List<Question>();
        Randomizer = new Random();
        
    }
    public void SetTitle(string title)
    {
        _title = title;
    }

    public Question GetRandomQuestion()
    {
        if(myQuestions.Count == 0)
        {
            throw new InvalidOperationException("No questions available");
        }
        int index = Randomizer.Next(0, myQuestions.Count);
        return myQuestions[index];
    }

    public void AddQuestion(string statement, int correctAnswer,string category, params string[] answers)
    {
        Question q = new Question(statement, correctAnswer, answers ,category);
        myQuestions.Add(q);
        _questions = myQuestions.ToList();
    }
    
    public void RemoveQuestion(int index)
    {
        if(index >=0 && index < myQuestions.Count)
        {
            myQuestions.RemoveAt(index);
            _questions = myQuestions.ToList();
        }
    }
    public static string GetQuizFilePath(string title)
    {
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);// Get the path to the local application data folder
        string folder = Path.Combine(appDataPath, "QuizGame"); // Combine it with your application's folder name
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder); // Create the directory if it doesn't exist
        }
        return Path.Combine(folder,  $"{title}.json");
    }
    public void SaveToJson()
    {
        
        string filePath =GetQuizFilePath(this.Title);

        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(this, options);
        File.WriteAllText(filePath, json);
    }
    public static Quiz LoadFromJson(string fileName)
    {   
        // Implementation for loading quiz from JSON file
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);// Get the path to the local application data folder
        string folder = Path.Combine(appDataPath, "QuizGame"); // Combine it with your application's folder name
        string filePath= Path.Combine(folder, Path.GetFileName(fileName));
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("The specified quiz file was not found.", filePath);
        }
        string json = File.ReadAllText(filePath);
        Quiz quiz = JsonSerializer.Deserialize<Quiz>(json);
        if(quiz.Randomizer == null)
        {
            quiz.Randomizer = new Random();
        }
        if(quiz._questions != null && quiz._questions.Any())
        {
            quiz.myQuestions = quiz._questions.ToList();
        }
        else if (quiz.myQuestions == null)
        {
            quiz.myQuestions = new List<Question>();
            quiz._questions = quiz.myQuestions;
        }else
        {
            quiz._questions = quiz.myQuestions;
        }

        return quiz;
    }
    
}