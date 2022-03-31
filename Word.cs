public class Word
{
    public string EngWord {get; set;}
    public string RusWord {get; set;}

    public int timesAsked;
    public int correctAnswers;

    public Word(string en, string ru)
    {
        EngWord = en;
        RusWord = ru;
        timesAsked = 0;
        correctAnswers = 0;
    }

    public override string ToString()
    {
        return EngWord + " - " + RusWord;
    }
}