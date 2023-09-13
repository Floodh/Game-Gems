
using System;

class MainClass
{
    static int Main(string[] args)
    {
        Console.WriteLine("Start");
        using GameWindow game = new GameWindow();
        game.Run();
        Console.WriteLine("End");
        return 0;
    }

}


