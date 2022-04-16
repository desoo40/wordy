
using Newtonsoft.Json;

public class WordsDict
{
    public List<Word> Words = new List<Word>();
    public int Count => Words.Count;
    public WordsDict()
    {

    }

    public WordsDict(string path)
    {
        UploadFromFile(path);
    }

    void UploadFromFile(string path)
    {
        Console.WriteLine($"Trying upload from {path}...");
        if (File.Exists(path))
        {
            var jsonText = File.ReadAllText(path);
            var jsonRes = JsonConvert.DeserializeObject<List<Word>>(jsonText);

            if (jsonRes != null)
            {
                Words = jsonRes;
                Console.WriteLine("Upload ended!");
            }
        }

        Console.WriteLine("No such file!");
    }

    public void SaveToFile(string path, bool log = true)
    {
        try
        {
            if (log)
                Console.WriteLine("Writing to file...");
            var json = JsonConvert.SerializeObject(Words);
            File.WriteAllText(path, json);
            if (log)
                Console.WriteLine("Dictionary saved!");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public Word Find(string findWord)
    {
        var res = Words.Where(x => x.EngWord == findWord);
        if (res.Any())
            return res.First();
        else
            return null;
    }

    public bool Add(string eng, string rus)
    {
        if (Find(eng) == null)
        {
            Words.Add(new Word(eng, rus));
            return true;
        }
        return false;
    }


    public bool DeleteWord(string delWord)
    {
        var word = Find(delWord);
        if (word == null)
            return false;

        return Words.Remove(word);
    }

    public List<Word> GetRandomNElemsUnderRate(int lim, decimal rate)
    {
        var subWords = Words.Where(x => x.LearnRate <= rate).ToList();
        var randRange = subWords.Count;

        if (lim >= randRange)
            return subWords;

        var kek = new Random();
        var visited = new HashSet<int>();
        var resList = new List<Word>();

        var i = 0; // for continue
        while (i < lim)
        {
            var currNum = kek.Next(randRange);
            if (visited.Contains(currNum))
                continue;

            resList.Add(subWords[currNum]);
            visited.Add(currNum);
            ++i;
        }

        return resList;
    }

    public void Print()
    {
        Console.WriteLine("-------------------------------");

        foreach (var el in Words)
            Console.WriteLine($"{el}");

        Console.WriteLine("-------------------------------");
        Console.WriteLine($"Dictionary words count = {Count}");
        Console.WriteLine("-------------------------------");
    }
}