public class Stat
{
    public int Asked {get; set;}
    public int TrueAnswers {get; set;}
    public decimal LearningRate
    {
        get => Asked == 0 ? 0 : (decimal)TrueAnswers / Asked;
    }

    public Stat()
    {
        Asked = 0;
        TrueAnswers = 0;
    }
}