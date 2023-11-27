using System.IO;

static class Save
{
    private const string path = "Save/HighscoreNight.txt";

    public static int HighscoreNight {get; set;} = 0;

    public static void Load()
    {
        HighscoreNight = 0;
        if (File.Exists(path))
        {
            try
            {
                string text = File.ReadAllText(path);
                HighscoreNight = int.Parse(text);
            }
            catch
            {
                HighscoreNight = 0;
            }
        }     

            

        
    }

    public static void WriteToFile()
    {
        File.WriteAllText(path, HighscoreNight.ToString());
    }

}