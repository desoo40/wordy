// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;

var userName = Login();
var userPath = $"database/{userName}.json";
var genDict = new WordsDict(userPath);

Menu();
genDict.SaveToFile(userPath);

string Login()
{
    Console.WriteLine("Enter your login:");
    return Utils.CustomRead();
}

void Menu()
{
    while (true)
    {
        Console.WriteLine(
@"Enter action:
    1 - Add word
    2 - Print dict
    3 - Save to file
    4 - Delete word
    5 - Find word
    6 - Learn words (Mixed)
    7 - Learn words (Rus to Eng)
    8 - Learn words (Eng to Rus)
    0 - Exit");

        var ans = Utils.CustomRead();

        switch (ans)
        {
            case "1":
                AddWordToDict();
                break;
            case "2":
                genDict.Print();
                break;
            case "3":
                genDict.SaveToFile(userPath);
                break;
            case "4":
                DeleteWord();
                break;
            case "5":
                FindWord();
                break;
            case "6":
                Learn(LangMode.Mixed);
                break;
            case "7":
                Learn(LangMode.RusToEn);
                break;
            case "8":
                Learn(LangMode.EnToRus);
                break;
            case "0":
                return;
        }

        genDict.SaveToFile(userPath);
    }
}

void DeleteWord()
{
    Console.WriteLine("Enter word to delete:");
    var deleteWord = Utils.CustomRead();
    var res = genDict.DeleteWord(deleteWord);

    if (res)
        Console.WriteLine($"\"{deleteWord}\" was deleted!");
    else
        Console.WriteLine($"No such word \"{deleteWord}\" in dictionary!");
}

void AddWordToDict()
{
    while (true)
    {
        Console.Write("Enter word on Eng: ");
        var engWord = Utils.CustomRead();
        if (engWord == ":q")
            break;

        Console.Write("Enter word on Rus: ");
        var rusWord = Utils.CustomRead();
        if (rusWord == ":q")
            break;

        if (genDict.Find(engWord) != null)
        {
            Console.WriteLine("Dictionary already contains this word!");
            continue;
        }

        var res = genDict.Add(engWord, rusWord);

        if (res)
            Console.WriteLine("Added to dictionary!");
        else
            Console.WriteLine("Something go wrong!");

        genDict.SaveToFile(userPath, false);
    }
}

void FindWord()
{
    Console.WriteLine("Enter word to find:");
    var input = Utils.CustomRead();
    var word = genDict.Find(input);

    if (word == null)
        Console.WriteLine($"No such word \"{input}\" in dictionary!");
    else
        Console.WriteLine(word);
}

void Learn(LangMode mode)
{
    try
    {
        var exitWord = ":q";

        Console.Write("Set rate: ");
        var rate = Convert.ToDecimal(Utils.CustomRead());

        Console.Write("Set limit: ");
        var lim = Convert.ToInt32(Utils.CustomRead());

        var learningWords = genDict.GetRandomNElemsUnderRate(lim, rate, mode);
        var wordsCount = learningWords.Count;
        var rounds = 1;
        var exit = false;
        var rand = new Random();

        Console.WriteLine("========LEARNING========");
        while (wordsCount != 0)
        {
            Console.WriteLine($"========ROUND #{rounds}========");
            Console.WriteLine($"====WORDS TO LEARN {wordsCount}====");

            var marked = new bool[wordsCount];
            var markedCnt = 0;
            var goodAns = 0;

            var wordsToDel = new List<Word>();

            while (markedCnt != wordsCount)
            {
                var randNum = rand.Next(wordsCount);

                if (marked[randNum])
                    continue;

                marked[randNum] = true;
                markedCnt++;

                var tmpMode = LangMode.EnToRus;
                if (mode == LangMode.Mixed && randNum % 2 == 0)
                    tmpMode = LangMode.RusToEn;

                var randEl = learningWords.ElementAt(randNum);

                var askWord = randEl.EngWord;
                var ansWord = randEl.RusWord;
                var tmpAsked = randEl.engToRusAsked;
                var tmpTrue = randEl.engToRusTrueAnswers;

                if (tmpMode != LangMode.EnToRus)
                {
                    askWord = randEl.RusWord;
                    ansWord = randEl.EngWord;
                    tmpAsked = randEl.rusToEngAsked;
                    tmpTrue = randEl.rusToEngTrueAnswers;
                }

                Console.Write($"[{markedCnt}/{wordsCount}]:\n" + askWord + " - ");

                var ans = Utils.CustomRead();
                if (ans == exitWord)
                {
                    exit = true;
                    break;
                }

                tmpAsked++;

                if (ans == ansWord)
                {
                    tmpTrue++;
                    Console.WriteLine($"Good! [{tmpTrue} of {tmpAsked}]");
                    goodAns++;
                    wordsToDel.Add(randEl);
                }
                else
                {
                    Console.WriteLine($"Wrong! [{tmpTrue} of {tmpAsked}]");
                    Console.WriteLine($"{askWord} - {ansWord}");
                }

                if (tmpMode == LangMode.EnToRus)
                {
                    randEl.EngWord = askWord;
                    randEl.RusWord = ansWord;
                    randEl.engToRusAsked = tmpAsked;
                    randEl.engToRusTrueAnswers = tmpTrue;
                }
                else
                {
                    randEl.RusWord = askWord;
                    randEl.EngWord = ansWord;
                    randEl.rusToEngAsked = tmpAsked;
                    randEl.rusToEngTrueAnswers = tmpTrue;
                }
            }

            learningWords = learningWords.Where(x => !wordsToDel.Contains(x)).ToList();
            Console.WriteLine($"=========={goodAns} of {wordsCount}===========");
            wordsCount = learningWords.Count;

            if (exit)
                break;
        }

        Console.WriteLine("==========EXIT===========");
    }
    catch (Exception e)
    {
        Console.WriteLine($"Cought some exeption {e}");
        Console.WriteLine("==========EXIT===========");
        return;
    }
}