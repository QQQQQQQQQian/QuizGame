namespace Labb3_NET22.DataModels;

public class Question
{
    public string Statement { get; }
    public string[] Answers { get; }
    public int CorrectAnswer { get; }
    public string Category { get; set; }


    public Question(string statement,  int correctAnswer, string[] answers, string category ="General")
    {
        Statement = statement;
        Answers = answers;
        CorrectAnswer = correctAnswer;
        Category = category;
    }
    public bool IsCorrect(int selectedIndex)
    {
        return selectedIndex == CorrectAnswer;
    }
}
