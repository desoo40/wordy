public class Word
{
    public string EngWord {get; set;}
    public string RusWord {get; set;}

    public int engToRusTimeAsked;
    public int engToRusCorrectAnswers;
    public int rusToEngTimeAsked;
    public int rusToEngCorrectAnswers;

    public decimal RusToEngLearningRate
    {
        get => rusToEngTimeAsked == 0 ? 0 : (decimal)rusToEngCorrectAnswers / rusToEngTimeAsked;
    }

    public decimal EngToRusLearningRate
    {
        get => engToRusTimeAsked == 0 ? 0 : (decimal)engToRusCorrectAnswers / engToRusTimeAsked;
    }

    public Word(string en, string ru)
    {
        EngWord = en;
        RusWord = ru;
        engToRusTimeAsked = 0;
        rusToEngTimeAsked = 0;
    }

    public override string ToString()
    {
        return EngWord + " - " + RusWord;
    }
}