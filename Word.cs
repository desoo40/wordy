using Newtonsoft.Json;

public class Word
{
    public string EngWord { get; set; }
    public string RusWord { get; set; }

    public int engToRusAsked = 0;
    public int engToRusTrueAnswers = 0;
    public int rusToEngAsked = 0;
    public int rusToEngTrueAnswers = 0;

    [JsonIgnore]
    public decimal RusToEngLearningRate
    {
        get => rusToEngAsked == 0 ? 0 : (decimal)rusToEngTrueAnswers / rusToEngAsked;
    }

    [JsonIgnore]
    public decimal EngToRusLearningRate
    {
        get => engToRusAsked == 0 ? 0 : (decimal)engToRusTrueAnswers / engToRusAsked;
    }

    [JsonIgnore]
    public decimal LearnRate
    {
        get => (EngToRusLearningRate + RusToEngLearningRate) / 2;
    }

    public Word(string en, string ru)
    {
        EngWord = en;
        RusWord = ru;
        engToRusAsked = 0;
        rusToEngAsked = 0;
    }

    public override string ToString()
    {
        return
@$"{EngWord} - {RusWord}

Eng to Rus - {engToRusTrueAnswers} of {engToRusAsked} [{string.Format("{0:0.00}", EngToRusLearningRate)}]
Rus to Eng - {rusToEngTrueAnswers} of {rusToEngAsked} [{string.Format("{0:0.00}", RusToEngLearningRate)}]
LR - {string.Format("{0:0.00}", LearnRate)}";

    }
}