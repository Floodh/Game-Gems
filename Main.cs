
using System;

namespace TDDD23_MonogameProject;

class MainClass
{
    static int Main(string[] args)
    {
        Console.WriteLine("Start");
        using Game1 game = new Game1();
        game.Run();
        Console.WriteLine("End");
        return 0;
    }

}


