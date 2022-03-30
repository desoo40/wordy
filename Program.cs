// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;

var userName = Login();

string Login()
{
    Console.WriteLine("Enter your login:");
    return Console.ReadLine();
}

var filePath = $"{userName}.json";
var dict = new Dictionary<string, string>();

if (File.Exists(filePath))
{   
    var jsonText = File.ReadAllText(filePath);
    dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonText);
}
else
{
    Console.WriteLine($"You are new user! Welcome {userName}!");
}

Menu(dict);
SaveDictToFile(dict);

void AddWordToDict(Dictionary<string, string> dict)
{
    Console.WriteLine("Enter word on Eng: ");
    var engWord = Console.ReadLine();
    
    Console.WriteLine("Enter word on Rus: ");
    var rusWord = Console.ReadLine();

    dict.Add(engWord, rusWord);
}

void Menu(Dictionary<string, string> dict)
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

        var ans = Console.ReadLine();

        switch (ans)
        {
            case "1":
                AddWordToDict(dict);
                break;
            case "2":
                PrintDict(dict);
                break;
            case "3":
                SaveDictToFile(dict);
                break;
            case "4":
                DeleteWord(dict);
                break;
            case "5":
                FindWord(dict);
                break;
            case "6":
                Learn(dict);
                break;
            case "0":
                return;
        }

    }
}

void Learn(Dictionary<string, string> dict)
{
}

void FindWord(Dictionary<string, string> dict)
{
    Console.WriteLine("Enter word to find:");
    var findWord = Console.ReadLine();
    
    if (dict.ContainsKey(findWord))
        Console.WriteLine($"{findWord} - {dict[findWord]}");
    else 
        Console.WriteLine($"No such word \"{findWord}\" in dictionary!");
}

void DeleteWord(Dictionary<string, string> dict)
{
    Console.WriteLine("Enter word to delete:");
    var deleteWord = Console.ReadLine();
    
    if (dict.ContainsKey(deleteWord))
    {
        dict.Remove(deleteWord);
        Console.WriteLine($"\"{deleteWord}\" was deleted!");
    }
    else 
        Console.WriteLine($"No such word \"{deleteWord}\" in dictionary!");
}
void SaveDictToFile(Dictionary<string, string> dict)
{
    try
    {
        Console.WriteLine("Writing to file...");
        var json = JsonConvert.SerializeObject(dict);
        File.WriteAllText(filePath, json);
        Console.WriteLine("Dictionary saved!");
    }
    catch(Exception e)
    {
        Console.WriteLine(e);
    }
}

void PrintDict(Dictionary<string, string> dict)
{
    Console.WriteLine("-------------------------------");
    Console.WriteLine($"Dictionary words count = {dict.Count}");
    Console.WriteLine("-------------------------------");
    
    foreach(var el in dict)
        Console.WriteLine($"{el.Key} - {el.Value}");

    Console.WriteLine("-------------------------------");
}