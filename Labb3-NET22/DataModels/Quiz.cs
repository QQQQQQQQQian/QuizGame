using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Collections.ObjectModel;

namespace Labb3_NET22.DataModels;   

public class Quiz
{
    
    private IEnumerable<Question> _questions;
    private string _title = string.Empty;
    [JsonPropertyName("_QuestionsInternal")]
    public IEnumerable<Question> Questions => _questions;
    public string Title => _title;

    //Add new fields to use Random class for random number generation
    //Add a field to use List <Question> to store questions,because IEnumerable is read-only
    [JsonPropertyName("Questions")]
    public ObservableCollection<Question> myQuestions { get; set; }=new ObservableCollection<Question>();
    public Random Randomizer { get; set; }
    public string FilePath { get; set; } = string.Empty;// Not serialized, used for tracking file path in memory
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
        myQuestions = new ObservableCollection<Question>();

        _questions = myQuestions.ToList();
        Randomizer = new Random();
        
    }
    public void SetTitle(string title)
    {
        _title = title;
    }
    public void SyncQuestions()
    {
        if (myQuestions != null)
        {
            _questions = myQuestions.ToList();
        }
    }
    public Question GetRandomQuestion()
    {
        
        if(myQuestions.Count == 0)
        {
            throw new InvalidOperationException("No questions available");
        }
        _questions = myQuestions.ToList();
        int index = Randomizer.Next(myQuestions.Count);
        //var question = myQuestions[index];
        //myQuestions.RemoveAt(index);
       
        return myQuestions[index];

    }

    public void AddQuestion(string statement, int correctAnswer,string category, string[] answers, string imagePath=null)
    {
        Question q = new Question(statement, correctAnswer, answers,category,imagePath);
        myQuestions.Add(q);
        _questions = myQuestions;
    }
    
    public void RemoveQuestion(int index)
    {
        if(index >=0 && index < myQuestions.Count)
        {
            myQuestions.RemoveAt(index);
            _questions = myQuestions;
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
    public async Task SaveToJsonAsync(string title)
    {
        SyncQuestions();
        string filePath =!string.IsNullOrWhiteSpace(FilePath)
                            ? FilePath :
                            GetQuizFilePath(title);

        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(this, options);
        await File.WriteAllTextAsync(filePath, json);
       
    }
    public static string GetQuizFolderPath()
    {
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string folder = Path.Combine(appDataPath, "QuizGame");
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);
        return folder;
    }
   
    public static async Task <Quiz> LoadFromJsonAsync(string fileName)
    {   
       string filePath =File.Exists(fileName)?fileName: GetQuizFilePath(fileName);
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("The specified quiz file was not found.", filePath);
        }
        string json = await File.ReadAllTextAsync(filePath);

        var quiz = JsonSerializer.Deserialize<Quiz>(json)?? new Quiz();
        if (quiz.Randomizer == null)
        {
            quiz.Randomizer = new Random();
        }


        if(quiz.myQuestions == null)
       
            quiz.myQuestions = new ObservableCollection<Question>();


        quiz._questions = quiz.myQuestions.ToList();
        quiz.FilePath = filePath;
        return quiz;
    }
    
}
//%localappdata%\QuizGame