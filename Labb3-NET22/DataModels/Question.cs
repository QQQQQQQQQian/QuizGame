using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Windows.Media.Imaging;

namespace Labb3_NET22.DataModels;

public class Question:INotifyPropertyChanged
{
    public string Statement { get; set; } = string.Empty;
    public string[] Answers { get; set; } 
    public int CorrectAnswer { get; set; }
    public string Category { get; set; } = "General";
    public string ImagePath { get; set; }
    public int CorrectAnswerIndex { get; set; }

    public string Answer1
    {
        get => Answers.Length > 0 ? Answers[0] : "";
        set { Answers[0] = value; OnPropertyChanged(nameof(Answer1)); }
    }
    public string Answer2
    {
        get => Answers.Length > 1 ? Answers[1] : "";
        set { Answers[1] = value; OnPropertyChanged(nameof(Answer2)); }
    }
    public string Answer3
    {
        get => Answers.Length > 2 ? Answers[2] : "";
        set { Answers[2] = value; OnPropertyChanged(nameof(Answer3)); }
    }
    public ObservableCollection<string> AnswerOptions {  get; set; }= new ObservableCollection<string>();

    public Question() { }
    [JsonIgnore]
    public BitmapImage? ImagePreview
    {
        get
        {
            if (string.IsNullOrEmpty(ImagePath) || !File.Exists(ImagePath))
                return null;

            try
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(ImagePath, UriKind.Absolute);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.DecodePixelWidth = 200;
                bitmap.EndInit();
                return bitmap;
            }
            catch
            {
                return null;
            }
        }
    }

    public Question(string statement,  int correctAnswer, string[] answers, string category ="General", string imagePath=null)
    {
        Statement = statement;
        Answers = answers;
        CorrectAnswer = correctAnswer;
        Category = category;
        ImagePath = imagePath;
    }
    public bool IsCorrect(int selectedIndex)
    {
        return selectedIndex == CorrectAnswer;
    }
    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string name = "")
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
