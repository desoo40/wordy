using Newtonsoft.Json;

public static class Utils
{
    static string usersPath = "database\\users.json";
    public static string CustomRead()
    {
        return Console.ReadLine().Trim();
    }

    public static void GetUsersBase(Dictionary<string, string> users)
    {
        try
        {
            var json = JsonConvert.SerializeObject(users);
            File.WriteAllText(usersPath, json);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public static Dictionary<string, string> UpdateUsersBase()
    {
        if (File.Exists(usersPath))
        {
            var jsonText = File.ReadAllText(usersPath);
            var jsonRes = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonText);

            return jsonRes;
        }
        return null;
    }
}