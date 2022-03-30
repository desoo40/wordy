// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;

var filePath = "mydict.json";
var jsonText = File.ReadAllText(filePath);
var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonText);

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
            case "0":
                return;
        }

    }
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