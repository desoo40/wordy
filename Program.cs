// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;


var userName = Login();

string Login()
{
    Console.WriteLine("Enter your login:");
    return CustomRead();
}

var dictPath = $"{userName}.json";
var statPath = $"{userName}_stat.json";
var dict = new Dictionary<string, string>();
var stat = new Dictionary<string, Stat>();

if (File.Exists(dictPath))
{   
    var jsonText = File.ReadAllText(dictPath);
    var jsonTextStat = File.ReadAllText(statPath);
    dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonText);
    stat = JsonConvert.DeserializeObject<Dictionary<string, Stat>>(jsonTextStat);

    if (dict == null)
        dict = new Dictionary<string, string>();
    if (stat == null)
        stat = new Dictionary<string, Stat>();
        
    UpdateStat();
}
else
{
    Console.WriteLine($"You are new user! Welcome {userName}!");
}

Menu();
SaveDictToFile();

void Menu()
{
   while(true)
    {
        Console.WriteLine(
@"Enter action:
    1 - Add word
    2 - Print dict
    3 - Save to file
    4 - Delete word
    5 - Find word
    6 - Learn words
    0 - Exit");

        var ans = CustomRead();

        switch (ans)
        {
            case "1":
                AddWordToDict();
                break;
            case "2":
                PrintDict();
                break;
            case "3":
                SaveDictToFile();
                break;
            case "4":
                DeleteWord();
                break;
            case "5":
                FindWord();
                break;
            case "6":
                Learn();
                break;
            case "erase!":
                dict = new Dictionary<string, string>();
                break;
            case "0":
                return;
        }

    }
}

void Learn()
{
    Console.Write("Set rate: ");
    var rate = Convert.ToDecimal(CustomRead());
    var selectedDict = dict.Where(x => stat[x.Key].LearningRate <= rate);

    var exitWord = ":q";
    var rand = new Random();
    var dictCount = selectedDict.Count();
    var marked = new bool[dictCount];
    var markedCnt = 0;
    var goodAns = 0;
    Console.WriteLine("========LEARNING========");
    while(markedCnt != dictCount)
    {
        var randNum = rand.Next(dictCount);

        if (marked[randNum])
            continue;
        
        marked[randNum] = true;
        markedCnt++;

        var randEl = selectedDict.ElementAt(randNum).Key;
        Console.Write(randEl + " - ");
        var ans = CustomRead();
        if (ans == exitWord)
            break;

        if (!stat.ContainsKey(randEl))
            stat.Add(randEl, new Stat());

        stat[randEl].Asked++;

        if (ans == dict[randEl])
        {
            Console.WriteLine("Good!");
            stat[randEl].TrueAnswers++;
            goodAns++;
        }
        else
        {
            Console.WriteLine("Wrong!");
            Console.WriteLine($"{randEl} - {dict[randEl]}");
        }

    }
    Console.WriteLine($"=========={goodAns} of {dictCount}===========");
    Console.WriteLine("==========EXIT===========");

}

void UpdateStat()
{
    foreach (var el in dict)
        if (!stat.ContainsKey(el.Key))
            stat.Add(el.Key, new Stat());
}

string CustomRead()
{
    return Console.ReadLine().Trim();
}

void FindWord()
{
    Console.WriteLine("Enter word to find:");
    var findWord = CustomRead();
    
    if (dict.ContainsKey(findWord))
        Console.WriteLine($"{findWord} - {dict[findWord]}");
    else 
        Console.WriteLine($"No such word \"{findWord}\" in dictionary!");
}

void AddWordToDict()
{
    while(true)
    {
        Console.Write("Enter word on Eng: ");
        var engWord = CustomRead();
        if (engWord == ":q")
            break;
        Console.Write("Enter word on Rus: ");
        var rusWord = CustomRead();
        
        if (dict.ContainsKey(engWord))
        {
            Console.WriteLine("Dictionary already contains this word!");
            continue;
        }
        if (stat.ContainsKey(engWord))
            continue;

        dict.Add(engWord, rusWord);
        stat.Add(engWord, new Stat());
        SaveDictToFile(false);
    }
    
}

void DeleteWord()
{
    Console.WriteLine("Enter word to delete:");
    var deleteWord = CustomRead();
    
    if (dict.ContainsKey(deleteWord))
    {
        dict.Remove(deleteWord);
        Console.WriteLine($"\"{deleteWord}\" was deleted!");
    }
    else 
        Console.WriteLine($"No such word \"{deleteWord}\" in dictionary!");
}
void SaveDictToFile(bool log=true)
{
    try
    {
        if (log)
            Console.WriteLine("Writing to file...");
        var json = JsonConvert.SerializeObject(dict);
        var json_stat = JsonConvert.SerializeObject(stat);
        File.WriteAllText(dictPath, json);
        File.WriteAllText(statPath, json_stat);
        if (log)
            Console.WriteLine("Dictionary saved!");
    }
    catch(Exception e)
    {
        Console.WriteLine(e);
    }
}

void PrintDict()
{
    Console.WriteLine("-------------------------------");
    Console.WriteLine($"Dictionary words count = {dict.Count}");
    Console.WriteLine("-------------------------------");
    
    foreach(var el in dict)
    {
        if (!stat.ContainsKey(el.Key))
            stat.Add(el.Key, new Stat());

        Console.WriteLine($"{el.Key} - {el.Value} ({stat[el.Key].TrueAnswers}/{stat[el.Key].Asked}) - [{stat[el.Key].LearningRate}]");

    }

    Console.WriteLine("-------------------------------");
}