
using System;
using System.Drawing;
using Microsoft.Xna.Framework;

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


