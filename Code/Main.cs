
using System;
using Microsoft.Xna.Framework;

class MainClass
{
    static int Main(string[] args)
    {
        Console.WriteLine("Start");

        //EnergyBeam energyBeam = new(new Point(8,8), new Point(1,2), EnergyBeam.Type.Line);


        using GameWindow game = new GameWindow();
        game.Run();

        Console.WriteLine("End");
        return 0;
    }

}


